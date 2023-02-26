using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels
{
    public class CreateProductDetailModel
    {
        [Required]
        [StringLength(50)]
        public string UnitId { get; set; }
        [Required]
        public int UnitLevel { get; set; }
        [Required]
        public int Quantitative { get; set; }
        [Required]
        public int SellQuantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public bool IsSell { get; set; }
        [Required]
        public bool IsVisible { get; set; }

        [StringLength(50)]
        public string BarCode { get; set; }
        public List<ProductImage> imageURL { get; set; }
    }

    public class ProductImage {
        [Required]
        public string imageURL { get; set; }
        public bool? IsFirstImage { get; set; }
    
    }

}
