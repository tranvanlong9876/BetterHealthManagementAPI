using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.InternalUserAuthRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UserWorkingSiteRepos;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalUser
{
    public class InternalUserAuthService : IInternalUserAuthService
    {
        private readonly IInternalUserAuthRepo _employeeAuthRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;
        private readonly IUserWorkingSiteRepo _userWorkingSiteRepo;

        public InternalUserAuthService(IInternalUserAuthRepo employeeAuthRepo, IDynamicAddressRepo dynamicAddressRepo, IUserWorkingSiteRepo userWorkingSiteRepo)
        {
            _employeeAuthRepo = employeeAuthRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
            _userWorkingSiteRepo = userWorkingSiteRepo;
        }

        public async Task<InternalUserTokenModel> Login(LoginInternalUser loginEmployee)
        {
            Repository.DatabaseModels.InternalUser user = await _employeeAuthRepo.CheckLogin(loginEmployee);

            if(user == null) throw new ArgumentException("Không tìm thấy tài khoản của nhân viên.");
            if (user.Status == 0) throw new ArgumentException("Tài khoản nhân viên đã ngưng kích hoạt, vui lòng liên hệ Admin để được hỗ trợ.");

            byte[] passwordHashByte = PasswordHash.GetByteFromString(user.Password);
            byte[] passwordSaltByte = PasswordHash.GetByteFromString(user.PasswordSalt);

            var check = PasswordHash.VerifyPasswordHash(loginEmployee.Password.Trim(), passwordHashByte, passwordSaltByte);

            if (!check) throw new ArgumentException("Mật khẩu đăng nhập không đúng.");

            string token = JwtUserToken.CreateInternalUserToken(user);

            InternalUserTokenModel employeeTokenModel = new()
            {
                Id = user.Id,
                Name = user.Fullname,
                Email = user.Email,
                ImageURL = user.ImageUrl,
                Status = user.Status,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                Token = token
            };

            return employeeTokenModel;
        }

        public async Task<List<Repository.DatabaseModels.InternalUser>> GetEmployeeById(string id)
        {
            List<Repository.DatabaseModels.InternalUser> list = new List<Repository.DatabaseModels.InternalUser>();
            return list;
        }

        public async Task<RegisterInternalUserStatus> Register(RegisterInternalUser internalUser)
        {
            var check = false;
            var checkError = new RegisterInternalUserStatus();
            var isMatches = internalUser.Password.Trim().Equals(internalUser.ConfirmPassword.Trim());
            if (!isMatches)
            {
                checkError.isError = true;
                checkError.ConfirmPasswordFailed = "Mật khẩu xác nhận không trùng khớp.";
            }

            if (await _employeeAuthRepo.CheckDuplicateUsername(internalUser.Username)) {
                checkError.isError = true;
                checkError.DuplicateUsername = "Tài khoản đã tồn tại, vui lòng nhập tài khoản khác.";
            }
            if (await _employeeAuthRepo.CheckDuplicateEmail(internalUser.Email, false))
            {
                checkError.isError = true;
                checkError.DuplicateEmail = "Email đã tồn tại.";
            }
            if (await _employeeAuthRepo.CheckDuplicatePhoneNo(internalUser.PhoneNo, false))
            {
                checkError.isError = true;
                checkError.DuplicatePhoneNo = "Số điện thoại đã tồn tại.";
            }
            if (internalUser.RoleId == "1" || internalUser.RoleId == "2")
            {
                if(internalUser.SiteId == null)
                {
                    checkError.isError = true;
                    checkError.missingSiteID = "Khi đăng ký cho tài khoản Manager hoặc Employee, bắt buộc phải có Mã chi nhánh làm việc hợp lệ.";
                }
            }

            if (checkError.isError) return checkError;

            //create empCode.
            var lastestEmpCode = await _employeeAuthRepo.GetLatestEmployeeCode(); //PM0100002
            int lastestCode = 0;
            if (lastestEmpCode != String.Empty) lastestCode = Convert.ToInt32(lastestEmpCode.Substring(4, 5)); //00002 -> 2

            int newCode = lastestCode + 1; //-> 3
            var currentLength = newCode.ToString().Length; // length = 1
            string newEmployeeCode = String.Empty;
            if (internalUser.RoleId == "1") newEmployeeCode += "MN01";
            if (internalUser.RoleId == "2") newEmployeeCode += "PM01";
            if (internalUser.RoleId == "3") newEmployeeCode += "OW01";
            if (internalUser.RoleId == "4") newEmployeeCode += "AD01";
            if (currentLength < 5)
            {
                for(int i = currentLength; i < 5; i++)
                {
                    newEmployeeCode += "0";
                }
            }
            newEmployeeCode += newCode.ToString();

            //create encrypted Password for Employee.
            PasswordHash.CreatePasswordHash(internalUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var empID = Guid.NewGuid().ToString();
            var addressID = Guid.NewGuid().ToString();
            var insertAddress = false;

            //check if need to insert Employee's Address?
            if(internalUser.CityID != null && internalUser.DistrictID != null && internalUser.WardID != null && internalUser.HomeNumber != null) {
                Repository.DatabaseModels.DynamicAddress dynamicAddress = new()
                {
                    Id = addressID,
                    CityId = internalUser.CityID,
                    DistrictId = internalUser.DistrictID,
                    WardId = internalUser.WardID,
                    HomeAddress = internalUser.HomeNumber
                };
                await _dynamicAddressRepo.InsertNewAddress(dynamicAddress);
                insertAddress = true;
            } //done insert employee's address.

            Repository.DatabaseModels.InternalUser internalUserModel = new()
            {
                Id = empID,
                Code = newEmployeeCode,
                Username = internalUser.Username,
                Fullname = internalUser.Fullname,
                PhoneNo = internalUser.PhoneNo,
                Email = internalUser.Email,
                AddressId = insertAddress ? addressID : null,
                ImageUrl = internalUser.ImageUrl,
                Dob = internalUser.DOB,
                Password = Convert.ToBase64String(passwordHash).Trim(),
                PasswordSalt = Convert.ToBase64String(passwordSalt).Trim(),
                RoleId = internalUser.RoleId,
                Status = internalUser.Status,
                Gender = internalUser.Gender  
            };

            check = await _employeeAuthRepo.RegisterInternalUser(internalUserModel);

            //check if is internal user role Employee or role Manager, run this function to insert working place.
            if (internalUser.RoleId == "1" || internalUser.RoleId == "2")
            {
                if (check)
                {
                    var workingSiteID = Guid.NewGuid().ToString();
                    InternalUserWorkingSite internalUserWorkingSiteModel = new()
                    {
                        Id = workingSiteID,
                        SiteId = internalUser.SiteId,
                        UserId = empID,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    check = await _userWorkingSiteRepo.InsertWorkingSite(internalUserWorkingSiteModel);

                }
            } //insert working site done.

            if (check) {
                checkError.isError = false;
                await EmailService.SendWelcomeEmail(internalUser, $"Chào mừng {internalUser.Fullname} về đội của chúng tôi.", true);
                //send account information via email for new internal user.
            }
            else
            {
                checkError.isError = true;
                checkError.OtherError = "Hệ thống đang bị lỗi, vui lòng thử lại sau.";
            }
            return checkError;
        }
    }
}
