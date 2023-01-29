using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels;
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
        }
    }
}
