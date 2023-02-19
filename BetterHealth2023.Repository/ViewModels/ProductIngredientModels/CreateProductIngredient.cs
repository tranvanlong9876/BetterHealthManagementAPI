using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels
{
    public class CreateProductIngredient
    {
        [Required]
        public string Ingredient_Name { get; set; }
    }
}
