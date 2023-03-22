using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_Execution")]
    public partial class OrderExecution
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
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
        public bool IsInternalUser { get; set; }
        [Required]
        [Column("UserID")]
        [StringLength(50)]
        public string UserId { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderExecutions))]
        public virtual OrderHeader Order { get; set; }
        [ForeignKey(nameof(StatusChangeFrom))]
        [InverseProperty(nameof(OrderStatus.OrderExecutionStatusChangeFromNavigations))]
        public virtual OrderStatus StatusChangeFromNavigation { get; set; }
        [ForeignKey(nameof(StatusChangeTo))]
        [InverseProperty(nameof(OrderStatus.OrderExecutionStatusChangeToNavigations))]
        public virtual OrderStatus StatusChangeToNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(InternalUser.OrderExecutions))]
        public virtual InternalUser User { get; set; }
    }
}
