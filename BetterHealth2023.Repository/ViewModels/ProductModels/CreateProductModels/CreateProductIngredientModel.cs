using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels
{
    public class CreateProductIngredientModel
    {
        [StringLength(50)]
        public string IngredientId { get; set; }
        public double? Content { get; set; }

        [StringLength(50)]
        public string UnitId { get; set; }
    }
}
