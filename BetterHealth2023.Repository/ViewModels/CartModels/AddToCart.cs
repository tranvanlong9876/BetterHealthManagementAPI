using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels
{
    [FirestoreData]
    public class AddToCart
    {
        [FirestoreProperty("pid")]
        [Required]
        public string ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm trong giỏ hàng phải luôn luôn lớn hơn 0")]
        [FirestoreProperty("quantity")]
        public int Quantity { get; set; }
    }

    [FirestoreData]
    public class Cart
    {
        [Required]
        
        public string cartId { get; set; }

        [Required]
        public AddToCart Item { get; set; }
        [FirestoreProperty("items")]
        [JsonIgnore]
        public List<AddToCart> Items { get; set; }

        [FirestoreProperty("last-update")]
        [JsonIgnore]
        public Timestamp LastUpdated { get; set; }
    }
}
