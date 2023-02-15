using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientDescriptionRepos
{
    public class ProductIngredientDescriptionRepo : Repository<ProductIngredientDescription>, IProductIngredientDescriptionRepo
    {
        public ProductIngredientDescriptionRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> AddMultipleProductIngredients(List<ProductIngredientDescription> productIngredientDescriptions)
        {
            await context.AddRangeAsync(productIngredientDescriptions);
            await Update();
            return true;
        }

        public async Task<List<ProductIngredientModel>> GetProductIngredient(string productDescId)
        {
            var query = from pro_ingre_desc in context.ProductIngredientDescriptions.Where(desc => desc.ProductDescriptionId.Equals(productDescId))
                        from ingredient in context.ProductIngredients.Where(ingre => ingre.Id == pro_ingre_desc.IngredientId).DefaultIfEmpty()
                        from unit in context.Units.Where(units => units.Id == pro_ingre_desc.UnitId).DefaultIfEmpty()
                        select new { pro_ingre_desc, ingredient, unit };

            var data = await query.Select(selector => new ProductIngredientModel()
            {
                IngredientId = selector.pro_ingre_desc.IngredientId,
                IngredientName = selector.ingredient.IngredientName,
                Content = selector.pro_ingre_desc.Content == null ? null : (double) selector.pro_ingre_desc.Content,
                UnitId = selector.pro_ingre_desc.UnitId,
                UnitName = selector.unit.UnitName
            }).ToListAsync();

            return data;
        }

        public async Task<List<UpdateProductIngredientModel>> GetProductIngredientUpdate(string productDescId)
        {
            var query = from pro_ingre_desc in context.ProductIngredientDescriptions.Where(desc => desc.ProductDescriptionId.Equals(productDescId))
                        from ingredient in context.ProductIngredients.Where(ingre => ingre.Id == pro_ingre_desc.IngredientId).DefaultIfEmpty()
                        from unit in context.Units.Where(units => units.Id == pro_ingre_desc.UnitId).DefaultIfEmpty()
                        select new { pro_ingre_desc, ingredient, unit };

            var data = await query.Select(selector => new UpdateProductIngredientModel()
            {
                Id = selector.pro_ingre_desc.Id,
                IngredientId = selector.pro_ingre_desc.IngredientId,
                Content = (double)selector.pro_ingre_desc.Content,
                UnitId = selector.pro_ingre_desc.UnitId
            }).ToListAsync();

            return data;
        }

        public async Task<bool> RemoveAllProductIngredients(string descriptionID)
        {
            var productDescriptions = await context.ProductIngredientDescriptions.Where(x => x.ProductDescriptionId.Equals(descriptionID.Trim())).ToListAsync();

            context.RemoveRange(productDescriptions);

            await Update();

            return true;
        }
    }
}
