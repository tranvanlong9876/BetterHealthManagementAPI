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
        public List<string> imageURL { get; set; }
    }
}
