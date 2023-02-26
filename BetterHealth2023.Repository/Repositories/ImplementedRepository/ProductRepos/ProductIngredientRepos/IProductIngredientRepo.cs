using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientRepos
{
    public interface IProductIngredientRepo : IRepository<ProductIngredient>
    {
        public Task<PagedResult<ViewProductIngredient>> GetProductIngredientPaging(ProductIngredientPagingRequest pagingRequest);
        public Task<bool> CheckDuplicateProductIngredient(string name);
    }
}
