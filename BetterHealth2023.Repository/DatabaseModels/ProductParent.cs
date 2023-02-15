using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_Parent")]
    public partial class ProductParent
    {
        public ProductParent()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("Product_Description_ID")]
        [StringLength(50)]
        public string ProductDescriptionId { get; set; }
        [Required]
        [Column("Sub_Category_ID")]
        [StringLength(50)]
        public string SubCategoryId { get; set; }
        [Column("Manufacturer_ID")]
        [StringLength(50)]
        public string ManufacturerId { get; set; }
        [Column("isPrescription")]
        public bool IsPrescription { get; set; }
        [Column("loadSellProduct")]
        public int LoadSellProduct { get; set; }
        [Column("isBatches")]
        public bool IsBatches { get; set; }
        [Column("isDelete")]
        public bool IsDelete { get; set; }

        [ForeignKey(nameof(ManufacturerId))]
        [InverseProperty("ProductParents")]
        public virtual Manufacturer Manufacturer { get; set; }
        [ForeignKey(nameof(ProductDescriptionId))]
        [InverseProperty("ProductParents")]
        public virtual ProductDescription ProductDescription { get; set; }
        [ForeignKey(nameof(SubCategoryId))]
        [InverseProperty("ProductParents")]
        public virtual SubCategory SubCategory { get; set; }
        [InverseProperty(nameof(ProductDetail.ProductIdParentNavigation))]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
