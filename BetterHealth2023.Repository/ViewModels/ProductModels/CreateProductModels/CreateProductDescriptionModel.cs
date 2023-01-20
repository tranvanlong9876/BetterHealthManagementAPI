using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels
{
    public class CreateProductDescriptionModel
    {
        public string Effect { get; set; }
        public string Instruction { get; set; }
        public string SideEffect { get; set; }
        public string Contraindications { get; set; }
        public string Preserve { get; set; }

        public List<CreateProductIngredientModel> ingredientModel { get; set; }
    }
}
