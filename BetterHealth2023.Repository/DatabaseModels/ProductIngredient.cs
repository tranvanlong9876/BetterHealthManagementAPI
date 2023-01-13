using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_Ingredient")]
    public partial class ProductIngredient
    {
        public ProductIngredient()
        {
            ProductIngredientDescriptions = new HashSet<ProductIngredientDescription>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Ingredient_Name")]
        [StringLength(100)]
        public string IngredientName { get; set; }

        [InverseProperty(nameof(ProductIngredientDescription.Ingredient))]
        public virtual ICollection<ProductIngredientDescription> ProductIngredientDescriptions { get; set; }
    }
}
