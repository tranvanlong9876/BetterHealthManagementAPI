using System.Collections.Generic;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.SiteErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site
{
    public interface ISiteService
    {
        
        public Task<SiteInformation> InsertSite(SiteEntranceModels SiteViewModels);
        public Task<bool> UpdateSite(UpdateSiteModel updateSiteModel);
        public Task<UpdateSiteStatus> UpdateSiteIsActive(string SiteId, bool IsActive);
        public Task<UpdateSiteStatus> UpdateSiteIsDelivery(string SiteId, bool IsDelivery);
        public Task<SiteViewModel> GetSite(string siteId);
        public Task<PagedResult<SiteViewModel>> GetListSitePaging(GetSitePagingRequest pagingRequest);
    }
}
