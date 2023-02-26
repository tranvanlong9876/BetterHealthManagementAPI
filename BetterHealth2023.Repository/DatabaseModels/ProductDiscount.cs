using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_Discount")]
    public partial class ProductDiscount
    {
        public ProductDiscount()
        {
            EventProductDiscounts = new HashSet<EventProductDiscount>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Reason { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountMoney { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public bool IsDelete { get; set; }

        [InverseProperty(nameof(EventProductDiscount.Discount))]
        public virtual ICollection<EventProductDiscount> EventProductDiscounts { get; set; }
    }
}
