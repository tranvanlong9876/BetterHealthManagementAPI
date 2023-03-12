using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels
{
    [FirestoreData]
    public class CartItem
    {
        [FirestoreProperty("pid")]
        public string ProductId { get; set; }
        [FirestoreProperty("image")]

        public string ProductImageUrl { get; set; }
        [Required]
        [FirestoreProperty("name")]
        public string ProductName { get; set; }
        [Required]
        [FirestoreProperty("quantity")]
        public int Quantity { get; set; }
    }

    [FirestoreData]
    public class ViewCart
    {
        [FirestoreProperty("items")]
        public List<CartItem> Items { get; set; }
    }
}
