using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Customer_Address")]
    public partial class CustomerAddress
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
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
        [InverseProperty(nameof(DynamicAddress.CustomerAddresses))]
        public virtual DynamicAddress Address { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("CustomerAddresses")]
        public virtual Customer Customer { get; set; }
    }
}
