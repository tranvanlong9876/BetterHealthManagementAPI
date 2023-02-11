using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ViewSpecificProductModel
    {
        public string Id { get; set; }
        public string ProductIdParent { get; set; }
        public string Name { get; set; }

        public string NameWithUnit { get; set; }
        public string TotalUnitOnly { get; set; }

        public string SubCategoryId { get; set; }

        public string ManufacturerId { get; set; }

        public bool IsPrescription { get; set; }
        public bool IsBatches { get; set; }

        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public int UnitLevel { get; set; }
        public double Price { get; set; }

        public ProductDescriptionModel descriptionModels { get; set; }
        public List<ProductImageView> imageModels { get; set; }
        public List<ProductUnitModel> productUnitReferences { get; set; }
    }
}
