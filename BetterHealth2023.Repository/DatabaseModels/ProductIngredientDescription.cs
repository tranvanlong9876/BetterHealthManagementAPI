using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("Product_Ingredient_Description")]
    public partial class ProductIngredientDescription
    {
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
        public virtual ProductDescription DrugDescription { get; set; }
        [ForeignKey(nameof(IngredientId))]
        public virtual ProductIngredient Ingredient { get; set; }
    }
}
