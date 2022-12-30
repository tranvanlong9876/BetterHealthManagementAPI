using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("Customer_Address")]
    public partial class CustomerAddress
    {
        [Required]
        [Column("Customer_ID")]
        [StringLength(50)]
        public string CustomerId { get; set; }
        [Required]
        [Column("Address_ID")]
        [StringLength(50)]
        public string AddressId { get; set; }
        public bool MainAddress { get; set; }

        [ForeignKey(nameof(AddressId))]
        public virtual DynamicAddress Address { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
    }
}
