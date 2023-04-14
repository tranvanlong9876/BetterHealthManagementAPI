using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SiteInventoryModels;
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
        //Có động
        public async Task<List<ViewSpecificMissingProduct>> CheckMissingProductOfSiteId(string SiteId, List<OrderProductLastUnitLevel> orderProducts)
        {
            orderProducts = orderProducts.OrderByDescending(x => x.productQuantity).ToList();
            List<ViewSpecificMissingProduct> missingProducts = new List<ViewSpecificMissingProduct>();

            //khởi tạo list tạm cho những sản phẩm trùng cha
            List<ExistProductModel> existProductModels = new List<ExistProductModel>();

            for (int i = 0; i < orderProducts.Count; i++)
            {
                var product = orderProducts[i];
                var quantityCanThem = 0;
                if (product.productQuantityButOnlyOne > 1 && product.isBatches)
                {
                    var currentQuantity = product.productQuantity;

                    var notEditableCurrentQuantity = currentQuantity;

                    var isExistProductModel = existProductModels.Find(x => x.productId == product.productId);

                    if (isExistProductModel != null)
                    {
                        quantityCanThem += isExistProductModel.Quantity;
                    }

                    var query = from sib in context.SiteInventoryBatches
                                from pi in context.ProductImportBatches.Where(x => x.Id == sib.ImportBatchId).DefaultIfEmpty()
                                where sib.SiteId == SiteId && (sib.ImportBatchId == null || pi.ExpireDate >= CustomDateTime.Now) && (sib.ProductId == product.productId) && (sib.Quantity >= product.productQuantityButOnlyOne)
                                select new { sib, pi };

                    var listOfQuantity = await query.Select(selector => selector.sib.Quantity).ToListAsync();
                    var hasQuantity = true;
                    for (int j = 0; j < listOfQuantity.Count && hasQuantity; j++)
                    {
                        var quantityInsideBatch = listOfQuantity[j];
                        int quantityAfterDivide = quantityInsideBatch / product.productQuantityButOnlyOne;
                        if (quantityAfterDivide >= 1)
                        {
                            currentQuantity = currentQuantity - (quantityAfterDivide * product.productQuantityButOnlyOne);
                            if (currentQuantity == 0)
                            {
                                hasQuantity = false;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (currentQuantity > 0)
                    {
                        var existProductMissing = missingProducts.Find(x => x.ProductId == product.productId);
                        if (existProductMissing != null)
                        {
                            existProductMissing.missingQuantity += currentQuantity;
                            existProductMissing.StatusMessage = $"Sản phẩm đang bị thiếu tồn kho.";
                        }
                        else
                        {
                            var missingProductModel = new ViewSpecificMissingProduct()
                            {
                                missingQuantity = currentQuantity,
                                ProductId = product.productId,
                                StatusMessage = $"Sản phẩm đang bị thiếu tồn kho."
                            };
                            missingProducts.Add(missingProductModel);
                        }
                    }

                    var findExist = existProductModels.Find(x => x.productId == product.productId);

                    if (findExist != null)
                    {
                        findExist.Quantity = notEditableCurrentQuantity + quantityCanThem;
                        findExist.checkResultIsEnough = currentQuantity <= 0 ? true : false;
                    }
                    else
                    {
                        existProductModels.Add(new ExistProductModel()
                        {
                            checkResultIsEnough = currentQuantity <= 0 ? true : false,
                            productId = product.productId,
                            Quantity = notEditableCurrentQuantity
                        });
                    }
                }
                else
                {
                    var query = from sib in context.SiteInventoryBatches
                                from pi in context.ProductImportBatches.Where(x => x.Id == sib.ImportBatchId).DefaultIfEmpty()
                                where sib.SiteId == SiteId && (sib.ImportBatchId == null || pi.ExpireDate >= CustomDateTime.Now) && (sib.ProductId == product.productId) && (sib.Quantity >= product.productQuantityButOnlyOne)
                                group sib by new { sib.SiteId, sib.ProductId } into grp
                                select new
                                {
                                    Site_ID = grp.Key.SiteId,
                                    Product_ID = grp.Key.ProductId,
                                    TotalQuantity = grp.Sum(x => x.Quantity)
                                };
                    var existProduct = existProductModels.Find(x => x.productId == product.productId);

                    var currentQuantity = product.productQuantity;
                    var notEditableQuantity = currentQuantity;

                    if (existProduct != null)
                    {
                        currentQuantity += existProduct.Quantity;
                    }

                    bool isEnough = true;
                    if (await query.Where(x => x.TotalQuantity >= currentQuantity).CountAsync() == 0)
                    {
                        //thiếu sản phẩm
                        var missingProductModel = await query.Select(selector => new ViewSpecificMissingProduct()
                        {
                            missingQuantity = currentQuantity - selector.TotalQuantity,
                            ProductId = product.productId,
                            StatusMessage = $"Sản phẩm đang bị thiếu tồn kho."
                        }).FirstOrDefaultAsync();

                        if (missingProductModel == null)
                        {
                            missingProductModel = new ViewSpecificMissingProduct()
                            {
                                missingQuantity = currentQuantity,
                                ProductId = product.productId,
                                StatusMessage = $"Sản phẩm đang bị thiếu tồn kho."
                            };
                        }

                        isEnough = false;
                        var existMissingProduct = missingProducts.Find(x => x.ProductId == product.productId);

                        if (existMissingProduct != null)
                        {
                            existMissingProduct.missingQuantity += missingProductModel.missingQuantity;
                            existMissingProduct.StatusMessage = $"Sản phẩm đang bị thiếu tồn kho.";
                        }
                        else
                        {
                            missingProducts.Add(missingProductModel);
                        }
                    }

                    var findExist = existProductModels.Find(x => x.productId == product.productId);

                    if (findExist != null)
                    {
                        findExist.Quantity = notEditableQuantity + quantityCanThem;
                        findExist.checkResultIsEnough = isEnough;
                    }
                    else
                    {
                        existProductModels.Add(new ExistProductModel()
                        {
                            checkResultIsEnough = isEnough,
                            productId = product.productId,
                            Quantity = notEditableQuantity
                        });
                    }
                }
            }
            return missingProducts;
        }

        //không động đến
        public async Task<List<SiteInventoryBatch>> GetAllProductBatchesAvailable(string productId, string siteId)
        {
            var query = from batch in context.SiteInventoryBatches
                        from productImportBatch in context.ProductImportBatches.Where(x => x.Id == batch.ImportBatchId).DefaultIfEmpty()
                        select new { batch, productImportBatch };

            var productBatches = await query.Where(x => x.batch.ProductId.Equals(productId) && x.productImportBatch.ExpireDate >= CustomDateTime.Now && x.batch.Quantity > 0 && x.batch.SiteId.Equals(siteId)).OrderBy(x => x.productImportBatch.ExpireDate).Select(x => x.batch).ToListAsync();

            return productBatches;
        }

        //không động
        public async Task<List<OrderBatch>> GetAllSiteInventoryBatchFromOrderProductBatch(string productId, string siteId, string orderId)
        {
            var query = from orderBatch in context.OrderBatches
                        from siteInvenBatch in context.SiteInventoryBatches.Where(x => x.Id == orderBatch.SiteInventoryBatchId).DefaultIfEmpty()
                        select new { orderBatch, siteInvenBatch };
            query = query.Where(x => x.orderBatch.OrderId.Equals(orderId) && x.siteInvenBatch.SiteId.Equals(siteId) && x.siteInvenBatch.ProductId.Equals(productId));

            return await query.Select(x => x.orderBatch).ToListAsync();
        }

        //Có
        public async Task<SiteInventoryModel> GetInventoryOfProductOfSite(string productId, string siteId, int quantityConvert)
        {
            var query = from sib in context.SiteInventoryBatches
                        from batch in context.ProductImportBatches.Where(x => x.Id == sib.ImportBatchId).DefaultIfEmpty()
                        select new { sib, batch };

            query = query.Where(x => x.sib.SiteId.Equals(siteId) && x.sib.ProductId.Equals(productId));
            var queryTotalQuantity = query.Where(x => string.IsNullOrEmpty(x.sib.ImportBatchId) || x.batch.ExpireDate >= CustomDateTime.Now);
            var queryTotalQuantityForFirstUnit = query.Where(x => (string.IsNullOrEmpty(x.sib.ImportBatchId) || x.batch.ExpireDate >= CustomDateTime.Now) && (x.sib.Quantity % quantityConvert != 0));

            var totalQuantity = await queryTotalQuantity.SumAsync(x => (int?)x.sib.Quantity) ?? 0;
            var totalQuantityFirstUnit = 0;

            var batchesNotDivine = await queryTotalQuantityForFirstUnit.ToListAsync();
            var leTe = 0;

            for (int i = 0; i < batchesNotDivine.Count; i++)
            {
                leTe += batchesNotDivine[i].sib.Quantity % quantityConvert;
            }

            totalQuantityFirstUnit = totalQuantity - leTe;

            return new SiteInventoryModel()
            {
                TotalQuantity = totalQuantity,
                TotalQuantityForFirst = totalQuantityFirstUnit
            };
        }

        //Không
        public async Task<SiteInventoryBatch> GetSiteInventory(string siteID, string ProductID)
        {
            var query = from siteinven in context.SiteInventoryBatches.Where(x => x.ProductId.Equals(ProductID.Trim()) && x.SiteId.Equals(siteID.Trim()))
                        select siteinven;

            return await query.FirstOrDefaultAsync();
        }

        //Có
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


            var bigSiteList = new List<List<string>>();

            List<ExistProductModel> existProductModels = new List<ExistProductModel>();
            cartModels = cartModels.OrderByDescending(x => x.QuantityConvert).ToList();
            for (int i = 0; i < cartModels.Count; i++)
            {
                //cần sửa
                var cartModel = cartModels[i];
                var quantityCanThem = 0;
                if (cartModel.QuantityConvert > 1 && cartModel.isBatches)
                {
                    var currentQuantity = cartModel.Quantity;
                    var notEditableCurrentQuantity = currentQuantity;
                    var isExistProductModel = existProductModels.Find(x => x.productId == cartModel.ProductId);
                    if (isExistProductModel != null)
                    {
                        quantityCanThem += isExistProductModel.Quantity;
                    }

                    var queryToCheck = from sib in context.SiteInventoryBatches
                                       from pi in context.ProductImportBatches.Where(x => x.Id == sib.ImportBatchId).DefaultIfEmpty()
                                       where (sib.ImportBatchId == null || pi.ExpireDate >= CustomDateTime.Now) && (sib.ProductId == cartModel.ProductId) && (sib.Quantity >= cartModel.QuantityConvert)
                                       select new { sib, pi };

                    var siteList = await context.SiteInformations.Select(x => x.Id).ToListAsync();
                    var siteListDuDieuKien = new List<string>();
                    //vòng for Site
                    for (int site = 0; site < siteList.Count; site++)
                    {
                        currentQuantity = notEditableCurrentQuantity;
                        currentQuantity += quantityCanThem;
                        var siteId = siteList[site];
                        var listOfQuantity = await queryToCheck.Where(x => x.sib.SiteId == siteId).Select(selector => selector.sib.Quantity).ToListAsync();

                        var hasQuantity = true;

                        for (int j = 0; j < listOfQuantity.Count && hasQuantity; j++)
                        {
                            var quantityInsideBatch = listOfQuantity[j];
                            int quantityAfterDivide = quantityInsideBatch / cartModel.QuantityConvert;
                            if (quantityAfterDivide >= 1)
                            {
                                currentQuantity = currentQuantity - (quantityAfterDivide * cartModel.QuantityConvert);
                                if (currentQuantity <= 0)
                                {
                                    hasQuantity = false;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (currentQuantity <= 0)
                        {
                            siteListDuDieuKien.Add(siteId);
                        }

                    }

                    bigSiteList.Add(siteListDuDieuKien);

                    var findExist = existProductModels.Find(x => x.productId == cartModel.ProductId);

                    if (findExist != null)
                    {
                        findExist.Quantity = notEditableCurrentQuantity + quantityCanThem;
                        findExist.checkResultIsEnough = currentQuantity <= 0 ? true : false;
                    }
                    else
                    {
                        existProductModels.Add(new ExistProductModel()
                        {
                            checkResultIsEnough = currentQuantity == 0 ? true : false,
                            productId = cartModel.ProductId,
                            Quantity = notEditableCurrentQuantity
                        });
                    }
                }
                else
                {
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

                    var existProduct = existProductModels.Find(x => x.productId == cartModel.ProductId);

                    var currentQuantity = cartModel.Quantity;
                    var notEditableQuantity = currentQuantity;
                    currentQuantity += quantityCanThem;
                    if (existProduct != null)
                    {
                        currentQuantity += existProduct.Quantity;
                    }
                    bool isEnough = true;
                    var data = await queryToCheck.Where(x => (x.Product_ID.Equals(cartModel.ProductId) && x.TotalQuantity >= currentQuantity)).Select(selector => selector.Site_ID).ToListAsync();

                    bigSiteList.Add(data);

                    var findExist = existProductModels.Find(x => x.productId == cartModel.ProductId);

                    if (findExist != null)
                    {
                        findExist.Quantity = notEditableQuantity + quantityCanThem;
                        findExist.checkResultIsEnough = isEnough;
                    }
                    else
                    {
                        existProductModels.Add(new ExistProductModel()
                        {
                            checkResultIsEnough = isEnough,
                            productId = cartModel.ProductId,
                            Quantity = notEditableQuantity
                        });
                    }
                }

                //ngoài range
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
