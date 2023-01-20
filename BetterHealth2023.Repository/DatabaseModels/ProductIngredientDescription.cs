using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_Ingredient_Description")]
    public partial class ProductIngredientDescription
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Product_Description_ID")]
        [StringLength(50)]
        public string ProductDescriptionId { get; set; }
        [Required]
        [Column("Ingredient_ID")]
        [StringLength(50)]
        public string IngredientId { get; set; }
        public double? Content { get; set; }
        [Column("Unit_ID")]
        [StringLength(50)]
        public string UnitId { get; set; }

        [ForeignKey(nameof(IngredientId))]
        [InverseProperty(nameof(ProductIngredient.ProductIngredientDescriptions))]
        public virtual ProductIngredient Ingredient { get; set; }
        [ForeignKey(nameof(ProductDescriptionId))]
        [InverseProperty("ProductIngredientDescriptions")]
        public virtual ProductDescription ProductDescription { get; set; }
        [ForeignKey(nameof(UnitId))]
        [InverseProperty("ProductIngredientDescriptions")]
        public virtual Unit Unit { get; set; }
    }
}
