using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UserWorkingSiteRepos
{
    public interface IUserWorkingSiteRepo : IRepository<InternalUserWorkingSite>
    {
        public Task<bool> InsertWorkingSite(InternalUserWorkingSite internalUserWorkingSite);
        public Task<string> GetInternalUserWorkingSite(string userID);

        public Task<SiteViewModel> GetInternalUserWorkingSiteModel(string userID);

        public Task<List<InternalUserWorkingSite>> GetTotalPharmacist(string siteID);
        public Task<List<InternalUserWorkingSite>> GetTotalManager(string siteID);

        public Task<PagedResult<UserWorkingSiteModel>> GetUserWorkingAtSite(EmployeeWorkingSitePagingRequest pagingRequest, string siteId);

    }
}
