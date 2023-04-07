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
        public string OrderStatusName { get; set; }
        public double TotalPrice { get; set; }
        public int UsedPoint { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool NeedAcceptance { get; set; }
        public List<ViewSpecificOrderProduct> orderProducts { get; set; }
        public ViewSpecificActionStatus actionStatus { get; set; }
        public ViewSpecificOrderContactInfo orderContactInfo { get; set; }
        public ViewSpecificOrderPickUp orderPickUp { get; set; }
        public ViewSpecificOrderDelivery orderDelivery { get; set; }
    }

    public class ViewSpecificOrderPickUp
    {
        public string DatePickUp { get; set; }
        public string TimePickUp { get; set; }
    }

    public class ViewSpecificOrderDelivery
    {
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string HomeNumber { get; set; }
        public string FullyAddress { get; set; }
        public double ShippingFee { get; set; }
        public string AddressId { get; set; }

        public string EstimatedDeliveryTime { get; set; }
    }

    public class ViewSpecificActionStatus
    {
        public bool CanAccept { get; set; }
        public string StatusMessage { get; set; }
        //hiển thị đối với đơn hàng Giao hàng
        public List<ViewSpecificMissingProduct> missingProducts { get; set; }
    }

    public class ViewSpecificMissingProduct
    {
        public string ProductId { get; set; }
        public int missingQuantity { get; set; }
        public string StatusMessage { get; set; }
    }

    public class ViewSpecificOrderContactInfo
    {
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class ViewSpecificOrderProduct
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public bool IsBatches { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double PriceTotal { get; set; }
        public string ProductNoteFromPharmacist { get; set; }
        public List<ViewSpecificOrderBatch> orderBatches { get; set; }
    }

    public class ViewSpecificOrderBatch
    {
        public DateTime ManufacturerDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Quantity { get; set; }
        public string UnitName { get; set; }
    }
}
