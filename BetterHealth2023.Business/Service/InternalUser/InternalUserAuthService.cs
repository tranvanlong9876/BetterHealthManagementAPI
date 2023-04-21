using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.InternalUserAuthRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UserWorkingSiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using System;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalUser
{
    public class InternalUserAuthService : IInternalUserAuthService
    {
        private readonly IInternalUserAuthRepo _employeeAuthRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;
        private readonly IUserWorkingSiteRepo _userWorkingSiteRepo;
        private readonly ISiteRepo _siteRepo;
        private readonly IOrderHeaderRepo _orderHeaderRepo;

        public InternalUserAuthService(IInternalUserAuthRepo employeeAuthRepo, IDynamicAddressRepo dynamicAddressRepo, IUserWorkingSiteRepo userWorkingSiteRepo, 
            ISiteRepo siteRepo, IOrderHeaderRepo orderHeaderRepo)
        {
            _employeeAuthRepo = employeeAuthRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
            _userWorkingSiteRepo = userWorkingSiteRepo;
            _siteRepo = siteRepo;
            _orderHeaderRepo = orderHeaderRepo;
        }

        public async Task<LoginUserStatus> Login(LoginInternalUser loginEmployee)
        {
            Repository.DatabaseModels.InternalUser user = await _employeeAuthRepo.CheckLogin(loginEmployee);
            var checkError = new LoginUserStatus();

            if(user == null)
            {
                checkError.isError = true;
                checkError.UserNotFound = "Không tìm thấy tài khoản nhân viên nội bộ, vui lòng thử lại.";
                return checkError;
            }
            if (user.Status == 0)
            {
                checkError.isError = true;
                checkError.UserNotFound = "Tài khoản nhân viên đã ngưng kích hoạt, vui lòng liên hệ Admin để được hỗ trợ.";
                return checkError;
            }

            byte[] passwordHashByte = PasswordHash.GetByteFromString(user.Password);
            byte[] passwordSaltByte = PasswordHash.GetByteFromString(user.PasswordSalt);

            var check = PasswordHash.VerifyPasswordHash(loginEmployee.Password.Trim(), passwordHashByte, passwordSaltByte);

            if (!check)
            {
                checkError.isError = true;
                checkError.WrongPassword = "Mật khẩu đăng nhập không đúng.";
                return checkError;
            }
            SiteViewModel workingSite = null;
            if (user.Role.RoleName.Equals(Commons.PHARMACIST_NAME) || user.Role.RoleName.Equals(Commons.MANAGER_NAME))
            {
                workingSite = await _userWorkingSiteRepo.GetInternalUserWorkingSiteModel(user.Id);
                if(workingSite == null)
                {
                    checkError.isError = true;
                    checkError.NoWorkingSite = "Dược Sĩ và Quản Lý không có chi nhánh làm việc.";
                    return checkError;
                }
            }
            string token = JwtUserToken.CreateInternalUserToken(user, workingSite);
            

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

            checkError.isError = false;
            checkError.userToken = employeeTokenModel;

            return checkError;
        }

        public async Task<RegisterInternalUserStatus> Register(RegisterInternalUser internalUser)
        {
            var check = false;
            var checkError = new RegisterInternalUserStatus();
            var randomPassword = PasswordHash.CreateNormalPassword(12);

            if (await _employeeAuthRepo.CheckDuplicateUsername(internalUser.Username)) {
                checkError.isError = true;
                checkError.DuplicateUsername = "Tài khoản đã tồn tại, vui lòng nhập tài khoản khác.";
            }
            if (await _employeeAuthRepo.CheckDuplicateEmail(internalUser.Username, internalUser.Email, false))
            {
                checkError.isError = true;
                checkError.DuplicateEmail = "Email đã tồn tại.";
            }
            if (await _employeeAuthRepo.CheckDuplicatePhoneNo(internalUser.Username, internalUser.PhoneNo, false))
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
            PasswordHash.CreatePasswordHash(randomPassword, out byte[] passwordHash, out byte[] passwordSalt);
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
                Status = 1,
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
                        CreatedDate = CustomDateTime.Now,
                        UpdatedDate = CustomDateTime.Now,
                        IsWorking = true
                    };

                    check = await _userWorkingSiteRepo.InsertWorkingSite(internalUserWorkingSiteModel);

                }
            } //insert working site done.

            if (check) {
                checkError.isError = false;
                if (!string.IsNullOrEmpty(internalUser.SiteId))
                {
                    var siteDB = await _siteRepo.Get(internalUser.SiteId);

                    internalUser.SiteName = siteDB.SiteName;
                    internalUser.SiteAddress = await _dynamicAddressRepo.GetFullAddressFromAddressId(siteDB.AddressId);
                }
                _ = Task.Run(() => EmailService.SendWelcomeEmailAsync(internalUser, randomPassword.Trim(), $"Chào mừng {internalUser.Fullname} về đội của chúng tôi.", true)).ConfigureAwait(false);
                //send account information via email for new internal user.
            }
            else
            {
                checkError.isError = true;
                checkError.OtherError = "Hệ thống đang bị lỗi, vui lòng thử lại sau.";
            }
            return checkError;
        }

        public async Task<UpdateInternalUserStatus> UpdateInternalUser(UpdateInternalUser user)
        {
            var check = false;
            var checkError = new UpdateInternalUserStatus();

            //lấy ra mã khóa chính từ username của nhân viên.
            var EmpGuid = await _employeeAuthRepo.GetInternalUserID(user.Username.Trim());

            if (string.IsNullOrEmpty(EmpGuid))
            {
                checkError.isError = true;
                checkError.Notfound = "Không tìm thấy nhân viên";
                return checkError;
            }

            //kiểm tra trùng lặp email và số điện thoại so với các nhân viên khác.
            if (await _employeeAuthRepo.CheckDuplicateEmail(user.Username, user.Email, true))
            {
                checkError.isError = true;
                checkError.DuplicateEmail = "Email đã tồn tại.";
            }
            if (await _employeeAuthRepo.CheckDuplicatePhoneNo(user.Username, user.PhoneNo, true))
            {
                checkError.isError = true;
                checkError.DuplicatePhoneNo = "Số điện thoại đã tồn tại.";
            }
            //hủy chức năng và báo cáo lại việc email hoặc sđt bị trùng với các nhân viên khác.
            if (checkError.isError) return checkError;

            //nếu nhân viên nội bộ cần thay đổi mật khẩu. isChangePassword = true
            if (user.isChangePassword) 
            {
                //kiểm tra thông tin cần để thay đổi mật khẩu nhân viên đã nhập đủ chưa?
                if(string.IsNullOrEmpty(user.OldPassword) || string.IsNullOrEmpty(user.NewPassword) || string.IsNullOrEmpty(user.ConfirmPassword))
                {
                    checkError.isError = true;
                    checkError.RequireChangePasswordFailed = "Thông tin cần thay đổi mật khẩu nhập không đầy đủ, vui lòng kiểm tra lại.";
                    return checkError;
                }
                //bắt đầu kiểm tra password cũ.

                //get old password.
                Repository.DatabaseModels.InternalUser internalUser = await _employeeAuthRepo.GetOldPassword(user);

                //giải mã
                byte[] passwordHashByte = PasswordHash.GetByteFromString(internalUser.Password);
                byte[] passwordSaltByte = PasswordHash.GetByteFromString(internalUser.PasswordSalt);

                var checkOldPassword = PasswordHash.VerifyPasswordHash(user.OldPassword.Trim(), passwordHashByte, passwordSaltByte);
                if(checkOldPassword) //nếu mật khẩu cũ nhập chính xác
                {
                    //kiểm tra mật khẩu mới nhập vs nhau đã trùng khớp chưa.
                    var isMatches = user.NewPassword.Trim().Equals(user.ConfirmPassword.Trim());
                    if (!isMatches)
                    {
                        checkError.isError = true;
                        checkError.ConfirmPasswordFailed = "Mật khẩu xác nhận không trùng khớp.";
                        return checkError;
                    }
                    //xác nhận trùng khớp, mã hóa mật khẩu mới cho nhân viên.
                    PasswordHash.CreatePasswordHash(user.NewPassword.Trim(), out byte[] newPasswordHash, out byte[] newPasswordSalt);
                    //thay đổi mật khẩu.
                    check = await _employeeAuthRepo.ChangePassword(EmpGuid, Convert.ToBase64String(newPasswordHash).Trim(), Convert.ToBase64String(newPasswordSalt).Trim());
                    
                    if(!check)
                    {
                        checkError.isError = true;
                        checkError.RequireChangePasswordFailed = "Lỗi thay đổi mật khẩu, vui lòng kiểm tra lại.";
                        return checkError;
                    }
                } else //sai mật khẩu cũ.
                {
                    checkError.isError = true;
                    checkError.WrongOldPassword = "Sai mật khẩu cũ, vui lòng thử lại.";
                    return checkError;
                }
            } //hoàn thành quá trình kiểm tra và thay đổi mật khẩu.

            //Mỗi nhân viên chỉ được tự cập nhật thông tin tài khoản của chính mình.
            //Thông tin được cập nhật ngoại trừ Code, Role, Status.

            //lấy dữ liệu user nội bộ đang có.
            var currentUser = await _employeeAuthRepo.Get(EmpGuid);

            //khi update bắt buộc bổ sung đủ địa chỉ.
            //kiểm tra nhân viên đã có địa chỉ khi tạo chưa ?

            //nếu chưa có
            if(string.IsNullOrEmpty(currentUser.AddressId))
            {
                //tạo mới địa chỉ
                var newAddressId = Guid.NewGuid().ToString();
                DynamicAddress dynamicAddress = new DynamicAddress()
                {
                    CityId = user.CityID,
                    DistrictId = user.DistrictID,
                    WardId = user.WardID,
                    HomeAddress = user.HomeNumber,
                    Id = newAddressId
                };
                await _dynamicAddressRepo.InsertNewAddress(dynamicAddress);
                //thêm xong, cập nhật địa chỉ vào cho user.
                currentUser.AddressId = newAddressId;
                await _employeeAuthRepo.Update();
            } else //nếu địa chỉ đã có sẵn
            {
                //lấy ra mã địa chỉ của user
                var currentAddressID = currentUser.AddressId;
                //lấy thông tin địa chỉ
                var userAddress = await _dynamicAddressRepo.Get(currentAddressID);
                //cập nhật lại thông tin
                userAddress.CityId = user.CityID;
                userAddress.DistrictId = user.DistrictID;
                userAddress.WardId = user.WardID;
                userAddress.HomeAddress = user.HomeNumber;

                await _dynamicAddressRepo.Update();
            }

            //tiến hành cập nhật nốt các thông tin còn lại.

            currentUser.Dob = user.DOB;
            currentUser.Gender = user.Gender;
            currentUser.ImageUrl = user.ImageUrl;
            currentUser.PhoneNo = user.PhoneNo;
            currentUser.Email = user.Email;
            currentUser.Fullname = user.Fullname;

            check = await _employeeAuthRepo.Update();

            if (check)
            {
                checkError.isError = false;
            }
            else
            {
                checkError.isError = true;
                checkError.OtherError = "Hệ thống đang bị lỗi, vui lòng thử lại sau.";
            }
            return checkError;
        }

        public async Task<UpdateUserStatus> UpdateAccountStatus(string guid, int status)
        {
            var deleteUser = await _employeeAuthRepo.Get(guid);

            var checkError = new UpdateUserStatus();

            //check if the site that user is working is still Active ?
            var siteID = await _userWorkingSiteRepo.GetInternalUserWorkingSite(deleteUser.Id);

            //if admin is going to delete user, do this function.
            if (siteID != null && status == 0)
            {
                //get site details.
                var site = await _siteRepo.Get(siteID);
                if(site.IsActivate || site.IsDelivery)
                {
                    var pharmacistWorking = await _userWorkingSiteRepo.GetTotalPharmacist(siteID);
                    var managerWorking = await _userWorkingSiteRepo.GetTotalManager(siteID);
                    //if delivery mode is turn on, site has to have at least 1 Pharmacist and 2 Manager.
                    if (site.IsDelivery)
                    {
                        if (pharmacistWorking.Count <= 2 && deleteUser.RoleId.Equals(Commons.PHARMACIST))
                        {
                            checkError.isError = true;
                            checkError.NotEnoughPharmacist = "Hiện tại không thể ngắt hoạt động Dược sĩ vì chi nhánh cần tối thiểu 2 Dược Sĩ để giao hàng.";
                        }
                        if (managerWorking.Count <= 1 && deleteUser.RoleId.Equals(Commons.MANAGER))
                        {
                            checkError.isError = true;
                            checkError.NotEnoughManager = "Hiện tại không thể ngắt hoạt động Quản Lý vì chi nhánh cần tối thiểu 1 Quản Lý để nhập hàng.";
                        }
                    }

                    if (checkError.isError) return checkError;
                    //if activate mode is turn on, site has to have at least 1 Pharmacist and 1 Manager.

                    if (site.IsActivate)
                    {
                        if (pharmacistWorking.Count <= 1 && deleteUser.RoleId.Equals(Commons.PHARMACIST))
                        {
                            checkError.isError = true;
                            checkError.NotEnoughPharmacist = "Hiện tại không thể ngắt hoạt động Dược sĩ vì chi nhánh cần tối thiểu 1 Dược Sĩ để bán hàng.";
                        }
                        if (managerWorking.Count <= 1 && deleteUser.RoleId.Equals(Commons.MANAGER))
                        {
                            checkError.isError = true;
                            checkError.NotEnoughManager = "Hiện tại không thể ngắt hoạt động Quản Lý vì chi nhánh cần tối thiểu 1 Quản Lý để nhập hàng.";
                        }
                    }
                }
            }
            //done checking Manager vs Pharmacist working site.
            if (checkError.isError) return checkError;

            //check Pharmacist's Executing Order.
            if (deleteUser.RoleId.Equals(Commons.PHARMACIST) && status == 0)
            {
                var executingOrder = await _orderHeaderRepo.GetExecutingOrdersByPharmacistId(deleteUser.Id);
                if(executingOrder.Count >= 1)
                {
                    checkError.isError = true;
                    checkError.PharmacistHaveOrder = "Hiện tại không thể ngắt hoạt động Dược Sĩ vì vẫn còn đơn hàng đang xử lý.";
                }
            }
            //done checking Pharmacist's Executing Order.
            if (checkError.isError) return checkError;
            //update database
            var check = await _employeeAuthRepo.UpdateAccountStatus(deleteUser.Id, status);
            
            if (check)
            {
                checkError.isError = false;
            }
            else
            {
                checkError.isError = true;
                checkError.OtherError = "Hệ thống đang bị lỗi, vui lòng thử lại sau.";
            }
            return checkError;
        }

        public async Task<UserInfoModel> GetUserInfoModel(string guid)
        {
            UserInfoModel infoModel = await _employeeAuthRepo.GetUserInfo(guid);
            infoModel.FullyAddress = string.IsNullOrEmpty(infoModel.AddressID) ? "Chưa cập nhật thông tin địa chỉ" : await _dynamicAddressRepo.GetFullAddressFromAddressId(infoModel.AddressID);
            return infoModel;
        }

        public async Task<PagedResult<UserInfoModel>> GetAllUserPaging(GetInternalUserPagingRequest pagingRequest)
        {
            var userInfoList = await _employeeAuthRepo.GetAllPaging(pagingRequest);
            userInfoList.Items.ForEach(x => x.FullyAddress = "Không trả ở list");
            return userInfoList;
        }
    }
}
