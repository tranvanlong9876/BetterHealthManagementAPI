using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site
{
    public interface ISiteService
    {
        
        public Task<SiteInformation> InsertSite(SiteInformation siteInformation);
        public Task<bool> UpdateSite(SiteInformation siteInformation);
        public Task DeleteSite(SiteInformation siteInformation);
        public Task<SiteInformation> GetSiteById(string id);
    }
}
