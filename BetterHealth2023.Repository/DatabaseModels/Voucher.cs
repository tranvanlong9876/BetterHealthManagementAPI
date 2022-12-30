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
            OrderHeaders = new HashSet<OrderHeader>();
            VoucherCustomerRestrictions = new HashSet<VoucherCustomerRestriction>();
        }

        [Key]
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
        public bool ApplyForSpecific { get; set; }

        [InverseProperty(nameof(OrderHeader.VoucherCodeNavigation))]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
        [InverseProperty(nameof(VoucherCustomerRestriction.VoucherCodeNavigation))]
        public virtual ICollection<VoucherCustomerRestriction> VoucherCustomerRestrictions { get; set; }
    }
}
