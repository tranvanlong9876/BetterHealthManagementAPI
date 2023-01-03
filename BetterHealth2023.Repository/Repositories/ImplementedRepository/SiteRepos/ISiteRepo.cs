using System;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos
{
    public interface ISiteRepo: IRepository<SiteInformation>
    {
        public Task<bool> UpdateSite(SiteInformation siteInformation);
        public Task DeleteSite(SiteInformation siteInformation);
        public Task<SiteInformation> GetSiteById(string id);
    }
}
