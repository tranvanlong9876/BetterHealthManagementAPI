﻿using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using static System.Linq.Queryable;

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
                        from pi in context.ProductImportBatches.Where(x => x.Id == sib.ImportBatchId).DefaultIfEmpty()
                        group sib by new { sib.SiteId, sib.ProductId, sib.ImportBatchId, pi.ExpireDate } into grp
                        select new
                        {
                            ExpireDate = grp.Key.ExpireDate,
                            Site_ID = grp.Key.SiteId,
                            Product_ID = grp.Key.ProductId,
                            Batch_ID = grp.Key.ImportBatchId,
                            TotalQuantity = grp.Sum(x => x.Quantity)
                        };

            query = query.Where(x => x.Site_ID.Equals(SiteId));

            query = query.Where(x => string.IsNullOrEmpty(x.Batch_ID) || x.ExpireDate >= CustomDateTime.Now);

            for (int i = 0; i < orderProducts.Count; i++)
            {
                var product = orderProducts[i];
                var queryInside = query.Where(x => x.Product_ID.Equals(product.productId));

                if (await queryInside.Where(x => x.TotalQuantity >= product.productQuantity).CountAsync() == 0)
                {
                    //thiếu sản phẩm
                    var missingProductModel = await queryInside.Select(selector => new ViewSpecificMissingProduct()
                    {
                        missingQuantity = product.productQuantity - selector.TotalQuantity,
                        ProductId = product.productId,
                        StatusMessage = $"Sản phẩm đang bị thiếu tồn kho {product.productQuantity - selector.TotalQuantity} {product.UnitName}"
                    }).FirstOrDefaultAsync();

                    if (missingProductModel == null)
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

            var productBatches = await query.Where(x => x.batch.ProductId.Equals(productId) && x.productImportBatch.ExpireDate >= CustomDateTime.Now && x.batch.Quantity > 0 && x.batch.SiteId.Equals(siteId)).OrderBy(x => x.productImportBatch.ExpireDate).Select(x => x.batch).ToListAsync();

            return productBatches;
        }

        public async Task<List<OrderBatch>> GetAllSiteInventoryBatchFromOrderProductBatch(string productId, string siteId, string orderId)
        {
            var query = from orderBatch in context.OrderBatches
                        from siteInvenBatch in context.SiteInventoryBatches.Where(x => x.Id == orderBatch.SiteInventoryBatchId).DefaultIfEmpty()
                        select new { orderBatch, siteInvenBatch };
            query = query.Where(x => x.orderBatch.OrderId.Equals(orderId) && x.siteInvenBatch.SiteId.Equals(siteId) && x.siteInvenBatch.ProductId.Equals(productId));

            return await query.Select(x => x.orderBatch).ToListAsync();
        }

        public async Task<int> GetInventoryOfProductOfSite(string productId, string siteId)
        {
            var query = from sib in context.SiteInventoryBatches
                        from batch in context.ProductImportBatches.Where(x => x.Id == sib.ImportBatchId).DefaultIfEmpty()
                        select new { sib, batch };

            query = query.Where(x => x.sib.SiteId.Equals(siteId) && x.sib.ProductId.Equals(productId));
            query = query.Where(x => string.IsNullOrEmpty(x.sib.ImportBatchId) || x.batch.ExpireDate >= CustomDateTime.Now);

            var totalQuantity = await query.SumAsync(x => (int?) x.sib.Quantity) ?? 0;

            return totalQuantity;
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
                               from batch in context.ProductImportBatches.Where(x => x.Id == sib.ImportBatchId).DefaultIfEmpty()
                               where string.IsNullOrEmpty(sib.ImportBatchId) || batch.ExpireDate >= CustomDateTime.Now
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
                        WardId = selector.address.WardId,
                        AddressId = selector.address.Id
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
