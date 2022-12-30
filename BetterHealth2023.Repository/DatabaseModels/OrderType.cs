using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_Type")]
    public partial class OrderType
    {
        public OrderType()
        {
            OrderHeaders = new HashSet<OrderHeader>();
            OrderStatuses = new HashSet<OrderStatus>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column("OrderType_Name")]
        [StringLength(50)]
        public string OrderTypeName { get; set; }

        [InverseProperty(nameof(OrderHeader.OrderType))]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
        [InverseProperty(nameof(OrderStatus.ApplyForTypeNavigation))]
        public virtual ICollection<OrderStatus> OrderStatuses { get; set; }
    }
}
