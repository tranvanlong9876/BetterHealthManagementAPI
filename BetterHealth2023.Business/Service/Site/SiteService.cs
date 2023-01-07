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
            var address = _dynamicAddressRepo.Get(siteviewmodel.AddressID);
            if (address == null)
            {
                return await Task.FromResult<SiteInformation>(null);
            }

            SiteInformation site = new SiteInformation()
            {
                Id = new Guid().ToString(),
                SiteName = siteviewmodel.SiteName,
                Description = siteviewmodel.Description,
                ContactInfo = siteviewmodel.ContactInfo,
                AddressId = siteviewmodel.AddressID,
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
            var address = await _dynamicAddressRepo.Get(stSiteViewModels.AddressID);
            if (address == null)
            {
                return await Task.FromResult(false);
                ;
            }

            if (site == null)
            {
                return await Task.FromResult(false);
            }

            site.SiteName = stSiteViewModels.SiteName;
            site.Description = stSiteViewModels.Description;
            site.ContactInfo = stSiteViewModels.ContactInfo;
            site.AddressId = stSiteViewModels.AddressID;
            site.LastUpdate = DateTime.Now;
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


                if (IsDelivery == true)
                {
                    List<Repository.DatabaseModels.InternalUser> employees =
                        await _employeeAuthRepo.GetEmployeeBySiteID(SiteId);
                    if (employees.Count < 2)
                    {
                        return await Task.FromResult(false);
                    }
                }

                if (IsDelivery == false)
                {
                List<OrderHeader> orderHeaders = await _orderHeaderRepo.GetOrderHeadersBySiteId(SiteId);
                if (orderHeaders.Count < 1)
                {
                    throw new Exception("Site must have at least one order");
                }

                foreach (var x in orderHeaders)
                {
                    if (x.OrderStatus != "1" || x.OrderStatus != "4" || x.OrderStatus != "8")
                    {
                        throw new Exception("Site still have order in progress");
                    }

                }
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


                if (IsActive == true)
                {
                    List<Repository.DatabaseModels.InternalUser> employees =
                        await _employeeAuthRepo.GetEmployeeBySiteID(SiteId);
                    if (employees.Count < 1)
                    {
                        throw new Exception("Site must have at least one employee");
                    }
                }

                if (IsActive == false)
                {
                    List<OrderHeader> orderHeaders = await _orderHeaderRepo.GetOrderHeadersBySiteId(SiteId);
                    if (orderHeaders.Count < 1)
                    {
                        throw new Exception("Site must have at least one order");
                    }

                    foreach (var x in orderHeaders)
                    {
                        if (x.OrderStatus != "1" || x.OrderStatus != "4" || x.OrderStatus != "8")
                        {
                            throw new Exception("Site still have order in progress");
                        }

                    }

                UpdateSiteIsDelivery(SiteId, false);


            }
                site.IsActivate = IsActive;
                site.LastUpdate = DateTime.Now;
                await _siteRepo.Update();
                return await Task.FromResult(true);
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
