using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ProductIngredientModel
    {
        public string IngredientId { get; set; }
        public string IngredientName { get; set; }
        public double Content { get; set; }

        public string UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
