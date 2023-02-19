using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels
{
    public class ProductIngredientPagingRequest : PagingRequestBase
    {
        public string Name { get; set; }
    }
}
