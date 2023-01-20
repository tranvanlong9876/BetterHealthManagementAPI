using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Event_ProductDiscount")]
    public partial class EventProductDiscount
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Product_ID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        [Required]
        [Column("Discount_ID")]
        [StringLength(50)]
        public string DiscountId { get; set; }

        [ForeignKey(nameof(DiscountId))]
        [InverseProperty(nameof(ProductDiscount.EventProductDiscounts))]
        public virtual ProductDiscount Discount { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.EventProductDiscounts))]
        public virtual ProductDetail Product { get; set; }
    }
}
