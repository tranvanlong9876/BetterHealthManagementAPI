using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductIngredientService
{
    public interface IProductIngredientService
    {
        public Task<PagedResult<ViewProductIngredient>> GetProductIngredientPaging(ProductIngredientPagingRequest pagingRequest);

        public Task<ViewProductIngredient> GetProductIngredient(string id);
        public Task<ViewProductIngredient> CreateProductIngredient(CreateProductIngredient createProductIngredient);
    }
}
