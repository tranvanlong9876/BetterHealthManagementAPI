using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos
{
    public interface ISiteRepo: IRepository<SiteInformation>
    {
        public Task<string> GetSiteAddressId(string siteId);
        public Task<bool> UpdateSite(SiteInformation siteInformation);
        public Task DeleteSite(SiteInformation siteInformation);
        public Task<SiteViewModel> GetSiteById(string id);
        public Task<PagedResult<SiteViewModel>> GetAllSitePaging(GetSitePagingRequest pagingRequest);
    }
}
