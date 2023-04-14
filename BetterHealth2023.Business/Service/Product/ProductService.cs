using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductParentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductUserTargetRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.ProductErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ProductUserTargetModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductDescriptionRepo _productDescriptionRepo;
        private readonly IProductIngredientDescriptionRepo _productIngredientDescriptionRepo;
        private readonly IProductParentRepo _productParentRepo;
        private readonly IProductDetailRepo _productDetailRepo;
        private readonly IProductImageRepo _productImageRepo;
        private readonly IProductEventDiscountRepo _productEventDiscountRepo;
        private readonly ISiteInventoryRepo _siteInventoryRepo;
        private readonly IProductUserTargetRepo _productUserTargetRepo;

        public ProductService(IProductDescriptionRepo productDescriptionRepo
            , IProductIngredientDescriptionRepo productIngredientDescriptionRepo
            , IProductParentRepo productParentRepo
            , IProductDetailRepo productDetailRepo
            , IProductImageRepo productImageRepo
            , IProductEventDiscountRepo productEventDiscountRepo, ISiteInventoryRepo siteInventoryRepo, IProductUserTargetRepo productUserTargetRepo)
        {
            _productDescriptionRepo = productDescriptionRepo;
            _productIngredientDescriptionRepo = productIngredientDescriptionRepo;
            _productParentRepo = productParentRepo;
            _productDetailRepo = productDetailRepo;
            _productImageRepo = productImageRepo;
            _productEventDiscountRepo = productEventDiscountRepo;
            _siteInventoryRepo = siteInventoryRepo;
            _productUserTargetRepo = productUserTargetRepo;
        }

        public async Task<CartItem> AddMoreProductInformationToCart(string productId)
        {
            return await _productDetailRepo.AddMoreProductInformationToCart(productId);
        }

        public async Task<CreateProductErrorModel> CreateProduct(CreateProductModel createProductModel)
        {
            var checkError = new CreateProductErrorModel();
            bool duplicateBarCode;

            if (!string.IsNullOrEmpty(createProductModel.UserUsageTarget))
            {
                if (!int.TryParse(createProductModel.UserUsageTarget, out _)) throw new ArgumentException("UserTarget buộc phải là kí tự số và nằm trong 1-4 hoặc kí tự rỗng/null.");
            }

            foreach (var pro_details in createProductModel.productDetailModel)
            {
                if (!string.IsNullOrWhiteSpace(pro_details.BarCode))
                {
                    duplicateBarCode = await _productDetailRepo.CheckDuplicateBarCode(pro_details.BarCode);

                    if (duplicateBarCode)
                    {
                        checkError.isError = true;
                        checkError.DuplicateBarCode = "Mã BarCode sản phẩm bị trùng lặp.";
                        checkError.BarCodeError = pro_details.BarCode;
                        return checkError;
                    }
                }
            }

            //id
            var desc_id = Guid.NewGuid().ToString();
            var product_parent_id = Guid.NewGuid().ToString();

            //Insert Product Description

            var product_desc = _productDescriptionRepo.TransferBetweenTwoModels<CreateProductDescriptionModel, ProductDescription>(createProductModel.descriptionModel);
            product_desc.Id = desc_id;
            var check = await _productDescriptionRepo.Insert(product_desc);
            //done Insert Product Description

            //Insert Product Ingredient
            var List_Product_Ingredient = createProductModel.descriptionModel.ingredientModel;

            foreach (var product_ingredient in List_Product_Ingredient)
            {
                var product_ingre_desc_id = Guid.NewGuid().ToString();
                var product_ingre_db = _productIngredientDescriptionRepo.TransferBetweenTwoModels<CreateProductIngredientModel, ProductIngredientDescription>(product_ingredient);
                product_ingre_db.Id = product_ingre_desc_id;
                product_ingre_db.ProductDescriptionId = desc_id;
                check = await _productIngredientDescriptionRepo.Insert(product_ingre_db);
            }
            //Done Insert Product Ingredient

            //Insert Product Parent
            var productparentDB = _productParentRepo.TransferBetweenTwoModels<CreateProductModel, ProductParent>(createProductModel);
            productparentDB.IsDelete = false;
            productparentDB.Id = product_parent_id;
            productparentDB.ProductDescriptionId = desc_id;
            productparentDB.UserTarget = string.IsNullOrEmpty(createProductModel.UserUsageTarget) ? null : int.Parse(createProductModel.UserUsageTarget);
            productparentDB.CreatedDate = CustomDateTime.Now;
            productparentDB.UpdatedDate = CustomDateTime.Now;
            check = await _productParentRepo.Insert(productparentDB);
            //Done Insert Product Parent

            var List_Product_Images = createProductModel.imageModel;
            foreach (var product_images in List_Product_Images)
            {
                var product_image_id = Guid.NewGuid().ToString();
                var product_image_db = new ProductImage()
                {
                    Id = product_image_id,
                    ImageUrl = product_images.imageURL,
                    ProductId = product_parent_id,
                    IsFirstImage = !(product_images.IsFirstImage.HasValue) ? false : (bool)product_images.IsFirstImage
                };
                check = await _productImageRepo.Insert(product_image_db);
            }
            //done insert product images.


            //Insert Product Details
            var List_Product_Details = createProductModel.productDetailModel;
            foreach (var product_details in List_Product_Details)
            {
                var product_details_id = Guid.NewGuid().ToString();
                var productDetailDB = _productDetailRepo.TransferBetweenTwoModels<CreateProductDetailModel, ProductDetail>(product_details);
                productDetailDB.Id = product_details_id;
                productDetailDB.ProductIdParent = product_parent_id;
                productDetailDB.IsSell = product_details.UnitLevel == 1 ? true : product_details.IsSell;
                productDetailDB.Quantitative = product_details.UnitLevel == 1 ? 1 : product_details.Quantitative;
                productDetailDB.SellQuantity = 1;
                if (string.IsNullOrEmpty(productDetailDB.BarCode)) productDetailDB.BarCode = null;
                check = await _productDetailRepo.Insert(productDetailDB);

            }

            if (check) checkError.isError = false;

            return checkError;

        }

        public async Task<PagedResult<ViewProductListModel>> GetAllProductsPagingForCustomer(ProductPagingRequest pagingRequest)
        {
            if (!string.IsNullOrEmpty(pagingRequest.userTarget))
            {
                if (!int.TryParse(pagingRequest.userTarget, out _))
                {
                    throw new ArgumentException("UserTarger bắt buộc phải là số từ 1 - 4.");
                }
            }
            var pageResult = await _productDetailRepo.GetAllProductsPagingForCustomer(pagingRequest);
            for (var i = 0; i < pageResult.Items.Count; i++)
            {
                var productIdParent = await _productDetailRepo.GetProductParentID(pageResult.Items[i].Id);
                var filterRequest = new ProductUnitFilterRequest()
                {
                    isSell = true
                };
                var productUnitList = await _productDetailRepo.GetProductLaterUnitWithFilter(productIdParent, pageResult.Items[i].UnitLevel, filterRequest);
                pageResult.Items[i].productUnitReferences = productUnitList;
                var image = await _productImageRepo.GetProductImage(productIdParent);
                pageResult.Items[i].imageModel = image;

                for (int j = 0; j < pageResult.Items[i].productUnitReferences.Count; j++)
                {
                    var productReferences = pageResult.Items[i].productUnitReferences[j];

                    var productDiscount = await _productEventDiscountRepo.GetProductDiscount(productReferences.Id);

                    if (productReferences.Id.Equals(pageResult.Items[i].Id))
                    {
                        if (productDiscount != null)
                        {
                            if (productDiscount.DiscountMoney.HasValue)
                            {
                                pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price - productDiscount.DiscountMoney.Value;
                            }

                            if (productDiscount.DiscountPercent.HasValue)
                            {
                                pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price - (pageResult.Items[i].Price * productDiscount.DiscountPercent.Value / 100);
                            }
                            pageResult.Items[i].discountModel = productDiscount;
                        }
                        else
                        {
                            pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price;
                        }
                    }

                    //gán references
                    if (productDiscount != null)
                    {
                        if (productDiscount.DiscountMoney.HasValue)
                        {
                            productReferences.PriceAfterDiscount = productReferences.Price - productDiscount.DiscountMoney.Value;
                        }

                        if (productDiscount.DiscountPercent.HasValue)
                        {
                            productReferences.PriceAfterDiscount = productReferences.Price - (productReferences.Price * productDiscount.DiscountPercent.Value / 100);
                        }
                    }
                    else
                    {
                        productReferences.PriceAfterDiscount = productReferences.Price;
                    }
                }

            }
            return pageResult;
        }

        public async Task<IActionResult> GetAllProductsPagingForHomePage(ProductPagingHomePageRequest pagingRequest)
        {
            var pageResult = await _productDetailRepo.GetAllProductsPagingForHomePage(pagingRequest);

            for (var i = 0; i < pageResult.Items.Count; i++)
            {
                var productIdParent = await _productDetailRepo.GetProductParentID(pageResult.Items[i].Id);
                var filterRequest = new ProductUnitFilterRequest()
                {
                    isSell = true
                };
                var productUnitList = await _productDetailRepo.GetProductLaterUnitWithFilter(productIdParent, pageResult.Items[i].UnitLevel, filterRequest);
                pageResult.Items[i].productUnitReferences = productUnitList;
                var image = await _productImageRepo.GetProductImage(productIdParent);
                pageResult.Items[i].imageModel = image;

                for (int j = 0; j < pageResult.Items[i].productUnitReferences.Count; j++)
                {
                    var productReferences = pageResult.Items[i].productUnitReferences[j];

                    var productDiscount = await _productEventDiscountRepo.GetProductDiscount(productReferences.Id);

                    if (productReferences.Id.Equals(pageResult.Items[i].Id))
                    {
                        if (productDiscount != null)
                        {
                            if (productDiscount.DiscountMoney.HasValue)
                            {
                                pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price - productDiscount.DiscountMoney.Value;
                            }

                            if (productDiscount.DiscountPercent.HasValue)
                            {
                                pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price - (pageResult.Items[i].Price * productDiscount.DiscountPercent.Value / 100);
                            }
                            pageResult.Items[i].discountModel = productDiscount;
                        }
                        else
                        {
                            pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price;
                        }
                    }

                    //gán references
                    if (productDiscount != null)
                    {
                        if (productDiscount.DiscountMoney.HasValue)
                        {
                            productReferences.PriceAfterDiscount = productReferences.Price - productDiscount.DiscountMoney.Value;
                        }

                        if (productDiscount.DiscountPercent.HasValue)
                        {
                            productReferences.PriceAfterDiscount = productReferences.Price - (productReferences.Price * productDiscount.DiscountPercent.Value / 100);
                        }
                    }
                    else
                    {
                        productReferences.PriceAfterDiscount = productReferences.Price;
                    }
                }

            }
            return new OkObjectResult(pageResult);
        }

        public async Task<PagedResult<ViewProductListModelForInternal>> GetAllProductsPagingForInternalUser(ProductPagingRequest pagingRequest, string userToken)
        {
            if (!string.IsNullOrEmpty(pagingRequest.userTarget))
            {
                if (!int.TryParse(pagingRequest.userTarget, out _))
                {
                    throw new ArgumentException("UserTarger bắt buộc phải là số từ 1 - 4.");
                }
            }
            var roleName = JwtUserToken.DecodeAPITokenToRole(userToken);
            var siteId = (roleName.Equals(Commons.PHARMACIST_NAME) || roleName.Equals(Commons.MANAGER_NAME)) ? JwtUserToken.GetWorkingSiteFromManagerAndPharmacist(userToken) : string.Empty;
            var pageResult = await _productDetailRepo.GetAllProductsPagingForInternalUser(pagingRequest);
            for (var i = 0; i < pageResult.Items.Count; i++)
            {
                var productIdParent = await _productDetailRepo.GetProductParentID(pageResult.Items[i].Id);
                var image = await _productImageRepo.GetProductImage(productIdParent);
                var productUnitList = await _productDetailRepo.GetProductLaterUnit(productIdParent, pageResult.Items[i].UnitLevel);
                var filterRequest = new ProductUnitFilterRequest()
                {
                    isSell = pagingRequest.isSell
                };
                var productUnitListAfterFilter = await _productDetailRepo.GetProductLaterUnitWithFilter(productIdParent, pageResult.Items[i].UnitLevel, filterRequest);
                var quantityConvert = CountTotalQuantityFromFirstToLastUnit(productUnitList);
                pageResult.Items[i].productUnitReferences = productUnitListAfterFilter;
                pageResult.Items[i].imageModel = image;

                if (roleName.Equals(Commons.PHARMACIST_NAME) || roleName.Equals(Commons.MANAGER_NAME))
                {
                    var productLastUnit = productUnitList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();

                    var productInventoryModel = new ProductInventoryModel()
                    {
                        UnitId = productLastUnit.UnitId,
                        UnitName = productLastUnit.UnitName,
                        siteInventoryModel = await _siteInventoryRepo.GetInventoryOfProductOfSite(productLastUnit.Id, siteId, quantityConvert)
                    };

                    productInventoryModel.Quantity = productInventoryModel.siteInventoryModel.TotalQuantity;

                    var siteInventoryModel = productInventoryModel.siteInventoryModel;
                    var intfirstUnit = (int)(siteInventoryModel.TotalQuantityForFirst / quantityConvert);
                    if (siteInventoryModel.TotalQuantity != siteInventoryModel.TotalQuantityForFirst)
                    {
                        var duThua = siteInventoryModel.TotalQuantity - siteInventoryModel.TotalQuantityForFirst;
                        productInventoryModel.siteInventoryModel.Message = $"Trong đó, còn lại {intfirstUnit} {productUnitList.Find(x => x.UnitLevel == 1).UnitName} (chưa động đến) và {duThua} {productLastUnit.UnitName}";
                    }
                    else
                    {
                        productInventoryModel.siteInventoryModel.Message = $"Trong đó, còn lại {intfirstUnit} {productUnitList.Find(x => x.UnitLevel == 1).UnitName} (chưa động đến).";
                    }

                    pageResult.Items[i].productInventoryModel = productInventoryModel;
                }

                for (int j = 0; j < pageResult.Items[i].productUnitReferences.Count; j++)
                {
                    var productReferences = pageResult.Items[i].productUnitReferences[j];

                    var productDiscount = await _productEventDiscountRepo.GetProductDiscount(productReferences.Id);

                    if (productReferences.Id.Equals(pageResult.Items[i].Id))
                    {
                        if (productDiscount != null)
                        {
                            if (productDiscount.DiscountMoney.HasValue)
                            {
                                pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price - productDiscount.DiscountMoney.Value;
                            }

                            if (productDiscount.DiscountPercent.HasValue)
                            {
                                pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price - (pageResult.Items[i].Price * productDiscount.DiscountPercent.Value / 100);
                            }
                            pageResult.Items[i].discountModel = productDiscount;
                        }
                        else
                        {
                            pageResult.Items[i].PriceAfterDiscount = pageResult.Items[i].Price;
                        }
                    }

                    //gán references
                    if (productDiscount != null)
                    {
                        if (productDiscount.DiscountMoney.HasValue)
                        {
                            productReferences.PriceAfterDiscount = productReferences.Price - productDiscount.DiscountMoney.Value;
                        }

                        if (productDiscount.DiscountPercent.HasValue)
                        {
                            productReferences.PriceAfterDiscount = productReferences.Price - (productReferences.Price * productDiscount.DiscountPercent.Value / 100);
                        }
                    }
                    else
                    {
                        productReferences.PriceAfterDiscount = productReferences.Price;
                    }
                }

            }
            return pageResult;
        }

        public async Task<IActionResult> GetAllProductUserTarget()
        {
            var userTargetList = await _productUserTargetRepo.GetAll<ViewUserTargetModel>();
            userTargetList.Insert(0, new ViewUserTargetModel()
            {
                Id = null,
                UserTargetName = Commons.ALL_USER_TARGET_USAGE
            });

            return new OkObjectResult(userTargetList);
        }

        public async Task<ViewSpecificProductModel> GetViewProduct(string productId, bool isInternal)
        {
            var productModel = await _productDetailRepo.GetSpecificProduct(productId, isInternal);
            if (productModel == null) return null;
            productModel.descriptionModels.ingredientModel = await _productIngredientDescriptionRepo.GetProductIngredient(productModel.descriptionModels.Id);
            productModel.imageModels = await _productImageRepo.getProductImages(productModel.ProductIdParent);
            var productUnitPreferences = await _productDetailRepo.GetProductUnitButThis(productModel.ProductIdParent, productModel.UnitLevel);
            productModel.productUnitReferences = productUnitPreferences;

            var productDiscountHeader = await _productEventDiscountRepo.GetProductDiscount(productModel.Id);
            if (productDiscountHeader != null)
            {
                if (productDiscountHeader.DiscountMoney.HasValue)
                {
                    productModel.PriceAfterDiscount = productModel.Price - productDiscountHeader.DiscountMoney.Value;
                }

                if (productDiscountHeader.DiscountPercent.HasValue)
                {
                    productModel.PriceAfterDiscount = productModel.Price - (productModel.Price * productDiscountHeader.DiscountPercent.Value / 100);
                }
                productModel.discountModel = _productEventDiscountRepo.TransferBetweenTwoModels<ProductDiscountViewList, ProductDiscountViewSpecific>(productDiscountHeader);
            }
            else
            {
                productModel.PriceAfterDiscount = productModel.Price;
            }


            for (int j = 0; j < productModel.productUnitReferences.Count; j++)
            {
                var productReferences = productModel.productUnitReferences[j];

                var productDiscount = await _productEventDiscountRepo.GetProductDiscount(productReferences.Id);

                //gán references
                if (productDiscount != null)
                {
                    if (productDiscount.DiscountMoney.HasValue)
                    {
                        productReferences.PriceAfterDiscount = productReferences.Price - productDiscount.DiscountMoney.Value;
                    }

                    if (productDiscount.DiscountPercent.HasValue)
                    {
                        productReferences.PriceAfterDiscount = productReferences.Price - (productReferences.Price * productDiscount.DiscountPercent.Value / 100);
                    }
                }
                else
                {
                    productReferences.PriceAfterDiscount = productReferences.Price;
                }
            }

            return productModel;
        }

        public async Task<UpdateProductViewModel> GetViewProductForUpdate(string productId)
        {
            var productParentID = await _productDetailRepo.GetProductParentID(productId);
            if (productParentID == null)
            {
                return null;
            }
            var productModel = await _productParentRepo.GetViewModel<UpdateProductViewModel>(productParentID);
            var productImages = await _productImageRepo.getProductImagesUpdate(productParentID);
            productModel.ImageModels = productImages;
            //get product description
            var productDescriptionModel = await _productDescriptionRepo.GetViewModel<UpdateProductDescriptionModel>(productModel.ProductDescriptionId);
            productModel.descriptionModel = productDescriptionModel;
            //get product ingredient
            if (productModel.descriptionModel != null)
            {
                var productIngredientModel = await _productIngredientDescriptionRepo.GetProductIngredientUpdate(productModel.descriptionModel.Id);
                productModel.descriptionModel.ingredientModel = productIngredientModel;
            }
            //get list product detail
            var productDetailList = await _productDetailRepo.GetProductDetailLists(productParentID);

            productModel.productDetailModel = productDetailList;

            return productModel;
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
        public async Task<UpdateProductErrorModel> UpdateProduct(UpdateProductEntranceModel updateProductModel)
        {
            var checkError = new UpdateProductErrorModel();
            //check duplicate bar code

            if (!string.IsNullOrEmpty(updateProductModel.UserTarget))
            {
                if (!int.TryParse(updateProductModel.UserTarget, out _)) throw new ArgumentException("UserTarget buộc phải là kí tự số và nằm trong 1-4 hoặc kí tự rỗng/null.");
            }

            var productDetailList = await _productDetailRepo.GetProductDetailLists(updateProductModel.Id);
            if (updateProductModel.productDetailModel.Count != productDetailList.Count)
            {
                checkError.isError = true;
                return checkError;
            }

            for (int i = 0; i < updateProductModel.productDetailModel.Count; i++)
            {
                var productDetailModel = updateProductModel.productDetailModel[i];
                if (!string.IsNullOrWhiteSpace(productDetailModel.BarCode))
                {
                    var duplicateBarCode = await _productDetailRepo.CheckDuplicateBarCodeUpdate(productDetailModel.BarCode, productDetailModel.Id);

                    if (duplicateBarCode)
                    {
                        checkError.isError = true;
                        checkError.DuplicateBarCode = "Mã BarCode sản phẩm bị trùng lặp.";
                        checkError.BarCodeError = productDetailModel.BarCode;
                        return checkError;
                    }
                }
            }
            var productParentModel = await _productParentRepo.Get(updateProductModel.Id);
            //update general information
            productParentModel.IsPrescription = updateProductModel.isPrescription;
            productParentModel.Name = updateProductModel.Name;
            productParentModel.SubCategoryId = updateProductModel.subCategoryId;
            productParentModel.ManufacturerId = updateProductModel.manufacturerId;
            productParentModel.UserTarget = string.IsNullOrEmpty(updateProductModel.UserTarget) ? null : int.Parse(updateProductModel.UserTarget);
            productParentModel.UpdatedDate = CustomDateTime.Now;

            //await _productImageRepo.removeAllImages(updateProductModel.Id);
            //Load lên list tạm
            List<ProductImage> productImagesNeedToDelete = await _productImageRepo.GetProductImageDBs(updateProductModel.Id);
            if (updateProductModel.ImageModels != null)
            {
                List<ProductImage> productImageInsertDBs = new();
                for (var j = 0; j < updateProductModel.ImageModels.Count; j++)
                {
                    var imageModel = updateProductModel.ImageModels[j];
                    if (String.IsNullOrWhiteSpace(imageModel.Id))
                    {
                        ProductImage productImageDB = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ImageUrl = imageModel.ImageUrl,
                            IsFirstImage = imageModel.IsFirstImage,
                            ProductId = updateProductModel.Id
                        };
                        productImageInsertDBs.Add(productImageDB);
                    }
                    else
                    {
                        //Just need to update
                        var productImageDBUpdate = await _productImageRepo.Get(imageModel.Id);
                        productImageDBUpdate.ImageUrl = imageModel.ImageUrl;
                        productImageDBUpdate.IsFirstImage = imageModel.IsFirstImage;
                        await _productImageRepo.Update();
                        productImagesNeedToDelete.Remove(productImagesNeedToDelete.Find(x => x.Id.Equals(imageModel.Id)));
                    }
                }
                if (productImageInsertDBs.Count >= 1)
                {
                    await _productImageRepo.InsertRange(productImageInsertDBs);
                }
                if (productImagesNeedToDelete.Count >= 1)
                {
                    await _productImageRepo.removeAllImages(productImagesNeedToDelete);
                }
            }


            //done general information

            //update specific information
            for (var i = 0; i < updateProductModel.productDetailModel.Count(); i++)
            {
                var productDetailModelUpdate = updateProductModel.productDetailModel[i];
                var productDetailDB = await _productDetailRepo.Get(productDetailModelUpdate.Id);
                //productDetailDB.UnitId = productDetailModelUpdate.UnitId;
                //productDetailDB.UnitLevel = productDetailModelUpdate.UnitLevel;
                //productDetailDB.Quantitative = productDetailModelUpdate.Quantitative;
                productDetailDB.Price = productDetailModelUpdate.Price;
                if (productDetailModelUpdate.UnitLevel != 1)
                {
                    productDetailDB.IsSell = productDetailModelUpdate.IsSell;
                }
                productDetailDB.BarCode = string.IsNullOrEmpty(productDetailModelUpdate.BarCode) ? null : productDetailModelUpdate.BarCode;

                await _productDetailRepo.Update();

            }
            //done update specific information

            //update product description
            var productDescriptionUpdate = updateProductModel.descriptionModel;
            var productDescriptionDB = await _productDescriptionRepo.Get(productDescriptionUpdate.Id);
            productDescriptionDB.Instruction = productDescriptionUpdate.Instruction;
            productDescriptionDB.Preserve = productDescriptionUpdate.Preserve;
            productDescriptionDB.SideEffect = productDescriptionUpdate.SideEffect;
            productDescriptionDB.Effect = productDescriptionUpdate.Effect;
            productDescriptionDB.Contraindications = productDescriptionUpdate.Contraindications;
            await _productDescriptionRepo.Update();

            //await _productIngredientDescriptionRepo.RemoveAllProductIngredients(productDescriptionDB.Id);
            //Load lên list tạm
            List<ProductIngredientDescription> productIngredientNeedToDelete = await _productIngredientDescriptionRepo.GetProductIngredientDB(productDescriptionUpdate.Id);
            List<ProductIngredientDescription> productIngredientDescriptionsInsert = new();
            if (productDescriptionUpdate.ingredientModel != null)
            {
                for (var k = 0; k < productDescriptionUpdate.ingredientModel.Count; k++)
                {
                    var productIngredientDescription = productDescriptionUpdate.ingredientModel[k];
                    if (String.IsNullOrWhiteSpace(productIngredientDescription.Id))
                    {
                        ProductIngredientDescription productIngredientDescriptionDB = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IngredientId = productIngredientDescription.IngredientId,
                            Content = productIngredientDescription.Content,
                            UnitId = productIngredientDescription.UnitId,
                            ProductDescriptionId = productDescriptionDB.Id
                        };
                        productIngredientDescriptionsInsert.Add(productIngredientDescriptionDB);
                    }
                    else
                    {
                        //just need to update
                        var productIngredientDescriptionDB = await _productIngredientDescriptionRepo.Get(productIngredientDescription.Id);
                        productIngredientDescriptionDB.IngredientId = productIngredientDescription.IngredientId;
                        productIngredientDescriptionDB.Content = productIngredientDescription.Content;
                        productIngredientDescriptionDB.UnitId = productIngredientDescription.UnitId;
                        await _productIngredientDescriptionRepo.Update();
                        productIngredientNeedToDelete.Remove(productIngredientNeedToDelete.Find(x => x.Id.Equals(productIngredientDescription.Id)));
                    }

                }
                if (productIngredientDescriptionsInsert.Count >= 1)
                {
                    await _productIngredientDescriptionRepo.AddMultipleProductIngredients(productIngredientDescriptionsInsert);
                }
                if (productIngredientNeedToDelete.Count >= 1)
                {
                    await _productIngredientDescriptionRepo.RemoveAllProductIngredients(productIngredientNeedToDelete);
                }
            }
            checkError.isError = false;
            checkError.productViewModel = await GetViewProductForUpdate(updateProductModel.productDetailModel[0].Id);

            return checkError;
        }

    }
}
