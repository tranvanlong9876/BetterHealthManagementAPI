using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_Voucher")]
    public partial class OrderVoucher
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string VoucherId { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderVouchers))]
        public virtual OrderHeader Order { get; set; }
        [ForeignKey(nameof(VoucherId))]
        [InverseProperty("OrderVouchers")]
        public virtual Voucher Voucher { get; set; }
    }
}
