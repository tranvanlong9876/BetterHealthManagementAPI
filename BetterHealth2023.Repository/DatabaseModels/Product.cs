using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("Drug_Description_ID")]
        [StringLength(50)]
        public string DrugDescriptionId { get; set; }
        [Required]
        [Column("Sub_Category_ID")]
        [StringLength(50)]
        public string SubCategoryId { get; set; }
        [Column("Manufacturer_ID")]
        [StringLength(50)]
        public string ManufacturerId { get; set; }
        [Required]
        [StringLength(100)]
        public string Origin { get; set; }
        [Column("isPrescription")]
        public bool? IsPrescription { get; set; }
        [Column("isBatches")]
        public bool? IsBatches { get; set; }

        [ForeignKey(nameof(DrugDescriptionId))]
        [InverseProperty(nameof(ProductDescription.Products))]
        public virtual ProductDescription DrugDescription { get; set; }
        [ForeignKey(nameof(ManufacturerId))]
        [InverseProperty("Products")]
        public virtual Manufacturer Manufacturer { get; set; }
        [ForeignKey(nameof(SubCategoryId))]
        [InverseProperty("Products")]
        public virtual SubCategory SubCategory { get; set; }
        [InverseProperty(nameof(ProductDetail.ProductIdParentNavigation))]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
