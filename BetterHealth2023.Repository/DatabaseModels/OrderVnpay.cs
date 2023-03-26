using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_VNPay")]
    public partial class OrderVnpay
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderId { get; set; }
        [Required]
        [Column("VNP_PayDate")]
        [StringLength(50)]
        public string VnpPayDate { get; set; }
        [Required]
        [Column("VNP_TransactionNo")]
        [StringLength(50)]
        public string VnpTransactionNo { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderVnpays))]
        public virtual OrderHeader Order { get; set; }
    }
}
