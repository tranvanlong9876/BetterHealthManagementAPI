using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_PickUp")]
    public partial class OrderPickUp
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string DatePickUp { get; set; }
        [Required]
        [StringLength(50)]
        public string TimePickUp { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderPickUps))]
        public virtual OrderHeader Order { get; set; }
    }
}
