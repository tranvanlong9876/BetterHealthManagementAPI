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
            ProductDetails = new HashSet<ProductDetail>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        public double DiscountPercent { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        public string Reason { get; set; }

        [InverseProperty(nameof(ProductDetail.Discount))]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
