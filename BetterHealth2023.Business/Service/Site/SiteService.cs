using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.InternalUserAuthRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UserWorkingSiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.SiteErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using Microsoft.AspNetCore.Mvc;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site
{
    public class SiteService : ISiteService
    {
        private readonly ISiteRepo _siteRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;
        private readonly IInternalUserAuthRepo _employeeAuthRepo;
        private readonly IUserWorkingSiteRepo _userWorkingSiteRepo;

        public SiteService(ISiteRepo siteRepo, IDynamicAddressRepo dynamicAddressRepo,
            IInternalUserAuthRepo employeeAuthRepo, IUserWorkingSiteRepo userWorkingSiteRepo)
        {
            _siteRepo = siteRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
            _employeeAuthRepo = employeeAuthRepo;
            _userWorkingSiteRepo = userWorkingSiteRepo;
        }

        public async Task<SiteInformation> InsertSite(SiteEntranceModels siteviewmodel)
        {
            //check exist addressid
            var addressID = Guid.NewGuid().ToString();
            var siteID = Guid.NewGuid().ToString();

            SiteInformation site = new SiteInformation()
            {
                Id = siteID,
                SiteName = siteviewmodel.SiteName,
                Description = siteviewmodel.Description,
                ContactInfo = siteviewmodel.ContactInfo,
                ImageUrl = siteviewmodel.ImageUrl,
                AddressId = addressID,
                LastUpdate = CustomDateTime.Now,
                IsActivate = false,
                IsDelivery = false

            };
            DynamicAddress dynamicAddress = new DynamicAddress()
            {
                Id = addressID,
                CityId = siteviewmodel.cityId,
                DistrictId = siteviewmodel.districtId,
                WardId = siteviewmodel.wardId,
                HomeAddress = siteviewmodel.homeAddress,
            };
            await _dynamicAddressRepo.Insert(dynamicAddress);
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
            dynamicAddress.CityId = updateSiteModel.cityId;
            dynamicAddress.DistrictId = updateSiteModel.districtId;
            dynamicAddress.WardId = updateSiteModel.wardId;
            dynamicAddress.HomeAddress = updateSiteModel.homeAddress;
            site.LastUpdate = CustomDateTime.Now;
            await _dynamicAddressRepo.Update();
            await _siteRepo.Update();
            return await Task.FromResult(true);
        }

        public async Task<UpdateSiteStatus> UpdateSiteIsDelivery(string SiteId, bool IsDelivery)
        {
            UpdateSiteStatus updateSiteStatus = new UpdateSiteStatus();
            SiteInformation site = await _siteRepo.Get(SiteId);
            if (site == null)
            {
                updateSiteStatus.isError = true;
                updateSiteStatus.SiteNotFound = "Không tìm thấy dữ liệu chi nhánh";
            }

            if (updateSiteStatus.isError) return updateSiteStatus;

            if (IsDelivery)
            {
                var pharmacistWorking = await _userWorkingSiteRepo.GetTotalPharmacist(SiteId);
                var managerWorking = await _userWorkingSiteRepo.GetTotalManager(SiteId);

                if (pharmacistWorking.Count < 2)
                {
                    updateSiteStatus.isError = true;
                    updateSiteStatus.SiteNotEnoughPharmacist = "Hiện tại không thể bật giao hàng chi nhánh, cần tối thiểu 2 Dược Sĩ.";
                }
                if (managerWorking.Count < 1)
                {
                    updateSiteStatus.isError = true;
                    updateSiteStatus.SiteNotEnoughManager = "Hiện tại không thể bật giao hàng chi nhánh, cần tối thiểu 1 Manager.";
                }
            }

            if (updateSiteStatus.isError) return updateSiteStatus;

            site.IsDelivery = IsDelivery;
            site.LastUpdate = CustomDateTime.Now;
            await _siteRepo.Update();
            updateSiteStatus.isError = false;
            return await Task.FromResult(updateSiteStatus);
        }

        public async Task<UpdateSiteStatus> UpdateSiteIsActive(string SiteId, bool IsActive)
        {
            UpdateSiteStatus updateSiteStatus = new UpdateSiteStatus();
            SiteInformation site = await _siteRepo.Get(SiteId);
            if (site == null)
            {
                updateSiteStatus.isError = true;
                updateSiteStatus.SiteNotFound = "Không tìm thấy dữ liệu chi nhánh";
            }

            if (updateSiteStatus.isError) return updateSiteStatus;

            if (IsActive)
            {
                var pharmacistWorking = await _userWorkingSiteRepo.GetTotalPharmacist(SiteId);
                var managerWorking = await _userWorkingSiteRepo.GetTotalManager(SiteId);

                if (pharmacistWorking.Count < 1)
                {
                    updateSiteStatus.isError = true;
                    updateSiteStatus.SiteNotEnoughPharmacist = "Hiện tại không thể bật hoạt động chi nhánh, cần tối thiểu 1 Dược Sĩ.";
                }
                if (managerWorking.Count < 1)
                {
                    updateSiteStatus.isError = true;
                    updateSiteStatus.SiteNotEnoughManager = "Hiện tại không thể bật hoạt động chi nhánh, cần tối thiểu 1 Manager.";
                }
            }

            if (updateSiteStatus.isError) return updateSiteStatus;

            site.IsActivate = IsActive;
            site.LastUpdate = CustomDateTime.Now;

            if (site.IsDelivery && !IsActive) site.IsDelivery = false;

            await _siteRepo.Update();
            updateSiteStatus.isError = false;
            return await Task.FromResult(updateSiteStatus);
        }


        public async Task<SiteViewModel> GetSite(string siteId)
        {
            //get site by id
            return await _siteRepo.GetSiteById(siteId);
        }

        public async Task<PagedResult<SiteViewModel>> GetListSitePaging(GetSitePagingRequest pagingRequest)
        {
            var siteList = await _siteRepo.GetAllSitePaging(pagingRequest);

            return siteList;
        }
    }
}
