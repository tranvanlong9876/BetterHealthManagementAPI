﻿using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductParentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.ProductErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
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
                if(!string.IsNullOrEmpty(pro_details.BarCode))
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
                        var product_image_db = new ProductImage()
                        { 
                            Id = product_image_id,
                            ImageUrl = product_images,
                            ProductId = product_details_id
                        };
                        check = await _productImageRepo.Insert(product_image_db);
                    }
                    //done insert product images.
                }
            }

            if (check) checkError.isError = false;

            return checkError;

        }

        public async Task<PagedResult<ViewProductListModel>> GetAllProduct(ProductPagingRequest pagingRequest)
        {
            var productModel = await _productDetailRepo.GetAllProductsPaging(pagingRequest);

            for(var i = 0; i < productModel.Items.Count; i++)
            {
                var imageList = await _productImageRepo.getProductImages(productModel.Items[i].Id);
                var productUnitList = await _productDetailRepo.GetProductLaterUnit(await _productDetailRepo.GetProductParentID(productModel.Items[i].Id), productModel.Items[i].UnitLevel);
                var productUnitName = GetStringUnit(productUnitList);
                productModel.Items[i].imageModels = imageList;
                productModel.Items[i].TotalUnitOnly = productUnitName;
                productModel.Items[i].NameWithUnit = productModel.Items[i].Name + " (" + productUnitName + ")";
            }

            return productModel;
        }

        public async Task<ViewSpecificProductModel> GetViewProduct(string productId)
        {
            var productModel = await _productDetailRepo.GetSpecificProduct(productId);
            if (productModel == null) return null;
            productModel.descriptionModels.ingredientModel = await _productIngredientDescriptionRepo.GetProductIngredient(productModel.descriptionModels.Id);
            productModel.imageModels = await _productImageRepo.getProductImages(productModel.Id);
            var productUnitName = GetStringUnit(await _productDetailRepo.GetProductLaterUnit(productModel.ProductIdParent, productModel.UnitLevel));
            productModel.NameWithUnit = productModel.Name + " (" + productUnitName + ")";
            productModel.TotalUnitOnly = productUnitName;
            return productModel;
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
