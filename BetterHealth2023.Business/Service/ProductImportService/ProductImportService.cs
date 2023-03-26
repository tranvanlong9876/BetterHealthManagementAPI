using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportBatchRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductImportService
{
    public class ProductImportService : IProductImportService
    {
        private readonly IProductImportRepo _productImportRepo;
        private readonly IProductImportDetailRepo _productImportDetailRepo;
        private readonly IProductImportBatchRepo _productImportBatchRepo;
        private readonly ISiteInventoryRepo _siteInventoryRepo;
        private readonly IProductDetailRepo _productDetailRepo;

        public ProductImportService(IProductImportRepo productImportRepo, IProductImportDetailRepo productImportDetailRepo, IProductImportBatchRepo productImportBatchRepo, ISiteInventoryRepo siteInventoryRepo, IProductDetailRepo productDetailRepo)
        {
            _productImportRepo = productImportRepo;
            _productImportDetailRepo = productImportDetailRepo;
            _productImportBatchRepo = productImportBatchRepo;
            _siteInventoryRepo = siteInventoryRepo;
            _productDetailRepo = productDetailRepo;
        }

        public async Task<CreateProductImportStatus> CreateProductImport(CreateProductImportModel importModel)
        {
            var checkError = new CreateProductImportStatus();
            for (var i = 0; i < importModel.productImportDetails.Count; i++)
            {
                var importDetailModel = importModel.productImportDetails[i];
                if (await _productImportRepo.checkProductManageByBatches(importDetailModel.ProductId))
                {
                    if (importDetailModel.productBatches == null)
                    {
                        checkError.isError = true;
                        checkError.notFoundBatches = "Đối với hàng khóa quản lý theo lô, cần nhập tối thiểu 1 lô hàng";
                        checkError.ProductIDNeedBatches = importDetailModel.ProductId;

                        return checkError;
                    }
                    if (importDetailModel.productBatches.Count == 0)
                    {
                        checkError.isError = true;
                        checkError.notFoundBatches = "Đối với hàng khóa quản lý theo lô, cần nhập tối thiểu 1 lô hàng";
                        checkError.ProductIDNeedBatches = importDetailModel.ProductId;
                        return checkError;
                    }
                }
            }

            string headerID = Guid.NewGuid().ToString();

            //Insert Header
            var productImportHeader = _productImportRepo.TransferBetweenTwoModels<CreateProductImportModel, ProductImportReceipt>(importModel);
            productImportHeader.Id = headerID;
            productImportHeader.ImportDate = CustomDateTime.Now;
            productImportHeader.ProductImportDetails = null;
            await _productImportRepo.Insert(productImportHeader);

            //Insert Detail
            for (int i = 0; i < importModel.productImportDetails.Count; i++)
            {
                var importModelDetails = importModel.productImportDetails[i];
                var productDetailDB = _productImportDetailRepo.TransferBetweenTwoModels<ProductImportDetails, ProductImportDetail>(importModelDetails);
                var productDetailId = Guid.NewGuid().ToString();
                productDetailDB.ReceiptId = headerID;
                productDetailDB.Id = productDetailId;
                await _productImportDetailRepo.Insert(productDetailDB);
                //Insert Detail 1.

                //Insert Batch
                if (importModelDetails.productBatches != null && await _productImportRepo.checkProductManageByBatches(productDetailDB.ProductId))
                {
                    for (int j = 0; j < importModelDetails.productBatches.Count; j++)
                    {
                        var importModelBatches = importModelDetails.productBatches[j];
                        var importModelBatchesDB = _productImportBatchRepo.TransferBetweenTwoModels<ProductImportBatches, ProductImportBatch>(importModelBatches);
                        var productBatchesId = Guid.NewGuid().ToString();
                        importModelBatchesDB.Id = productBatchesId;
                        importModelBatchesDB.ImportDetailId = productDetailId;
                        await _productImportBatchRepo.Insert(importModelBatchesDB);
                    }
                }
                //Done
            }

            //Done Insert Detail.

            //Check IsRelease True, Insert Into Site Warehouse!

            if (importModel.IsReleased)
            {
                await ReleaseProductImport(headerID, importModel.SiteId, true);
            }

            //Done Insert Inventory

            checkError.isError = false;
            return checkError;
        }

        public async Task<bool> ReleaseProductImportController(string productImportID, string siteID)
        {
            var productDetailsDB = await _productImportDetailRepo.GetProductImportDetails(productImportID);
            var ProductImportDB = await _productImportRepo.Get(productImportID);
            if (ProductImportDB.IsReleased) return false;
            if (productDetailsDB.Count > 0)
            {
                await ReleaseProductImport(productImportID, siteID, true);
            }
            
            ProductImportDB.IsReleased = true;
            await _productImportRepo.Update();
            return true;
        }

        public async Task<UpdateProductImportStatus> UpdateProductImport(UpdateProductImportModel updateProductImport)
        {
            var checkError = new UpdateProductImportStatus();
            var productImportReceipt = await _productImportRepo.Get(updateProductImport.Id);
            if (productImportReceipt == null)
            {
                checkError.isError = true;
                checkError.NotFound = "Không tìm thấy đơn nhập hàng cần update.";
                return checkError;
            }

            if (productImportReceipt.IsReleased)
            {
                checkError.isError = true;
                checkError.AlreadyReleased = "Đơn nhập hàng này không thể cập nhật do đã được duyệt.";
                return checkError;
            }

            for (var i = 0; i < updateProductImport.productImportDetails.Count; i++)
            {
                var importDetailModel = updateProductImport.productImportDetails[i];
                if (await _productImportRepo.checkProductManageByBatches(importDetailModel.ProductId))
                {
                    if (importDetailModel.productBatches == null)
                    {
                        checkError.isError = true;
                        checkError.NotFoundBatches = "Đối với hàng khóa quản lý theo lô, cần nhập tối thiểu 1 lô hàng";
                        checkError.ProductIDNeedBatches = importDetailModel.ProductId;
                        return checkError;
                    }
                    if (importDetailModel.productBatches.Count == 0)
                    {
                        checkError.isError = true;
                        checkError.NotFoundBatches = "Đối với hàng khóa quản lý theo lô, cần nhập tối thiểu 1 lô hàng";
                        checkError.ProductIDNeedBatches = importDetailModel.ProductId;
                        return checkError;
                    }
                }
            }

            //Update Header.
            productImportReceipt.ImportDate = CustomDateTime.Now;
            productImportReceipt.IsReleased = updateProductImport.IsReleased;
            productImportReceipt.TaxPrice = updateProductImport.TaxPrice;
            productImportReceipt.TotalProductPrice = updateProductImport.TotalProductPrice;
            productImportReceipt.TotalShippingFee = updateProductImport.TotalShippingFee;
            productImportReceipt.TotalPrice = updateProductImport.TotalPrice;
            productImportReceipt.Note = updateProductImport.Note;
            await _productImportRepo.Update();
            //Done

            //Update Detail.
            //1. Find a list detail existing.
            var importDetailsListDB = await _productImportDetailRepo.GetProductImportDetails(productImportReceipt.Id);

            for (int i = 0; i < updateProductImport.productImportDetails.Count; i++)
            {
                var updateProductDetail = updateProductImport.productImportDetails[i];
                if (updateProductDetail.Id == null)
                {
                    updateProductDetail.Id = Guid.NewGuid().ToString();
                    var insertDetailDB = _productImportDetailRepo.TransferBetweenTwoModels<UpdateProductImportDetails, ProductImportDetail>(updateProductDetail);
                    insertDetailDB.ReceiptId = productImportReceipt.Id;
                    await _productImportDetailRepo.Insert(insertDetailDB);
                    if (updateProductDetail.productBatches.Count > 0 && await _productImportRepo.checkProductManageByBatches(insertDetailDB.ProductId))
                    {
                        for (int j = 0; j < updateProductDetail.productBatches.Count; j++)
                        {
                            var updateBatchDetail = updateProductDetail.productBatches[j];
                            var insertBatchDB = _productImportDetailRepo.TransferBetweenTwoModels<UpdateProductImportBatches, ProductImportBatch>(updateBatchDetail);
                            insertBatchDB.Id = Guid.NewGuid().ToString();
                            insertBatchDB.ImportDetailId = insertDetailDB.Id;
                            await _productImportBatchRepo.Insert(insertBatchDB);
                        }
                    }
                }
                else
                {
                    var updateProductDetailDB = importDetailsListDB.Find(x => x.Id.Equals(updateProductDetail.Id));
                    updateProductDetailDB.ImportPrice = updateProductDetail.ImportPrice;
                    updateProductDetailDB.ProductId = updateProductDetail.ProductId;
                    updateProductDetailDB.Quantity = updateProductDetail.Quantity;
                    await _productImportDetailRepo.Update();
                    //get list batch of current productDetail.
                    if (await _productImportRepo.checkProductManageByBatches(updateProductDetailDB.ProductId))
                    {
                        var productBatchListDB = await _productImportBatchRepo.GetProductImportBatches(updateProductDetailDB.Id);
                        for (int k = 0; k < updateProductDetail.productBatches.Count; k++)
                        {
                            var updateBatchEntrance = updateProductDetail.productBatches[k];
                            if (updateBatchEntrance.Id == null)
                            {
                                var insertBatchesDB = _productImportBatchRepo.TransferBetweenTwoModels<UpdateProductImportBatches, ProductImportBatch>(updateBatchEntrance);
                                var productBatchesId = Guid.NewGuid().ToString();
                                insertBatchesDB.Id = productBatchesId;
                                insertBatchesDB.ImportDetailId = updateProductDetailDB.Id;
                                await _productImportBatchRepo.Insert(insertBatchesDB);
                            }
                            else
                            {
                                var updateBatchDB = productBatchListDB.Find(x => x.Id.Equals(updateBatchEntrance.Id));
                                updateBatchDB.ManufactureDate = updateBatchEntrance.ManufactureDate;
                                updateBatchDB.ExpireDate = updateBatchEntrance.ExpireDate;
                                updateBatchDB.Quantity = updateBatchEntrance.Quantity;
                                await _productImportBatchRepo.Update();
                                productBatchListDB.Remove(productBatchListDB.Find(x => x.Id.Equals(updateBatchEntrance.Id)));
                            }
                        }
                        //Remove All unused or not updated.
                        await _productImportBatchRepo.RemoveBatchesRange(productBatchListDB);
                    }

                    importDetailsListDB.Remove(importDetailsListDB.Find(x => x.Id.Equals(updateProductDetail.Id)));
                }
            }

            //Remove All unused or not updated details.
            await _productImportDetailRepo.RemoveRangesImportDetails(importDetailsListDB);

            //Done Update Detail.

            //Check need to insert into Inventory ?

            if (productImportReceipt.IsReleased)
            {
                await ReleaseProductImport(productImportReceipt.Id, productImportReceipt.SiteId, false);
            }
            // Done Insert Inventory.

            checkError.isError = false;
            return checkError;
        }

        public async Task<PagedResult<ViewListProductImportModel>> ViewListProductImportPaging(GetProductImportPagingRequest pagingRequest)
        {
            return await _productImportRepo.ViewListProductImportPaging(pagingRequest);
        }

        public async Task<ViewSpecificProductImportModel> ViewSpecificProductImport(string productImportID)
        {
            var productImportReceipt = await _productImportRepo.GetViewModel<ViewSpecificProductImportModel>(productImportID);

            if (productImportReceipt == null) return null;

            productImportReceipt.productImportDetails = await _productImportDetailRepo.GetProductImportDetailsViewModel(productImportID);

            for (int i = 0; i < productImportReceipt.productImportDetails.Count; i++)
            {
                var productImportDetailModel = productImportReceipt.productImportDetails[i];
                productImportReceipt.productImportDetails[i].productBatches = await _productImportBatchRepo.GetProductImportBatchesViewModel(productImportDetailModel.Id);
            }

            return productImportReceipt;
        }

        private async Task<bool> ReleaseProductImport(string productImportID, string siteID, bool isCreate)
        {
            var productImportReceipt = await _productImportRepo.Get(productImportID);
            var productImportDetailsDB = await _productImportDetailRepo.GetProductImportDetails(productImportID);
            foreach (var productImportDetail in productImportDetailsDB)
            {
                
                //Get All ProductUnitOfThisParent
                var productParentId = await _productDetailRepo.GetProductParentID(productImportDetail.ProductId);

                var productDetailDB = await _productDetailRepo.Get(productImportDetail.ProductId);
                //Get All Unit 
                var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);

                //Lấy cấp bậc thấp nhất của unit
                var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();

                var isBatch = await _productImportRepo.checkProductManageByBatches(productImportDetail.ProductId);

                //Có quản lý theo lô
                if (isBatch)
                {
                    //lấy ra toàn bộ lô hàng vừa import
                    var productImportBatchListDB = await _productImportBatchRepo.GetProductImportBatches(productImportDetail.Id);

                    //insert đơn vị thấp nhất của lô hàng
                    for(int i = 0; i < productImportBatchListDB.Count; i++)
                    {
                        var productImportBatchDB = productImportBatchListDB[i];
                        var siteInventoryBatch = new SiteInventoryBatch()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = productLastUnitDetail.Id,
                            Quantity = productImportBatchDB.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList),
                            SiteId = siteID,
                            ImportBatchId = productImportBatchDB.Id,
                            CreatedDate = CustomDateTime.Now,
                            UpdatedDate = CustomDateTime.Now
                        };
                        await _siteInventoryRepo.Insert(siteInventoryBatch);
                    }

                    //insert thẳng không cần check gì cả.
                    
                } else //không quản lý theo lô
                {
                    var SiteInventoryBatchDB = await _siteInventoryRepo.GetSiteInventory(siteID, productLastUnitDetail.Id);
                    if (SiteInventoryBatchDB == null)
                    {
                        SiteInventoryBatchDB = new SiteInventoryBatch()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = productLastUnitDetail.Id,
                            Quantity = productImportDetail.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList),
                            SiteId = siteID,
                            CreatedDate = CustomDateTime.Now,
                            UpdatedDate = CustomDateTime.Now
                        };
                        await _siteInventoryRepo.Insert(SiteInventoryBatchDB);
                    }
                    else
                    {
                        SiteInventoryBatchDB.UpdatedDate = CustomDateTime.Now;
                        SiteInventoryBatchDB.Quantity = SiteInventoryBatchDB.Quantity + (productImportDetail.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList));
                        await _siteInventoryRepo.Update();
                    }
                }
            }
            return true;
        }
        private int CountTotalQuantityFromFirstToLastUnit(List<ProductUnitModel> productDetailList)
        {
            int totalQuantity = 1;

            if (productDetailList.Count <= 1) return totalQuantity;

            for (int i = 0; i < productDetailList.Count - 1; i++)
            {
                totalQuantity = totalQuantity * productDetailList.Find(x => x.UnitLevel == (i + 2)).Quantitative;
            }

            return totalQuantity;
        }

    }
}
