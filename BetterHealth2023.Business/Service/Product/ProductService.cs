using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductParentRepos;
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

        public ProductService(IProductDescriptionRepo productDescriptionRepo
            , IProductIngredientDescriptionRepo productIngredientDescriptionRepo
            , IProductParentRepo productParentRepo
            , IProductDetailRepo productDetailRepo
            , IProductImageRepo productImageRepo)
        {
            _productDescriptionRepo = productDescriptionRepo;
            _productIngredientDescriptionRepo = productIngredientDescriptionRepo;
            _productParentRepo = productParentRepo;
            _productDetailRepo = productDetailRepo;
            _productImageRepo = productImageRepo;
        }
        public async Task<CreateProductErrorModel> CreateProduct(CreateProductModel createProductModel)
        {
            var checkError = new CreateProductErrorModel();
            bool duplicateBarCode;

            foreach(var pro_details in createProductModel.productDetailModel)
            {
                if(!string.IsNullOrWhiteSpace(pro_details.BarCode))
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

            foreach(var product_ingredient in List_Product_Ingredient)
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

            //Insert Product Details
            var List_Product_Details = createProductModel.productDetailModel;
            foreach(var product_details in List_Product_Details)
            {
                var product_details_id = Guid.NewGuid().ToString();
                var productDetailDB = _productDetailRepo.TransferBetweenTwoModels<CreateProductDetailModel, ProductDetail>(product_details);
                productDetailDB.Id = product_details_id;
                productDetailDB.ProductIdParent = product_parent_id;
                check = await _productDetailRepo.Insert(productDetailDB);

                //insert Image each time success.

                if(check)
                {
                    var List_Product_Images = product_details.imageURL;
                    foreach(var product_images in List_Product_Images)
                    {
                        var product_image_id = Guid.NewGuid().ToString();
                        var product_image_db = new Repository.DatabaseModels.ProductImage()
                        {
                            Id = product_image_id,
                            ImageUrl = product_images.imageURL,
                            ProductId = product_details_id,
                            IsFirstImage = product_images.IsFirstImage.HasValue ? false : (bool) product_images.IsFirstImage
                        };
                        check = await _productImageRepo.Insert(product_image_db);
                    }
                    //done insert product images.
                }
            }

            if (check) checkError.isError = false;

            return checkError;

        }

        public async Task<PagedResult<ViewProductListModel>> GetAllProduct(ProductPagingRequest pagingRequest, bool isInternal)
        {
            var productParentList = await _productDetailRepo.GetProductParentDistinct();

            var productModelList = new List<ViewProductListModel>();

            foreach(var productParent in productParentList){
                if(isInternal)
                {
                    productModelList.AddRange(await _productDetailRepo.GetAllProductForInternal(productParent.Id, pagingRequest.isSell));
                } else
                {
                    productModelList.AddRange(await _productDetailRepo.GetAllProductWithParent(productParent.Id, productParent.LoadIsSell));
                }
            }

            if (!string.IsNullOrEmpty(pagingRequest.productName))
            {
                productModelList = productModelList.Where(x => (x.Name.Contains(pagingRequest.productName.Trim())) || (x.BarCode.Contains(pagingRequest.productName.Trim()))).ToList();
            }

            if (!string.IsNullOrEmpty(pagingRequest.subCategoryID))
            {
                productModelList = productModelList.Where(x => x.SubCategoryId.Equals(pagingRequest.subCategoryID.Trim())).ToList();
            }

            if (pagingRequest.isPrescription.HasValue)
            {
                productModelList = productModelList.Where(x => x.IsPrescription.Equals(pagingRequest.isPrescription)).ToList();
            }

            if (!string.IsNullOrEmpty(pagingRequest.manufacturerID))
            {
                productModelList = productModelList.Where(x => x.ManufacturerId.Equals(pagingRequest.manufacturerID.Trim())).ToList();
            }

            var totalRow = productModelList.Count();

            productModelList = productModelList.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                .Take(pagingRequest.pageItems).ToList();

            var pageResult = new PagedResult<ViewProductListModel>(productModelList, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);

            for (var i = 0; i < pageResult.Items.Count; i++)
            {
                var image = await _productImageRepo.GetProductImage(pageResult.Items[i].Id);
                var productUnitList = await _productDetailRepo.GetProductLaterUnit(await _productDetailRepo.GetProductParentID(pageResult.Items[i].Id), pageResult.Items[i].UnitLevel);
                var productUnitName = GetStringUnit(productUnitList);
                pageResult.Items[i].imageModel = image;
                pageResult.Items[i].TotalUnitOnly = productUnitName;
                pageResult.Items[i].NameWithUnit = pageResult.Items[i].Name + " (" + productUnitName + ")";
            }

            return pageResult;
        }

        public async Task<ViewSpecificProductModel> GetViewProduct(string productId, bool isInternal)
        {
            var productModel = await _productDetailRepo.GetSpecificProduct(productId, isInternal);
            if (productModel == null) return null;
            productModel.descriptionModels.ingredientModel = await _productIngredientDescriptionRepo.GetProductIngredient(productModel.descriptionModels.Id);
            productModel.imageModels = await _productImageRepo.getProductImages(productModel.Id);
            var productUnitName = GetStringUnit(await _productDetailRepo.GetProductLaterUnit(productModel.ProductIdParent, productModel.UnitLevel));
            var productUnitPreferences = await _productDetailRepo.GetProductUnitButThis(productModel.ProductIdParent, productModel.UnitLevel);
            productModel.NameWithUnit = productModel.Name + " (" + productUnitName + ")";
            productModel.TotalUnitOnly = productUnitName;
            productModel.productUnitReferences = productUnitPreferences;
            return productModel;
        }

        public async Task<UpdateProductViewModel> GetViewProductForUpdate(string productId)
        {
            var productParentID = await _productDetailRepo.GetProductParentID(productId);
            if(productParentID == null)
            {
                return null;
            }
            var productModel = await _productParentRepo.GetViewModel<UpdateProductViewModel>(productParentID);

            //get product description
            var productDescriptionModel = await _productDescriptionRepo.GetViewModel<UpdateProductDescriptionModel>(productModel.ProductDescriptionId);
            productModel.descriptionModel = productDescriptionModel;
            //get product ingredient
            if(productModel.descriptionModel != null)
            {
                var productIngredientModel = await _productIngredientDescriptionRepo.GetProductIngredientUpdate(productModel.descriptionModel.Id);
                productModel.descriptionModel.ingredientModel = productIngredientModel;
            }
            //get list product detail
            var productDetailList = await _productDetailRepo.GetProductDetailLists(productParentID);
            for(var i = 0; i < productDetailList.Count; i++)
            {
                productDetailList[i].ImageModels = await _productImageRepo.getProductImagesUpdate(productDetailList[i].Id);
            }
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

            for(int i = 0; i < updateProductModel.productDetailModel.Count; i++)
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
            productParentModel.IsPrescription = updateProductModel.isPrescription;
            //done general information

            //update specific information
            for(var i = 0; i < updateProductModel.productDetailModel.Count(); i++)
            {
                var productDetailModelUpdate = updateProductModel.productDetailModel[i];
                var productDetailDB = await _productDetailRepo.Get(productDetailModelUpdate.Id);
                productDetailDB.UnitId = productDetailModelUpdate.UnitId;
                productDetailDB.UnitLevel = productDetailModelUpdate.UnitLevel;
                productDetailDB.Quantitative = productDetailModelUpdate.Quantitative;
                productDetailDB.SellQuantity = productDetailModelUpdate.SellQuantity;
                productDetailDB.Price = productDetailModelUpdate.Price;
                productDetailDB.IsSell = productDetailModelUpdate.IsSell;
                productDetailDB.BarCode = productDetailModelUpdate.BarCode;

                await _productDetailRepo.Update();

                await _productImageRepo.removeAllImages(productDetailDB.Id);
                List<Repository.DatabaseModels.ProductImage> productImageDBs = new();
                if(productDetailModelUpdate.ImageModels != null)
                {
                    for (var j = 0; j < productDetailModelUpdate.ImageModels.Count; j++)
                    {
                        var imageModel = productDetailModelUpdate.ImageModels[j];
                        Repository.DatabaseModels.ProductImage productImageDB = new()
                        {
                            Id = String.IsNullOrWhiteSpace(imageModel.Id) ? Guid.NewGuid().ToString() : imageModel.Id,
                            ImageUrl = imageModel.ImageUrl,
                            ProductId = productDetailDB.Id
                        };
                        productImageDBs.Add(productImageDB);
                    }
                    if (productImageDBs.Count >= 1)
                    {
                        await _productImageRepo.addMultipleImages(productImageDBs);
                    }
                }
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

            await _productIngredientDescriptionRepo.RemoveAllProductIngredients(productDescriptionDB.Id);
            List<ProductIngredientDescription> productIngredientDescriptions = new();
            if(productDescriptionUpdate.ingredientModel != null)
            {
                for (var k = 0; k < productDescriptionUpdate.ingredientModel.Count; k++)
                {
                    var productIngredientDescription = productDescriptionUpdate.ingredientModel[k];
                    ProductIngredientDescription productIngredientDescriptionDB = new()
                    {
                        Id = String.IsNullOrWhiteSpace(productIngredientDescription.Id) ? Guid.NewGuid().ToString() : productIngredientDescription.Id,
                        IngredientId = productIngredientDescription.IngredientId,
                        Content = productIngredientDescription.Content,
                        UnitId = productIngredientDescription.UnitId,
                        ProductDescriptionId = productDescriptionDB.Id
                    };
                    productIngredientDescriptions.Add(productIngredientDescriptionDB);
                }
                if (productIngredientDescriptions.Count >= 1)
                {
                    await _productIngredientDescriptionRepo.AddMultipleProductIngredients(productIngredientDescriptions);
                }
            }
            checkError.isError = false;
            checkError.productViewModel = await GetViewProductForUpdate(updateProductModel.productDetailModel[0].Id);

            return checkError;
            
        }

        private string GetStringUnit(List<ProductUnitModel> productUnitList)
        {
            var namewithUnit = String.Empty;
            if (productUnitList.Count >= 1)
            {
                for (var j = 0; j < productUnitList.Count; j++)
                {
                    var productUnit = productUnitList[j];
                    if(j == 0)
                    {
                        namewithUnit = namewithUnit + "1 " + productUnit.UnitName;
                    } else
                    {
                        namewithUnit = namewithUnit + productUnit.Quantitative + " " + productUnit.UnitName;
                    }
                    
                    if (j != productUnitList.Count - 1) namewithUnit += " x ";
                }
            }
            return namewithUnit;
        }
    }
}
