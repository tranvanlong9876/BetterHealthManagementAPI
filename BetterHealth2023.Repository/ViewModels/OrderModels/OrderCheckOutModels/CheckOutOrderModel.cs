using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels
{
    public class CheckOutOrderModel
    {
        public string OrderId { get; set; }
        [Required]
        public int OrderTypeId { get; set; }
        public string SiteId { get; set; }
        public string PharmacistId { get; set; }
        [Required]
        public double SubTotalPrice { get; set; }
        [Required]
        public double DiscountPrice { get; set; }

        public double ShippingPrice { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public int UsedPoint { get; set; }
        [Required]
        public int PayType { get; set; }
        public string Note { get; set; }
        public List<OrderVoucher> Vouchers { get; set; }
        [Required]
        public List<OrderProduct> Products { get; set; }
        [Required]
        public ReveicerInformation ReveicerInformation { get; set; }

        public OrderPickUp OrderPickUp { get; set; }
        

    }

    public class OrderPickUp
    {
        [Required]
        [StringLength(50)]
        public string DatePickUp { get; set; }
        [Required]
        [StringLength(50)]
        public string TimePickUp { get; set; }
    }

    public class OrderProduct
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double OriginalPrice { get; set; }
        [Required]
        public double DiscountPrice { get; set; }
        [Required]
        public double TotalPrice { get; set; }
    }

    public class OrderVoucher
    {
        [Required]
        public string VoucherId { get; set; }
    }

    public class ReveicerInformation
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string HomeAddress { get; set; }
    }
}
