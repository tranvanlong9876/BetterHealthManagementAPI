using System.Collections.Generic;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.SiteErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site
{
    public interface ISiteService
    {
        
        public Task<SiteInformation> InsertSite(SiteViewModels SiteViewModels);
        public Task<bool> UpdateSite(UpdateSiteModel updateSiteModel);
        public Task<UpdateSiteStatus> UpdateSiteIsActive(string SiteId, bool IsActive);
        public Task<UpdateSiteStatus> UpdateSiteIsDelivery(string SiteId, bool IsDelivery);
        public Task<SiteInformation> GetSite(string siteId);
        public Task<List<SiteInformation>> GetListSite();
    }
}
