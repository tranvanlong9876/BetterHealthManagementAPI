using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_Details")]
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            Comments = new HashSet<Comment>();
            EventProductDiscounts = new HashSet<EventProductDiscount>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductImages = new HashSet<ProductImage>();
            ProductImportDetails = new HashSet<ProductImportDetail>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Product_ID_Parent")]
        [StringLength(50)]
        public string ProductIdParent { get; set; }
        [Required]
        [Column("Unit_ID")]
        [StringLength(50)]
        public string UnitId { get; set; }
        [Column("Unit_Level")]
        public int UnitLevel { get; set; }
        public int Quantitative { get; set; }
        [Column("sellQuantity")]
        public int SellQuantity { get; set; }
        public double Price { get; set; }
        [Column("isSell")]
        public bool IsSell { get; set; }

        [ForeignKey(nameof(ProductIdParent))]
        [InverseProperty(nameof(ProductParent.ProductDetails))]
        public virtual ProductParent ProductIdParentNavigation { get; set; }
        [ForeignKey(nameof(UnitId))]
        [InverseProperty("ProductDetails")]
        public virtual Unit Unit { get; set; }
        [InverseProperty(nameof(Comment.Product))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(EventProductDiscount.Product))]
        public virtual ICollection<EventProductDiscount> EventProductDiscounts { get; set; }
        [InverseProperty(nameof(OrderDetail.Product))]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [InverseProperty(nameof(ProductImage.Product))]
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        [InverseProperty(nameof(ProductImportDetail.Product))]
        public virtual ICollection<ProductImportDetail> ProductImportDetails { get; set; }
    }
}
