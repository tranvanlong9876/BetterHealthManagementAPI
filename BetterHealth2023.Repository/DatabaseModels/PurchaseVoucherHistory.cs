using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("PurchaseVoucherHistory")]
    public partial class PurchaseVoucherHistory
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("VoucherShop_ID")]
        [StringLength(50)]
        public string VoucherShopId { get; set; }
        [Required]
        [Column("Customer_ID")]
        [StringLength(50)]
        public string CustomerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PurchasedDate { get; set; }
        public int UsedPoint { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("PurchaseVoucherHistories")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(VoucherShopId))]
        [InverseProperty("PurchaseVoucherHistories")]
        public virtual VoucherShop VoucherShop { get; set; }
    }
}
