using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> CheckDuplicateEmail(string email, bool isUpdate)
        {
            var query = from x in context.InternalUsers
                        where x.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                        select new { x };
            var employee = await query.Select(selector => new InternalUser()).FirstOrDefaultAsync();
            if (employee != null) return true;
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

        public async Task<bool> CheckDuplicatePhoneNo(string phoneNo, bool isUpdate)
        {
            var query = from x in context.InternalUsers
                        where x.PhoneNo.ToLower().Trim().Equals(phoneNo.ToLower().Trim())
                        select new { x };
            var employee = await query.Select(selector => new InternalUser()).FirstOrDefaultAsync();
            if (employee != null) return true;
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
    }
}
