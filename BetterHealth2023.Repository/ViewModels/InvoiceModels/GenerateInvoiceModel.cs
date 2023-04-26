using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InvoiceModels
{
    public class GenerateInvoiceModel
    {
        public string OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExportedDate { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string CustomerEmail { get; set; }

        public int TotalQuantity { get; set; }
        public double SubTotalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double ShippingPrice { get; set; }
        public double TotalPrice { get; set; }

        public SiteInformationModel siteInformationModel { get; set; }
        public List<ProductInvoiceModel> productInvoiceModels { get; set; }
    }

    public class SiteInformationModel
    {
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }

        public string EmployeeName { get; set; }
    }
    public class ProductInvoiceModel
    {
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string UnitName { get; set; }
        public double TotalPrice { get; set; }

        public string Note { get; set; }
    }
}
