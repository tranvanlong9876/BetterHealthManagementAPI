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
            ShelvesManagements = new HashSet<ShelvesManagement>();
            WarehouseManagements = new HashSet<WarehouseManagement>();
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
        public double? Quantitative { get; set; }
        [Required]
        [Column("ImageURL")]
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [Column("sellQuantity")]
        public int SellQuantity { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Column("isSell")]
        public bool IsSell { get; set; }
        [Column("Discount_ID")]
        [StringLength(50)]
        public string DiscountId { get; set; }

        [ForeignKey(nameof(DiscountId))]
        [InverseProperty(nameof(ProductDiscount.ProductDetails))]
        public virtual ProductDiscount Discount { get; set; }
        [ForeignKey(nameof(ProductIdParent))]
        [InverseProperty(nameof(Product.ProductDetails))]
        public virtual Product ProductIdParentNavigation { get; set; }
        [ForeignKey(nameof(UnitId))]
        [InverseProperty("ProductDetails")]
        public virtual Unit Unit { get; set; }
        [InverseProperty(nameof(Comment.Product))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(ShelvesManagement.Product))]
        public virtual ICollection<ShelvesManagement> ShelvesManagements { get; set; }
        [InverseProperty(nameof(WarehouseManagement.Product))]
        public virtual ICollection<WarehouseManagement> WarehouseManagements { get; set; }
    }
}
