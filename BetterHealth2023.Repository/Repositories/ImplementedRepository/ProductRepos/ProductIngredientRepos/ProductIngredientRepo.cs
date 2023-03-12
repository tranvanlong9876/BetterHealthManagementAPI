using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Linq.Queryable;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientRepos
{
    public class ProductIngredientRepo : Repository<ProductIngredient>, IProductIngredientRepo
    {
        public ProductIngredientRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<bool> CheckDuplicateProductIngredient(string name)
        {
            var query = await context.ProductIngredients.Where(x => x.IngredientName.Equals(name.Trim())).FirstOrDefaultAsync();

            if (query != null) return true;
            return false;
        }

        public async Task<PagedResult<ViewProductIngredient>> GetProductIngredientPaging(ProductIngredientPagingRequest pagingRequest)
        {
            var query = from ingredient in context.ProductIngredients
                        select ingredient;

            if (!String.IsNullOrWhiteSpace(pagingRequest.Name))
            {
                query = query.Where(x => x.IngredientName.Contains(pagingRequest.Name.Trim()));
            }

            var totalRow = await query.CountAsync();
            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                .Take(pagingRequest.pageItems)
                .Select(selector => new ViewProductIngredient()
                {
                    Id = selector.Id,
                    IngredientName = selector.IngredientName
                }).ToListAsync();

            var pageResult = new PagedResult<ViewProductIngredient>(data, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pageResult;
        }
    }
}
