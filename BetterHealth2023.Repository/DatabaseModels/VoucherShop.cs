using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("VoucherShop")]
    public partial class VoucherShop
    {
        public VoucherShop()
        {
            PurchaseVoucherHistories = new HashSet<PurchaseVoucherHistory>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        public double? DiscountMoney { get; set; }
        public double? DiscountPercent { get; set; }
        public int ApplyFor { get; set; }
        public int ExchancePointFee { get; set; }
        public int Duration { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column("endDate", TypeName = "datetime")]
        public DateTime EndDate { get; set; }

        [InverseProperty(nameof(PurchaseVoucherHistory.VoucherShop))]
        public virtual ICollection<PurchaseVoucherHistory> PurchaseVoucherHistories { get; set; }
    }
}
