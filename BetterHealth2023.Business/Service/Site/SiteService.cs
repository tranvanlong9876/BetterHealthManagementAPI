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
       

        public SiteService(ISiteRepo siteRepo, IDynamicAddressRepo dynamicAddressRepo,
            IInternalUserAuthRepo employeeAuthRepo)
        {
            _siteRepo = siteRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
            _employeeAuthRepo = employeeAuthRepo;
          
        }

        public async Task<SiteInformation> InsertSite(SiteViewModels siteviewmodel)
        {
            //check exist addressid
            siteviewmodel.DynamicAddModel.Id =  Guid.NewGuid().ToString();
          

            SiteInformation site = new SiteInformation()
            {
                Id = Guid.NewGuid().ToString(),
                SiteName = siteviewmodel.SiteName,
                Description = siteviewmodel.Description,
                ContactInfo = siteviewmodel.ContactInfo,
                ImageUrl = siteviewmodel.ImageUrl,
                AddressId = siteviewmodel.DynamicAddModel.Id,
                LastUpdate = DateTime.Now,
                IsActivate = false,
                IsDelivery = false

            };
            DynamicAddress dynamicAddress2 = new DynamicAddress()
            {
                Id = siteviewmodel.DynamicAddModel.Id,
                CityId = siteviewmodel.DynamicAddModel.CityId,
                DistrictId = siteviewmodel.DynamicAddModel.DistrictId,
                WardId = siteviewmodel.DynamicAddModel.WardId,
                HomeAddress = siteviewmodel.DynamicAddModel.HomeAddress,
            };
            await _dynamicAddressRepo.Insert(dynamicAddress2);
            await _siteRepo.Insert(site);
            return await Task.FromResult(site);
        }




        public async Task<bool> UpdateSite(UpdateSiteModel updateSiteModel)
        {
           
            SiteInformation site = await _siteRepo.Get(updateSiteModel.SiteID);
            if (site == null)
            {
                return await Task.FromResult(false);
            }
            site.SiteName = updateSiteModel.SiteName;
            site.Description = updateSiteModel.Description;
            site.ContactInfo = updateSiteModel.ContactInfo;
            site.ImageUrl = updateSiteModel.ImageUrl;
            DynamicAddress dynamicAddress = await _dynamicAddressRepo.Get(site.AddressId);
            dynamicAddress.CityId = updateSiteModel.DynamicAddModel.CityId;
            dynamicAddress.DistrictId = updateSiteModel.DynamicAddModel.DistrictId;
            dynamicAddress.WardId = updateSiteModel.DynamicAddModel.WardId;
            dynamicAddress.HomeAddress = updateSiteModel.DynamicAddModel.HomeAddress;
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

        public async Task<List<SiteInformation>> GetListSite()
        {
            List<SiteInformation> list = await _siteRepo.GetAll();
            //arrange is Active
            list.Sort((x, y) => x.IsActivate.CompareTo(y.IsActivate));
            return list;
        }




    }
}
