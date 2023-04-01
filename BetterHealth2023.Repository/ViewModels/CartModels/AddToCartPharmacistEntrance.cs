using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels
{
    public class AddToCartPharmacistEntrance
    {
        [Required]
        public string CartId { get; set; }
        [Required]
        [MinLength(1)]
        public List<ItemInCart> items { get; set; }
    }

    public class ItemInCart
    {
        [Required]
        public string productId { get; set; }
        [Required]
        public int quantity { get; set; }
    }
}
