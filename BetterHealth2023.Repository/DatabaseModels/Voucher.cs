using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Voucher")]
    public partial class Voucher
    {
        public Voucher()
        {
            OrderVouchers = new HashSet<OrderVoucher>();
            VoucherCustomerRestrictions = new HashSet<VoucherCustomerRestriction>();
            VoucherDetails = new HashSet<VoucherDetail>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string VoucherCode { get; set; }
        [Required]
        public string VoucherDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpireDate { get; set; }
        public int Quantity { get; set; }
        public int UsedQuantity { get; set; }
        public bool Activate { get; set; }
        public bool ShowVoucherBar { get; set; }
        public bool ApplyForAll { get; set; }

        [InverseProperty(nameof(OrderVoucher.Voucher))]
        public virtual ICollection<OrderVoucher> OrderVouchers { get; set; }
        [InverseProperty(nameof(VoucherCustomerRestriction.Voucher))]
        public virtual ICollection<VoucherCustomerRestriction> VoucherCustomerRestrictions { get; set; }
        [InverseProperty(nameof(VoucherDetail.Voucher))]
        public virtual ICollection<VoucherDetail> VoucherDetails { get; set; }
    }
}
