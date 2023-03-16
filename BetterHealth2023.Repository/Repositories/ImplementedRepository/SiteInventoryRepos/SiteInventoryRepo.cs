using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;
using static System.Linq.Enumerable;
using System;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos
{
    public class SiteInventoryRepo : Repository<SiteInventoryBatch>, ISiteInventoryRepo
    {
        public SiteInventoryRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        //check đối với đơn giao hàng
        public async Task<List<ViewSpecificMissingProduct>> CheckMissingProductOfSiteId(string SiteId, List<OrderProductLastUnitLevel> orderProducts)
        {
            List<ViewSpecificMissingProduct> missingProducts = new List<ViewSpecificMissingProduct>();
            var query = from sib in context.SiteInventoryBatches
                        group sib by new { sib.SiteId, sib.ProductId } into grp
                        select new
                        {
                            Site_ID = grp.Key.SiteId,
                            Product_ID = grp.Key.ProductId,
                            TotalQuantity = grp.Sum(x => x.Quantity)
                        };

            query = query.Where(x => x.Site_ID.Equals(SiteId));

            for(int i = 0; i < orderProducts.Count; i++)
            {
                var product = orderProducts[i];
                var queryInside = query.Where(x => x.Product_ID.Equals(product.productId));

                if(await queryInside.Where(x => x.TotalQuantity >= product.productQuantity).CountAsync() == 0)
                {
                    //thiếu sản phẩm
                    var missingProductModel = await queryInside.Select(selector => new ViewSpecificMissingProduct()
                    {
                        missingQuantity = product.productQuantity - selector.TotalQuantity,
                        ProductId = product.productId,
                        StatusMessage = $"Sản phẩm đang bị thiếu tồn kho {product.productQuantity - selector.TotalQuantity} đơn vị" 
                    }).FirstOrDefaultAsync();

                    if(missingProductModel == null)
                    {
                        missingProductModel = new ViewSpecificMissingProduct()
                        {
                            missingQuantity = product.productQuantity,
                            ProductId = product.productId,
                            StatusMessage = $"Sản phẩm đang bị thiếu tồn kho {product.productQuantity} đơn vị"
                        };
                    }
                    missingProducts.Add(missingProductModel);
                }
            }
            return missingProducts;
        }

        public async Task<List<SiteInventoryBatch>> GetAllProductBatchesAvailable(string productId, string siteId)
        {
            var query = from batch in context.SiteInventoryBatches
                        from productImportBatch in context.ProductImportBatches.Where(x => x.Id == batch.ImportBatchId).DefaultIfEmpty()
                        select new { batch, productImportBatch };

            var productBatches = await query.Where(x => x.batch.ProductId.Equals(productId) && x.productImportBatch.ExpireDate > DateTime.Now && x.batch.Quantity > 0 && x.batch.SiteId.Equals(siteId)).OrderBy(x => x.productImportBatch.ExpireDate).Select(x => x.batch).ToListAsync();

            return productBatches;
        }

        public async Task<SiteInventoryBatch> GetSiteInventory(string siteID, string ProductID)
        {
            var query = from siteinven in context.SiteInventoryBatches.Where(x => x.ProductId.Equals(ProductID.Trim()) && x.SiteId.Equals(siteID.Trim()))
                        select siteinven;

            return await query.FirstOrDefaultAsync();
        }


        public async Task<SiteModelToPickUp> ViewSiteToPickUpsAsync(List<CartModel> cartModels, string cityId, string districtId)
        {
            var query = from site in context.SiteInformations
                        from inventory in context.SiteInventoryBatches.Where(x => x.SiteId == site.Id).DefaultIfEmpty()
                        from address in context.DynamicAddresses.Where(x => x.Id == site.AddressId).DefaultIfEmpty()
                        select new { site, inventory, address };

            query = query.Where(x => x.address.CityId.Equals(cityId) && x.site.IsActivate);

            if (!string.IsNullOrWhiteSpace(districtId))
            {
                query = query.Where(x => x.address.DistrictId.Equals(districtId));
            }

            //Big List.

            var queryToCheck = from sib in context.SiteInventoryBatches
                               group sib by new { sib.SiteId, sib.ProductId } into grp
                               select new
                               {
                                   Site_ID = grp.Key.SiteId,
                                   Product_ID = grp.Key.ProductId,
                                   TotalQuantity = grp.Sum(x => x.Quantity)
                               };
            var bigSiteList = new List<List<string>>();

            for (int i = 0; i < cartModels.Count; i++)
            {
                var productId = cartModels[i].ProductId;
                var quantity = cartModels[i].Quantity;
                var data = await queryToCheck.Where(x => (x.Product_ID.Equals(productId) && x.TotalQuantity >= quantity)).Select(selector => selector.Site_ID).ToListAsync();

                bigSiteList.Add(data);
            }

            for (int i = 0; i < bigSiteList.Count - 1; i++)
            {
                bigSiteList[0] = bigSiteList[0].Intersect(bigSiteList[i + 1]).ToList();
            }

            var totalRow = bigSiteList[0].Count();
            var SiteListToPickUp = new List<SiteListToPickUp>();
            for (int i = 0; i < bigSiteList[0].Count; i++)
            {
                var siteId = bigSiteList[0][i];
                if (await query.Where(x => x.site.Id.Equals(siteId)).CountAsync() > 0)
                {
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
                else
                {
                    totalRow--;
                }
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
