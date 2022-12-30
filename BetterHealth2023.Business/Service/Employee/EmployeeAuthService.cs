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
        private readonly IEmployeeInfoRepo _employeeInfoRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;

        public EmployeeAuthService(IEmployeeAuthRepo employeeAuthRepo, IEmployeeInfoRepo employeeInfoRepo, IDynamicAddressRepo dynamicAddressRepo)
        {
            _employeeAuthRepo = employeeAuthRepo;
            _employeeInfoRepo = employeeInfoRepo;
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

            Repository.DatabaseModels.EmployeeInfo employeeInfo = await _employeeInfoRepo.GetEmployeeInfo(employee.Id);

            string token = JwtUserToken.CreateEmployeeToken(employee, employeeInfo);

            EmployeeTokenModel employeeTokenModel = new()
            {
                Id = employee.Id,
                Name = employeeInfo.Fullname,
                Email = employeeInfo.Email,
                ImageURL = employeeInfo.ImageUrl,
                Status = employee.Status,
                RoleId = employee.RoleId,
                RoleName = employee.Role.RoleName,
                Token = token
            };

            return employeeTokenModel;
        }

        public async Task<RegisterEmployeeStatus> Register(RegisterEmployee employee)
        {
            var check = false;
            var employeeID = string.Empty;
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
            if (await _employeeInfoRepo.CheckDuplicateEmail(employee.Email, false))
            {
                checkError.isError = true;
                checkError.DuplicateEmail = "Email nhân viên đã tồn tại.";
            }
            if (await _employeeInfoRepo.CheckDuplicatePhoneNo(employee.PhoneNo, false))
            {
                checkError.isError = true;
                checkError.DuplicatePhoneNo = "Email nhân viên đã tồn tại.";
            }

            if (checkError.isError) return checkError;

            PasswordHash.CreatePasswordHash(employee.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var id = Guid.NewGuid().ToString();
            var addressID = Guid.NewGuid().ToString();
            var insertAddress = false;
            Repository.DatabaseModels.Employee employeeModel = new ()
            {
                Id = id,
                Username = employee.Username,
                Password = Convert.ToBase64String(passwordHash).Trim(),
                PasswordSalt = Convert.ToBase64String(passwordSalt).Trim(),
                RoleId = employee.RoleId,
                SiteId = employee.SiteId,
                Status = employee.Status
            };

            employeeID = await _employeeAuthRepo.RegisterEmployee(employeeModel);
            //done Insert Employee.

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

            Repository.DatabaseModels.EmployeeInfo employeeInfoModel = new()
            {
                EmployeeId = employeeID,
                Fullname = employee.Fullname,
                PhoneNo = employee.PhoneNo,
                Email = employee.Email,
                AddressId = insertAddress ? addressID : null,
                ImageUrl = employee.ImageUrl,
                Dob = employee.DOB
            };

            check = await _employeeInfoRepo.InsertEmployeeInfo(employeeInfoModel);

            if (check) {
                checkError.isError = false;
            } else
            {
                checkError.isError = true;
                checkError.OtherError = "Hệ thống đang bị lỗi, vui lòng thử lại sau.";
            }
            return checkError;
        }
    }
}
