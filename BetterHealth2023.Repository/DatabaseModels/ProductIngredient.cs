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
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Ingredient_Name")]
        [StringLength(100)]
        public string IngredientName { get; set; }
    }
}
