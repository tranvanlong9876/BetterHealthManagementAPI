using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels
{
    public class RemoveItemFromCart
    {
        [Required]
        public string productId { get; set; }
        public string cartId { get; set; }
    }
}
