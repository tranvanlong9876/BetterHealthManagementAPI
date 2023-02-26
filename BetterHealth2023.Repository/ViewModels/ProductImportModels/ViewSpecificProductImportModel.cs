using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class ViewSpecificProductImportModel
    {

        public string Id { get; set; }
        public string SiteId { get; set; }

        public string ManagerId { get; set; }

        public DateTime ImportDate { get; set; }
        public string Note { get; set; }
        public double TotalProductPrice { get; set; }
        public double TaxPrice { get; set; }
        public double TotalShippingFee { get; set; }
        public double TotalPrice { get; set; }
        public bool IsReleased { get; set; }

        public List<ViewSpecificProductImportDetails> productImportDetails { get; set; }
    }

    public class ViewSpecificProductImportDetails
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double ImportPrice { get; set; }

        public List<ViewSpecificProductImportBatches> productBatches { get; set; }
    }

    public class ViewSpecificProductImportBatches
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public bool IsOutOfStock { get; set; }
    }
}
