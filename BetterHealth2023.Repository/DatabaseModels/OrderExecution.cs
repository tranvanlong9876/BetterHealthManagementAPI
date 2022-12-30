using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("Order_Execution")]
    public partial class OrderExecution
    {
        [Required]
        [Column("OrderID")]
        [StringLength(50)]
        public string OrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string StatusChangeFrom { get; set; }
        [Required]
        [StringLength(50)]
        public string StatusChangeTo { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateOfCreate { get; set; }
        public int UserType { get; set; }
        [Required]
        [Column("UserID")]
        [StringLength(50)]
        public string UserId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual OrderHeader Order { get; set; }
        [ForeignKey(nameof(StatusChangeFrom))]
        public virtual OrderStatus StatusChangeFromNavigation { get; set; }
        [ForeignKey(nameof(StatusChangeTo))]
        public virtual OrderStatus StatusChangeToNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Employee User { get; set; }
    }
}
