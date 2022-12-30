﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("OrderStatus")]
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(500)]
        public string OrderStatusName { get; set; }
        public int ApplyForType { get; set; }

        [ForeignKey(nameof(ApplyForType))]
        [InverseProperty(nameof(OrderType.OrderStatuses))]
        public virtual OrderType ApplyForTypeNavigation { get; set; }
        [InverseProperty(nameof(OrderHeader.OrderStatusNavigation))]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
