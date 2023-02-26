using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels
{
    public class ViewProductDiscountSpecific
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Reason { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountMoney { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalProduct { get; set; }
        public string Status { get; set; }

        public List<ProductDiscountView> EventProductDiscounts { get; set; }
    }

    public class ProductDiscountView
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }

        public double Price { get; set; }
        public double PriceAfterDiscount { get; set; }
    }
}
