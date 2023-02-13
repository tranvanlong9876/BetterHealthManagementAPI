using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientDescriptionRepos
{
    public interface IProductIngredientDescriptionRepo : IRepository<ProductIngredientDescription>
    {
        public Task<List<ProductIngredientModel>> GetProductIngredient(string productDescId);

        public Task<List<UpdateProductIngredientModel>> GetProductIngredientUpdate(string productDescId);

        public Task<bool> RemoveAllProductIngredients(string descriptionID);
        public Task<bool> AddMultipleProductIngredients(List<ProductIngredientDescription> productIngredientDescriptions);
    }
}
