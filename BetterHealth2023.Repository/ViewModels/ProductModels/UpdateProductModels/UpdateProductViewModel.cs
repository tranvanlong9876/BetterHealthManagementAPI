using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels
{
    public class UpdateProductViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProductDescriptionId { get; set; }

        public string SubCategoryId { get; set; }

        public string ManufacturerId { get; set; }

        public bool IsPrescription { get; set; }

        public bool IsBatches { get; set; }

        public List<UpdateProductDetailModel> productDetailModel { get; set; }

        public UpdateProductDescriptionModel descriptionModel { get; set; }

        public List<UpdateProductImageModel> ImageModels { get; set; }
    }

    public class UpdateProductDetailModel
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string UnitId { get; set; }

        [JsonIgnore]
        public int UnitLevel { get; set; }
        [JsonIgnore]
        public int Quantitative { get; set; }
        [JsonIgnore]
        public int SellQuantity { get; set; }
        public double Price { get; set; }
        public bool IsSell { get; set; }

        public bool IsVisible { get; set; }
        public string BarCode { get; set; }
    }

    public class UpdateProductImageModel
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFirstImage { get; set; }
    }

    public class UpdateProductDescriptionModel
    {
        public string Id { get; set; }
        public string Effect { get; set; }
        public string Instruction { get; set; }
        public string SideEffect { get; set; }
        public string Contraindications { get; set; }
        public string Preserve { get; set; }

        public List<UpdateProductIngredientModel> ingredientModel { get; set; }
    }
    public class UpdateProductIngredientModel
    {
        public string Id { get; set; }
        public string IngredientId { get; set; }
        public double? Content { get; set; }
        public string UnitId { get; set; }
    }
}
