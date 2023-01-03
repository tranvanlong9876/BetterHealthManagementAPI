using System;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using Microsoft.AspNetCore.Mvc;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site
{
    public class SiteService : ISiteService
    {
        private readonly ISiteRepo _siteRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;

        public SiteService(ISiteRepo siteRepo, IDynamicAddressRepo dynamicAddressRepo)
        {
            _siteRepo = siteRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
        }
        
            public Task<SiteInformation> InsertSite(SiteViewModels siteviewmodel, [FromBody] string AddressId)
        {
            //check exist addressid
            var address = _dynamicAddressRepo.Get(AddressId);
            if (address == null)
            {
                return Task.FromResult<SiteInformation>(null);
            }
            SiteInformation site = new SiteInformation()
            {
                Id = new Guid().ToString(),
                SiteName = siteviewmodel.SiteName,
                Description = siteviewmodel.Description,
                ContactInfo = siteviewmodel.ContactInfo,
                AddressId = AddressId,
                LastUpdate = DateTime.Now,
                IsActivate = false,
                IsDelivery = false

            };
            _siteRepo.Insert(site);
            return Task.FromResult(site);
        }




        public Task<bool> UpdateSite(SiteInformation siteInformation)
        {
            return _siteRepo.UpdateSite(siteInformation);
        }

        public Task DeleteSite(SiteInformation siteInformation)
        {
            throw new System.NotImplementedException();
        }

        public Task<SiteInformation> GetSiteById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
