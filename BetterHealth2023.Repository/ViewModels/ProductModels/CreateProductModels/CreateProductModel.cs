using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels
{
    public class CreateProductModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string SubCategoryId { get; set; }

        [StringLength(50)]
        public string ManufacturerId { get; set; }

        [Required]
        public bool IsPrescription { get; set; }

        
        public int LoadSellProduct { get; set; }

        [Required]
        public bool IsBatches { get; set; }

        [Required]
        public List<CreateProductDetailModel> productDetailModel { get; set; }

        [Required]
        public CreateProductDescriptionModel descriptionModel { get; set; }

    }
}
