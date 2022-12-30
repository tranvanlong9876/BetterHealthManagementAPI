﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("VoucherCustomerRestriction")]
    public partial class VoucherCustomerRestriction
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string VoucherCode { get; set; }
        [Required]
        [Column("Customer_ID")]
        [StringLength(50)]
        public string CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("VoucherCustomerRestrictions")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(VoucherCode))]
        [InverseProperty(nameof(Voucher.VoucherCustomerRestrictions))]
        public virtual Voucher VoucherCodeNavigation { get; set; }
    }
}
