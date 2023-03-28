using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderValidateModels
{
    public class ValidateOrderModel
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public bool IsAccept { get; set; }

        public string Description { get; set; }

        [Required]
        public string IpAddress { get; set; }
    }
}
