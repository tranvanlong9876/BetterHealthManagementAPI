using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Voucher_Detail")]
    public partial class VoucherDetail
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string VoucherCode { get; set; }
        public double? MinOrderAmount { get; set; }
        public double? MaxOrderAmount { get; set; }
        public int? MinPoint { get; set; }
        public double? DiscountRate { get; set; }
        public double? DiscountMoney { get; set; }
        public double? DiscountMaxMoney { get; set; }

        [ForeignKey(nameof(VoucherCode))]
        [InverseProperty(nameof(Voucher.VoucherDetails))]
        public virtual Voucher VoucherCodeNavigation { get; set; }
    }
}
