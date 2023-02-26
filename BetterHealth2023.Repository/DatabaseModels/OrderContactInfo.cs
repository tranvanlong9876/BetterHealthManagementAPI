using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_ContactInfo")]
    public partial class OrderContactInfo
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderId { get; set; }
        [StringLength(50)]
        public string CustomerId { get; set; }
        [StringLength(50)]
        public string AddressId { get; set; }
        [Required]
        [StringLength(50)]
        public string Fullname { get; set; }
        [StringLength(50)]
        public string PhoneNo { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public bool Gender { get; set; }

        [ForeignKey(nameof(AddressId))]
        [InverseProperty(nameof(DynamicAddress.OrderContactInfos))]
        public virtual DynamicAddress Address { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("OrderContactInfos")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderContactInfos))]
        public virtual OrderHeader Order { get; set; }
    }
}
