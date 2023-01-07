using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.InternalUserAuthRepos
{
    public interface IInternalUserAuthRepo : IRepository<InternalUser>
    {
        public Task<bool> RegisterInternalUser(InternalUser internalUser);
        public Task<InternalUser> CheckLogin(LoginInternalUser loginEmployee);

        public Task<bool> CheckDuplicateUsername(string username);

        public Task<bool> CheckDuplicatePhoneNo(string username, string phoneNo, bool isUpdate);

        public Task<bool> CheckDuplicateEmail(string username, string email, bool isUpdate);

        public Task<List<InternalUser>> GetEmployeeBySiteID(string SiteID);
        
        public Task<string> GetLatestEmployeeCode();

        public Task<string> GetInternalUserID(string username);
        public Task<InternalUser> GetOldPassword(UpdateInternalUser updateInternalUser);
        public Task<bool> ChangePassword(string guid, string passwordHash, string passwordSalt);
    }
}
