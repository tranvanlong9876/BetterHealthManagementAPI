using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.InternalUserAuthRepos
{
    public class InternalUserAuthRepo : Repository<InternalUser>, IInternalUserAuthRepo
    {
        public InternalUserAuthRepo(BetterHealthManagementContext context) : base (context)
        {

        }

        public async Task<bool> CheckDuplicateUsername(string username)
        {

            var query = from x in context.InternalUsers
                        where x.Username.ToLower().Trim().Equals(username.ToLower().Trim())
                        select new { x };
            var employee = await query.Select(selector => new InternalUser()).FirstOrDefaultAsync();
            if (employee != null) return true;
            return false;
        }

        public async Task<bool> CheckDuplicateEmail(string username, string email, bool isUpdate)
        {
            InternalUser user = null;
            if (!isUpdate)
            {
                var query = from x in context.InternalUsers
                            where x.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                            select new { x };
                user = await query.Select(selector => new InternalUser()).FirstOrDefaultAsync();
            }
            else
            {
                var query = from x in context.InternalUsers
                            where (x.Email.ToLower().Trim().Equals(email.ToLower().Trim()))
                            && (x.Username.ToLower().Trim() != username.ToLower().Trim())
                            select new { x };
                user = await query.Select(selector => new InternalUser()).FirstOrDefaultAsync();
            }

            if (user != null) return true;
            return false;
        }

        public async Task<List<InternalUser>> GetEmployeeBySiteID(string SiteID)
        {
            var query = from x in context.InternalUsers
                        join working in context.InternalUserWorkingSites on x.Id equals working.UserId
                        where working.SiteId.ToLower().Trim().Equals(SiteID.ToLower().Trim())
                        select new { x };
            var employee = await query.Select(selector => new InternalUser()).ToListAsync();
            return employee;
        }

        public async Task<bool> CheckDuplicatePhoneNo(string username, string phoneNo, bool isUpdate)
        {
            InternalUser user = null;
            if(!isUpdate)
            {
                var query = from x in context.InternalUsers
                            where x.PhoneNo.ToLower().Trim().Equals(phoneNo.ToLower().Trim())
                            select new { x };
                user = await query.Select(selector => new InternalUser()).FirstOrDefaultAsync();
            } else
            {
                var query = from x in context.InternalUsers
                            where (x.PhoneNo.ToLower().Trim().Equals(phoneNo.ToLower().Trim())) 
                            && (x.Username.ToLower().Trim() != username.ToLower().Trim())
                            select new { x };
                user = await query.Select(selector => new InternalUser()).FirstOrDefaultAsync();
            }
            
            if (user != null) return true;
            return false;
        }

        public async Task<InternalUser> CheckLogin(LoginInternalUser loginEmployee)
        {
            var query = from x in context.InternalUsers
                        where x.Username.ToLower().Trim().Equals(loginEmployee.Username.ToLower().Trim())
                        select new { x };

            var user = await query.Select(selector => new InternalUser()
            {
                Id = selector.x.Id,
                Username = selector.x.Username,
                Fullname = selector.x.Fullname,
                ImageUrl = selector.x.ImageUrl,
                Password = selector.x.Password,
                PasswordSalt = selector.x.PasswordSalt,
                Role = selector.x.Role,
                RoleId = selector.x.RoleId,
                Status = selector.x.Status,
                Email = selector.x.Email
            }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> RegisterInternalUser(InternalUser user)
        {
            await context.AddAsync(user);
            await Update();
            return true;
        }

        public async Task<string> GetLatestEmployeeCode()
        {
            var empCode = String.Empty;
            var query = from x in context.InternalUsers
                        orderby x.Code.Substring(5,5) descending
                        select new { x };
            var employee = await query.Select(selector => new InternalUser()
            {
                Code = selector.x.Code
            }).FirstOrDefaultAsync();

            empCode = (employee != null) ? employee.Code : String.Empty;

            return empCode;
        }

        public async Task<InternalUser> GetOldPassword(UpdateInternalUser updateInternalUser)
        {
            var query = from x in context.InternalUsers
                        where x.Username.ToLower().Trim().Equals(updateInternalUser.Username.ToLower().Trim())
                        select new { x };

            var user = await query.Select(selector => new InternalUser()
            {
                Password = selector.x.Password,
                PasswordSalt = selector.x.PasswordSalt
            }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> ChangePassword(string guid, string passwordHash, string passwordSalt)
        {
            var user = await Get(guid);
            user.Password = passwordHash.Trim();
            user.PasswordSalt = passwordSalt.Trim();

            await Update();
            return true;
        }

        public async Task<string> GetInternalUserID(string username)
        {
            var user = await context.InternalUsers.Where(x => x.Username.Trim().Equals(username.Trim())).FirstOrDefaultAsync();
            if(user != null)
            {
                return user.Id;
            } else
            {
                throw new Exception("Lỗi lấy thông tin nhân viên. Hàm GetInternalUserID Error.");
            }

        }

        public async Task<bool> UpdateAccountStatus(string guid, int status)
        {
            var internalUser = await Get(guid);
            internalUser.Status = status;

            await Update();
            return true;
        }

        public async Task<UserInfoModel> GetUserInfo(string guid)
        {
            var query = from user in context.InternalUsers
                        join role in context.RoleInternals on user.RoleId equals role.Id
                        from working in context.InternalUserWorkingSites.Where(work => work.UserId == user.Id).DefaultIfEmpty()
                        from address in context.DynamicAddresses.Where(add => add.Id == user.AddressId).DefaultIfEmpty()
                        from site in context.SiteInformations.Where(s => s.Id == working.SiteId).DefaultIfEmpty()
                        where user.Id.Trim().Equals(guid.Trim())
                        select new { user, role, working, address, site };
            var userModel = await query.Select(selector => new UserInfoModel()
            {
                Id = selector.user.Id,
                Username = selector.user.Username,
                Fullname = selector.user.Fullname,
                Code = selector.user.Code,
                RoleId = selector.user.RoleId,
                RoleName = selector.role.RoleName,
                PhoneNo = selector.user.PhoneNo,
                Email = selector.user.Email,
                ImageUrl = selector.user.ImageUrl,
                Status = selector.user.Status,
                DOB = selector.user.Dob,
                Gender = selector.user.Gender ?? default(int),
                CityID = selector.address.CityId,
                DistrictID = selector.address.DistrictId,
                WardID = selector.address.WardId,
                HomeNumber = selector.address.HomeAddress,
                SiteID = selector.working.SiteId,
                SiteName = selector.site.SiteName,
                AddressID = selector.user.AddressId
            }).FirstOrDefaultAsync();

            return userModel;
        }

        public async Task<PagedResult<UserInfoModel>> GetAllPaging(GetInternalUserPagingRequest pagingRequest)
        {
            //1. Select
            var query = from user in context.InternalUsers
                        join role in context.RoleInternals on user.RoleId equals role.Id
                        from working in context.InternalUserWorkingSites.Where(work => work.UserId == user.Id).DefaultIfEmpty()
                        from address in context.DynamicAddresses.Where(add => add.Id == user.AddressId).DefaultIfEmpty()
                        from site in context.SiteInformations.Where(s => s.Id == working.SiteId).DefaultIfEmpty()
                        orderby user.Status descending
                        select new { user, role, working, address, site };

            //2. Filter
            if(!string.IsNullOrEmpty(pagingRequest.FullName))
            {
                query = query.Where(x => x.user.Fullname.Contains(pagingRequest.FullName.Trim()));
            }

            if (!string.IsNullOrEmpty(pagingRequest.RoleID))
            {
                query = query.Where(x => x.user.RoleId.Equals(pagingRequest.RoleID.Trim()));
            }

            if(pagingRequest.UserStatus != null)
            {
                query = query.Where(x => x.user.Status.Equals(pagingRequest.UserStatus));
            }

            //3. Paging
            //-> Mỗi trang 20 record. Trang đầu tiên Skip = 0
            //->
            int totalRow = await query.CountAsync();

            var userModelList = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                .Take(pagingRequest.pageItems)
                .Select(selector => new UserInfoModel()
                {
                    Id = selector.user.Id, //hidden form
                    Username = selector.user.Username, 
                    Fullname = selector.user.Fullname, //hiện trên danh sách
                    Code = selector.user.Code, //hiện trên danh sách
                    RoleId = selector.user.RoleId,
                    RoleName = selector.role.RoleName, //hiện trên danh sách
                    PhoneNo = selector.user.PhoneNo,
                    Email = selector.user.Email,
                    ImageUrl = selector.user.ImageUrl, //hiện trên danh sách
                    Status = selector.user.Status, //hiện trên danh sách, gạt nút tắt mở
                    DOB = selector.user.Dob,
                    Gender = selector.user.Gender ?? default(int),
                    CityID = selector.address.CityId,
                    DistrictID = selector.address.DistrictId,
                    WardID = selector.address.WardId,
                    HomeNumber = selector.address.HomeAddress,
                    SiteID = selector.working.SiteId,
                    SiteName = selector.site.SiteName,
                    AddressID = selector.user.AddressId
                }).ToListAsync();
            //4. Đổ vào dữ liệu đã paging
            var pageResult = new PagedResult<UserInfoModel>(userModelList, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);
            return pageResult;
        }
    }
}
