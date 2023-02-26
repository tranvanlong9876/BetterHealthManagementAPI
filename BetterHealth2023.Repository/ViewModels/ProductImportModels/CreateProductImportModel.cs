using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class CreateProductImportModel
    {
        [Required]
        public List<ProductImportDetails> productImportDetails { get; set; }

        [JsonIgnore]
        public string SiteId { get; set; }
        
        [JsonIgnore]
        public string ManagerId { get; set; }
        public string Note { get; set; }
        [Required]
        public double TotalProductPrice { get; set; }
        [Required]
        public double TaxPrice { get; set; }
        [Required]
        public double TotalShippingFee { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public bool IsReleased { get; set; }
    }

    public class ProductImportDetails
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double ImportPrice { get; set; }

        public List<ProductImportBatches> productBatches { get; set; }
    }

    public class ProductImportBatches
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime ManufactureDate { get; set; }
        [Required]
        //[GreaterThan("ManufactureDate")]
        public DateTime ExpireDate { get; set; }
    }
}
