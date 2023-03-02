using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels
{
    public class SendingEmailProductModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double TotalPrice { get; set; }

        public string imageUrl { get; set; }
        public string ProductName { get; set; }
    }
}
