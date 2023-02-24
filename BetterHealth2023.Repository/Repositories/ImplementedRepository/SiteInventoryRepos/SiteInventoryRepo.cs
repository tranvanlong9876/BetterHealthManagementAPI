using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos
{
    public class SiteInventoryRepo : Repository<SiteInventory>, ISiteInventoryRepo
    {
        public SiteInventoryRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<SiteInventory> GetSiteInventory(string siteID, string ProductID)
        {
            var query = from siteinven in context.SiteInventories.Where(x => x.ProductId.Equals(ProductID.Trim()) && x.SiteId.Equals(siteID.Trim()))
                        select siteinven;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SiteModelToPickUp> ViewSiteToPickUpsAsync(List<CartModel> cartModels, string cityId, string districtId)
        {
            var query = from site in context.SiteInformations
                        from inventory in context.SiteInventories.Where(x => x.SiteId == site.Id).DefaultIfEmpty()
                        from address in context.DynamicAddresses.Where(x => x.Id == site.AddressId).DefaultIfEmpty()
                        select new { site, inventory, address };

            query = query.Where(x => x.address.CityId.Equals(cityId) && x.site.IsActivate);

            if (!string.IsNullOrWhiteSpace(districtId))
            {
                query = query.Where(x => x.address.DistrictId.Equals(districtId));
            }

            //Big List.

            var bigSiteList = new List<List<string>>();

            for(int i = 0; i < cartModels.Count; i++)
            {
                var productId = cartModels[i].ProductId;
                var quantity = cartModels[i].Quantity;
                var data = await query.Where(x => (x.inventory.ProductId.Equals(productId) && x.inventory.Quantity >= quantity)).Select(selector => selector.site.Id).ToListAsync();

                bigSiteList.Add(data);
            }

            for(int i = 0; i < bigSiteList.Count - 1; i++)
            {
                bigSiteList[0] = bigSiteList[0].Intersect(bigSiteList[i + 1]).ToList();
            }

            var totalRow = bigSiteList[0].Count();
            var SiteListToPickUp = new List<SiteListToPickUp>();
            for (int i = 0; i < bigSiteList[0].Count; i++)
            {
                var siteId = bigSiteList[0][i];
                var data = await query.Where(x => x.site.Id.Equals(siteId)).Select(selector => new SiteListToPickUp()
                {
                    SiteId = selector.site.Id,
                    CityId = selector.address.CityId,
                    DistrictId = selector.address.DistrictId,
                    HomeAddress = selector.address.HomeAddress,
                    SiteName = selector.site.SiteName,
                    WardId = selector.address.WardId
                }).FirstOrDefaultAsync();
                SiteListToPickUp.Add(data);
            }

            SiteModelToPickUp model = new SiteModelToPickUp()
            {
                siteListToPickUps = SiteListToPickUp,
                totalSite = totalRow
            };

            return model;
        }
    }
}
