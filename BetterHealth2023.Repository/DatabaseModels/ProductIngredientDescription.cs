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
        [Column("Drug_Description_ID")]
        [StringLength(50)]
        public string DrugDescriptionId { get; set; }
        [Required]
        [Column("Ingredient_ID")]
        [StringLength(50)]
        public string IngredientId { get; set; }
        public int? Content { get; set; }

        [ForeignKey(nameof(DrugDescriptionId))]
        [InverseProperty(nameof(ProductDescription.ProductIngredientDescriptions))]
        public virtual ProductDescription DrugDescription { get; set; }
        [ForeignKey(nameof(IngredientId))]
        [InverseProperty(nameof(ProductIngredient.ProductIngredientDescriptions))]
        public virtual ProductIngredient Ingredient { get; set; }
    }
}
