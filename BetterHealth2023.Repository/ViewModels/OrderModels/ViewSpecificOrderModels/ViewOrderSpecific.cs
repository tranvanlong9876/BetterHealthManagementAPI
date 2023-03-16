using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels
{
    public class ViewOrderSpecific
    {
        public string Id { get; set; }
        public string PharmacistId { get; set; }
        public int OrderTypeId { get; set; }
        public string OrderTypeName { get; set; }
        public string SiteId { get; set; }
        public string OrderStatus { get; set; }
        public double TotalPrice { get; set; }
        public int UsedPoint { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool NeedAcceptance { get; set; }

        public ViewSpecificOrderProduct orderProducts { get; set; }
    }

    public class ViewSpecificOrderProduct
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Quantity { get; set; }
        public string OriginalPrice { get; set; }
        public string DiscountPrice { get; set; }
        public string PriceTotal { get; set; }
        public string ProductNoteFromPharmacist { get; set; }
        public List<ViewSpecificOrderBatch> orderBatches { get; set; }
    }

    public class ViewSpecificOrderBatch
    {
        public DateTime ManufacturerDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Quantity { get; set; }
    }
}
