using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels
{
    public class CartEntrance
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string Quantity { get; set; }

        [Required]
        public string CityId { get; set; }
        
        public string DistrictId { get; set; }
    }
}
