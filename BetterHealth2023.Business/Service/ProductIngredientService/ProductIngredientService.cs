using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductIngredientService
{
    public class ProductIngredientService : IProductIngredientService
    {
        private readonly IProductIngredientRepo _productIngredientRepo;

        public ProductIngredientService(IProductIngredientRepo productIngredientRepo)
        {
            _productIngredientRepo = productIngredientRepo;
        }

        public async Task<ViewProductIngredient> CreateProductIngredient(CreateProductIngredient createProductIngredient)
        {
            if (await _productIngredientRepo.CheckDuplicateProductIngredient(createProductIngredient.Ingredient_Name.Trim())) return null;

            string id = Guid.NewGuid().ToString();
            var productIngredientDB = new ProductIngredient();
            productIngredientDB.Id = id;
            productIngredientDB.IngredientName = createProductIngredient.Ingredient_Name;
            await _productIngredientRepo.Insert(productIngredientDB);
            return await GetProductIngredient(id);
        }

        public async Task<ViewProductIngredient> GetProductIngredient(string id)
        {
            return await _productIngredientRepo.GetViewModel<ViewProductIngredient>(id);
        }

        public async Task<PagedResult<ViewProductIngredient>> GetProductIngredientPaging(ProductIngredientPagingRequest pagingRequest)
        {
            return await _productIngredientRepo.GetProductIngredientPaging(pagingRequest);
        }
    }
}
