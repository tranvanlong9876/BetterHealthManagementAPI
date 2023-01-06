using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site
{
    public interface ISiteService
    {
        
        public Task<SiteInformation> InsertSite(SiteViewModels SiteViewModels);
        public Task<bool> UpdateSite(string SiteID,SiteViewModels stSiteViewModels);
        public Task<bool> UpdateSiteIsActive(string SiteId, bool IsActive);
        public Task<bool> UpdateSiteIsDelivery(string SiteId, bool IsDelivery);
       
    }
}
