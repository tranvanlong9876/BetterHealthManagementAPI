using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels
{
    public class OrderExecutionModel
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string OrderStatusId { get; set; }
        
        public string Description { get; set; }
    }
}
