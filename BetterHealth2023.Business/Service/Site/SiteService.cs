using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.InternalUserAuthRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using Microsoft.AspNetCore.Mvc;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site
{
    public class SiteService : ISiteService
    {
        private readonly ISiteRepo _siteRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;
        private readonly IInternalUserAuthRepo _employeeAuthRepo;
        private readonly IOrderHeaderRepo _orderHeaderRepo;

        public SiteService(ISiteRepo siteRepo, IDynamicAddressRepo dynamicAddressRepo,
            IInternalUserAuthRepo employeeAuthRepo, IOrderHeaderRepo orderHeaderRepo)
        {
            _siteRepo = siteRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
            _employeeAuthRepo = employeeAuthRepo;
            _orderHeaderRepo = orderHeaderRepo;
        }

        public async Task<SiteInformation> InsertSite(SiteViewModels siteviewmodel)
        {
            //check exist addressid
            siteviewmodel.DynamicAddress.Id =  Guid.NewGuid().ToString();
          

            SiteInformation site = new SiteInformation()
            {
                Id = new Guid().ToString(),
                SiteName = siteviewmodel.SiteName,
                Description = siteviewmodel.Description,
                ContactInfo = siteviewmodel.ContactInfo,
                AddressId = siteviewmodel.DynamicAddress.Id,
                LastUpdate = DateTime.Now,
                IsActivate = false,
                IsDelivery = false

            };
            _siteRepo.Insert(site);
            return await Task.FromResult(site);
        }




        public async Task<bool> UpdateSite(string SiteID, SiteViewModels stSiteViewModels)
        {
           
            SiteInformation site = await _siteRepo.Get(SiteID);
            if (site == null)
            {
                return await Task.FromResult(false);
            }
            site.SiteName = stSiteViewModels.SiteName;
            site.Description = stSiteViewModels.Description;
            site.ContactInfo = stSiteViewModels.ContactInfo;
            DynamicAddress dynamicAddress = await _dynamicAddressRepo.Get(site.AddressId);
            dynamicAddress.CityId = stSiteViewModels.DynamicAddress.CityId;
            dynamicAddress.DistrictId = stSiteViewModels.DynamicAddress.DistrictId;
            dynamicAddress.WardId = stSiteViewModels.DynamicAddress.WardId;
            dynamicAddress.HomeAddress = stSiteViewModels.DynamicAddress.HomeAddress;
            site.LastUpdate = DateTime.Now;
            await _dynamicAddressRepo.Update();
            await _siteRepo.Update();
            return await Task.FromResult(true);
        }

        

        //chua lam check tinh trang don hang
            public async Task<bool> UpdateSiteIsDelivery(string SiteId, bool IsDelivery)
            {
                SiteInformation site = await _siteRepo.Get(SiteId);
                if (site == null)
                {
                    return await Task.FromResult(false);
                }
                site.IsActivate = IsDelivery;
                site.LastUpdate = DateTime.Now;
                await _siteRepo.Update();
                return await Task.FromResult(true);
            }

            //chua lam check tinh trang don hang
            public async Task<bool> UpdateSiteIsActive(string SiteId, bool IsActive)
            {
                SiteInformation site = await _siteRepo.Get(SiteId);
                if (site == null)
                {
                    throw new Exception("Site not found");
                }

                site.IsActivate = IsActive;
                site.LastUpdate = DateTime.Now;
                await _siteRepo.Update();
                return await Task.FromResult(true);
            }


        public async Task<bool> UpdateSite(SiteInformation siteInformation)
        {
            return await _siteRepo.UpdateSite(siteInformation);
        }



        public async Task<SiteInformation> GetSite(string siteId)
        {
            //get site by id
            return await _siteRepo.GetSiteById(siteId);
        }




    }
}
