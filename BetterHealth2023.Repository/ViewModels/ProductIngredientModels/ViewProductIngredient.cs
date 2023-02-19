using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels
{
    public class ViewProductIngredient
    {
        public string Id { get; set; }
        public string IngredientName { get; set; }
    }
}
