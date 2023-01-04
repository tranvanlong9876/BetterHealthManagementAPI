using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Employee;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Employee
{
    public class EmployeeAuthService : IEmployeeAuthService
    {
        private readonly IEmployeeAuthRepo _employeeAuthRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;

        public EmployeeAuthService(IEmployeeAuthRepo employeeAuthRepo, IDynamicAddressRepo dynamicAddressRepo)
        {
            _employeeAuthRepo = employeeAuthRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
        }

        public async Task<EmployeeTokenModel> Login(LoginEmployee loginEmployee)
        {
            Repository.DatabaseModels.Employee employee = await _employeeAuthRepo.CheckLogin(loginEmployee);

            if(employee == null) throw new ArgumentException("Không tìm thấy tài khoản của nhân viên.");
            if (employee.Status == 0) throw new ArgumentException("Tài khoản nhân viên đã ngưng kích hoạt, vui lòng liên hệ Admin để được hỗ trợ.");

            byte[] passwordHashByte = PasswordHash.GetByteFromString(employee.Password);
            byte[] passwordSaltByte = PasswordHash.GetByteFromString(employee.PasswordSalt);

            var check = PasswordHash.VerifyPasswordHash(loginEmployee.Password.Trim(), passwordHashByte, passwordSaltByte);

            if (!check) throw new ArgumentException("Mật khẩu đăng nhập không đúng.");

            string token = JwtUserToken.CreateEmployeeToken(employee);

            EmployeeTokenModel employeeTokenModel = new()
            {
                Id = employee.Id,
                Name = employee.Fullname,
                Email = employee.Email,
                ImageURL = employee.ImageUrl,
                Status = employee.Status,
                RoleId = employee.RoleId,
                RoleName = employee.Role.RoleName,
                Token = token
            };

            return employeeTokenModel;
        }

        public async Task<List<Repository.DatabaseModels.Employee>> GetEmployeeById(string id)
        {
            //để return thử
            List<Repository.DatabaseModels.Employee> list = new List<Repository.DatabaseModels.Employee>();
            return list;// ko mà nãy t sử lý qua repo 
            //hạn chế đổi tên folder, mốt thêm file thôi.
        }

        public async Task<RegisterEmployeeStatus> Register(RegisterEmployee employee)
        {
            var check = false;
            var checkError = new RegisterEmployeeStatus();
            var isMatches = employee.Password.Trim().Equals(employee.ConfirmPassword.Trim());
            if (!isMatches)
            {
                checkError.isError = true;
                checkError.ConfirmPasswordFailed = "Mật khẩu xác nhận không trùng khớp.";
            }

            if (await _employeeAuthRepo.CheckDuplicateUsername(employee.Username)) {
                checkError.isError = true;
                checkError.DuplicateUsername = "Tài khoản nhân viên đã tồn tại, vui lòng nhập tài khoản khác.";
            }
            if (await _employeeAuthRepo.CheckDuplicateEmail(employee.Email, false))
            {
                checkError.isError = true;
                checkError.DuplicateEmail = "Email nhân viên đã tồn tại.";
            }
            if (await _employeeAuthRepo.CheckDuplicatePhoneNo(employee.PhoneNo, false))
            {
                checkError.isError = true;
                checkError.DuplicatePhoneNo = "Số điện thoại nhân viên đã tồn tại.";
            }

            if (checkError.isError) return checkError;

            //create empCode.
            var lastestEmpCode = await _employeeAuthRepo.GetLatestEmployeeCode(); //PM0100002
            int lastestCode = Convert.ToInt32(lastestEmpCode.Substring(4, 5)); //00002 -> 2
            int newCode = lastestCode + 1; //-> 3

            var currentLength = (lastestCode).ToString().Length; // length = 1
            string newEmployeeCode = String.Empty;
            if (employee.RoleId == "1") newEmployeeCode += "MN01";
            if (employee.RoleId == "2") newEmployeeCode += "PM01";
            if (currentLength < 5)
            {
                for(int i = currentLength; i < 5; i++)
                {
                    newEmployeeCode += "0";
                }
            }
            newEmployeeCode += newCode.ToString();

            //create encrypted Password for Employee.
            PasswordHash.CreatePasswordHash(employee.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var empID = Guid.NewGuid().ToString();
            var addressID = Guid.NewGuid().ToString();
            var insertAddress = false;

            //check if need to insert Employee's Address?
            if(employee.CityID != null && employee.DistrictID != null && employee.WardID != null && employee.HomeNumber != null) {
                Repository.DatabaseModels.DynamicAddress dynamicAddress = new()
                {
                    Id = addressID,
                    CityId = employee.CityID,
                    DistrictId = employee.DistrictID,
                    WardId = employee.WardID,
                    HomeAddress = employee.HomeNumber
                };
                await _dynamicAddressRepo.InsertNewAddress(dynamicAddress);
                insertAddress = true;
            } //done insert employee's address.

            Repository.DatabaseModels.Employee employeeModel = new()
            {
                Id = empID,
                Code = newEmployeeCode,
                Username = employee.Username,
                Fullname = employee.Fullname,
                PhoneNo = employee.PhoneNo,
                Email = employee.Email,
                AddressId = insertAddress ? addressID : null,
                ImageUrl = employee.ImageUrl,
                Dob = employee.DOB,
                Password = Convert.ToBase64String(passwordHash).Trim(),
                PasswordSalt = Convert.ToBase64String(passwordSalt).Trim(),
                RoleId = employee.RoleId,
                SiteId = employee.SiteId,
                Status = employee.Status,
                Gender = employee.Gender  
            };

            check = await _employeeAuthRepo.RegisterEmployee(employeeModel);

            if (check) {
                checkError.isError = false;
            } else
            {
                checkError.isError = true;
                checkError.OtherError = "Hệ thống đang bị lỗi, vui lòng thử lại sau.";
            }

            await EmailService.SendWelcomeEmail(employee, $"Chào mừng {employee.Fullname} về đội của chúng tôi.", true);

            return checkError;
        }
    }
}
