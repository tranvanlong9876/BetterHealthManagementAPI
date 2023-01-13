using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_Image")]
    public partial class ProductImage
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("ImageURL")]
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [Required]
        [Column("ProductID")]
        [StringLength(50)]
        public string ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.ProductImages))]
        public virtual ProductDetail Product { get; set; }
    }
}
