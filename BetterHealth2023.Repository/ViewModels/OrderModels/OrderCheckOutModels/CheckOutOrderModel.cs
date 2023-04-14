using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels
{
    public class CheckOutOrderModel
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Sai dữ liệu, chỉ được phép nằm từ 1 đến 3. 1: Đến Lấy, 2: Nhận Tại Cửa Hàng, 3: Giao Hàng Tận Nơi.")]
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
        [Range(1, 2, ErrorMessage = "Sai dữ liệu, chỉ được phép nằm từ 1 đến 2. 1: Thanh Toán Khi Nhận Hàng, 2: Thanh Toán VN PAY.")]
        public int PayType { get; set; }
        [Required]
        [SwaggerSchema(Description = "1 là Chưa Thanh Toán (tiền mặt), 2 là Đã Thanh Toán (VN Pay)")]
        public bool isPaid { get; set; }
        public string Note { get; set; }
        public List<OrderVoucher> Vouchers { get; set; }
        [Required]
        public List<OrderProduct> Products { get; set; }
        [Required]
        public ReveicerInformation ReveicerInformation { get; set; }
        public VNPayInformation VnpayInformation { get; set; }

        public OrderPickUp OrderPickUp { get; set; }


    }

    public class VNPayInformation
    {
        [Required]
        public string Vnp_TransactionNo { get; set; }
        [Required]
        public string vnp_PayDate { get; set; }
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

        [JsonIgnore]
        public int QuantityAfterConvert { get; set; }
        [JsonIgnore]
        public string ParentId { get; set; }
        [JsonIgnore]
        public ProductDetail productDetail { get; set; }
        [JsonIgnore]
        public List<ProductUnitModel> listUnit { get; set; }

        [JsonIgnore]
        public ProductUnitModel lastUnit { get; set; }
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
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string HomeAddress { get; set; }
    }
}
