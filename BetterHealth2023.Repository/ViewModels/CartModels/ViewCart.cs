using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels
{
    [FirestoreData]
    public class CartItem
    {
        [FirestoreProperty("pid")]
        public string ProductId { get; set; }
        
        [Required]
        [FirestoreProperty("quantity")]
        public int Quantity { get; set; }

        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public double Price { get; set; }
        public double PriceAfterDiscount { get; set; }
        public double PriceTotal { get; set; }

    }

    [FirestoreData]
    public class ViewCart
    {
        public string CartId { get; set; }
        [FirestoreProperty("items")]
        public List<CartItem> Items { get; set; }

        [FirestoreProperty("point")]
        public int? Point { get; set; }

        [FirestoreProperty("deviceids")]
        public List<string> DeviceIds { get; set; }

        [FirestoreProperty("customerid")]
        public string customerId { get; set; }
        public double SubTotalPrice { get; set; }

        public double DiscountPrice { get; set; }
        public double TotalCartPrice { get; set; }
    }
}
