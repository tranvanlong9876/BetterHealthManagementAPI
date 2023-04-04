using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ProductUserTargetModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Category_Main:
            CreateMap<CategoryMain, MainCategoryViewModel>();
            CreateMap<CategoryMain, CreateCategoryModel>();
            CreateMap<CreateCategoryModel, CategoryMain>();
            CreateMap<UpdateCategoryModel, CategoryMain>();

            //Sub_Category
            CreateMap<SubCategory, SubCategoryViewModel>();
            CreateMap<SubCategory, CreateSubCategoryModel>();
            CreateMap<CreateSubCategoryModel, SubCategory>();

            //Product
            CreateMap<CreateProductModel, ProductParent>();
            CreateMap<CreateProductDescriptionModel, ProductDescription>();
            CreateMap<CreateProductDetailModel, ProductDetail>();
            CreateMap<CreateProductIngredientModel, ProductIngredientDescription>();

            CreateMap<ProductDescription, UpdateProductDescriptionModel>();
            CreateMap<ProductDetail, UpdateProductDetailModel>();
            CreateMap<ProductParent, UpdateProductViewModel>();
            //Unit
            CreateMap<Unit, ViewUnitModel>();

            //Product Ingredient
            CreateMap<ProductIngredient, ViewProductIngredient>();

            //Product Import
            CreateMap<CreateProductImportModel, ProductImportReceipt>();
            CreateMap<ProductImportDetails, ProductImportDetail>();
            CreateMap<ProductImportBatches, ProductImportBatch>();
            CreateMap<UpdateProductImportDetails, ProductImportDetail>();
            CreateMap<UpdateProductImportBatches, ProductImportBatch>();

            CreateMap<ProductImportReceipt, ViewSpecificProductImportModel>();
            CreateMap<ProductImportDetail, ViewSpecificProductImportDetails>();
            CreateMap<ProductImportBatch, ViewSpecificProductImportBatches>();

            //Product Discount
            CreateMap<CreateProductDiscountModel, ProductDiscount>();
            CreateMap<ProductDiscountViewList, ProductDiscountViewSpecific>();
            CreateMap<ProductDiscount, ViewProductDiscountSpecific>();
            CreateMap<EventProductDiscount, ProductDiscountView>();

            //
            CreateMap<OrderDetail, SendingEmailProductModel>();

            //ProductUserTarget
            CreateMap<ProductUserTarget, ViewUserTargetModel>();
        }
    }
}
