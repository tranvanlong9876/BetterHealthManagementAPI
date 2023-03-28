using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductParentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.ProductErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
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

        public ProductService(IProductDescriptionRepo productDescriptionRepo
            , IProductIngredientDescriptionRepo productIngredientDescriptionRepo
            , IProductParentRepo productParentRepo
            , IProductDetailRepo productDetailRepo
            , IProductImageRepo productImageRepo
            , IProductEventDiscountRepo productEventDiscountRepo, ISiteInventoryRepo siteInventoryRepo)
        {
            _productDescriptionRepo = productDescriptionRepo;
            _productIngredientDescriptionRepo = productIngredientDescriptionRepo;
            _productParentRepo = productParentRepo;
            _productDetailRepo = productDetailRepo;
            _productImageRepo = productImageRepo;
            _productEventDiscountRepo = productEventDiscountRepo;
            _siteInventoryRepo = siteInventoryRepo;
        }

        public async Task<CartItem> AddMoreProductInformationToCart(string productId)
        {
            return await _productDetailRepo.AddMoreProductInformationToCart(productId);
        }

        public async Task<CreateProductErrorModel> CreateProduct(CreateProductModel createProductModel)
        {
            var checkError = new CreateProductErrorModel();
            bool duplicateBarCode;

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
                check = await _productDetailRepo.Insert(productDetailDB);

            }

            if (check) checkError.isError = false;

            return checkError;

        }

        public async Task<PagedResult<ViewProductListModel>> GetAllProductsPagingForCustomer(ProductPagingRequest pagingRequest)
        {

            var pageResult = await _productDetailRepo.GetAllProductsPagingForCustomer(pagingRequest);
            for (var i = 0; i < pageResult.Items.Count; i++)
            {
                var productIdParent = await _productDetailRepo.GetProductParentID(pageResult.Items[i].Id);
                var image = await _productImageRepo.GetProductImage(productIdParent);
                pageResult.Items[i].imageModel = image;

                var productDiscount = await _productEventDiscountRepo.GetProductDiscount(pageResult.Items[i].Id);
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
            return pageResult;
        }

        public async Task<PagedResult<ViewProductListModelForInternal>> GetAllProductsPagingForInternalUser(ProductPagingRequest pagingRequest, string userToken)
        {
            var roleName = JwtUserToken.DecodeAPITokenToRole(userToken);
            var siteId = (roleName.Equals(Commons.PHARMACIST_NAME) || roleName.Equals(Commons.MANAGER_NAME)) ? JwtUserToken.GetWorkingSiteFromManagerAndPharmacist(userToken) : string.Empty;
            var pageResult = await _productDetailRepo.GetAllProductsPagingForInternalUser(pagingRequest);
            for (var i = 0; i < pageResult.Items.Count; i++)
            {
                var productIdParent = await _productDetailRepo.GetProductParentID(pageResult.Items[i].Id);
                var image = await _productImageRepo.GetProductImage(productIdParent);
                var productUnitList = await _productDetailRepo.GetProductLaterUnit(productIdParent, pageResult.Items[i].UnitLevel);
                pageResult.Items[i].productUnitReferences = productUnitList;
                pageResult.Items[i].imageModel = image;

                if(roleName.Equals(Commons.PHARMACIST_NAME) || roleName.Equals(Commons.MANAGER_NAME))
                {
                    var productLastUnit = productUnitList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();

                    var productInventoryModel = new ProductInventoryModel()
                    {
                        UnitId = productLastUnit.UnitId,
                        UnitName = productLastUnit.UnitName,
                        Quantity = await _siteInventoryRepo.GetInventoryOfProductOfSite(productLastUnit.Id, siteId)
                    };

                    pageResult.Items[i].productInventoryModel = productInventoryModel;
                }

                var productDiscount = await _productEventDiscountRepo.GetProductDiscount(pageResult.Items[i].Id);
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
            return pageResult;
        }

        public async Task<ViewSpecificProductModel> GetViewProduct(string productId, bool isInternal)
        {
            var productModel = await _productDetailRepo.GetSpecificProduct(productId, isInternal);
            if (productModel == null) return null;
            productModel.descriptionModels.ingredientModel = await _productIngredientDescriptionRepo.GetProductIngredient(productModel.descriptionModels.Id);
            productModel.imageModels = await _productImageRepo.getProductImages(productModel.ProductIdParent);
            var productUnitPreferences = await _productDetailRepo.GetProductUnitButThis(productModel.ProductIdParent, productModel.UnitLevel);
            productModel.productUnitReferences = productUnitPreferences;

            var productDiscount = await _productEventDiscountRepo.GetProductDiscount(productId);

            if (productDiscount != null)
            {
                if (productDiscount.DiscountMoney.HasValue)
                {
                    productModel.PriceAfterDiscount = productModel.Price - productDiscount.DiscountMoney.Value;
                }

                if (productDiscount.DiscountPercent.HasValue)
                {
                    productModel.PriceAfterDiscount = productModel.Price - (productModel.Price * productDiscount.DiscountPercent.Value / 100);
                }
                productModel.discountModel = _productEventDiscountRepo.TransferBetweenTwoModels<ProductDiscountViewList, ProductDiscountViewSpecific>(productDiscount);
            }
            else
            {
                productModel.PriceAfterDiscount = productModel.Price;
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

        public async Task<UpdateProductErrorModel> UpdateProduct(UpdateProductEntranceModel updateProductModel)
        {
            var checkError = new UpdateProductErrorModel();
            //check duplicate bar code
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
                if(productDetailModelUpdate.UnitLevel != 1)
                {
                    productDetailDB.IsSell = productDetailModelUpdate.IsSell;
                }
                productDetailDB.BarCode = productDetailModelUpdate.BarCode;

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
