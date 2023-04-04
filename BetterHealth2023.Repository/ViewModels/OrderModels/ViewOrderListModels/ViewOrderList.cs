using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels
{
    public class ViewOrderList
    {
        public string Id { get; set; }
        public string PharmacistId { get; set; }
        public int OrderTypeId { get; set; }
        public string OrderTypeName { get; set; }
        public string SiteId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusName { get; set; }
        public double TotalPrice { get; set; }
        public int UsedPoint { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool NeedAcceptance { get; set; }
    }
}
