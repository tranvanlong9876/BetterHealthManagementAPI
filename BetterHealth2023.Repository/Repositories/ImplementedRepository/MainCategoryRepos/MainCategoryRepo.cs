using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static System.Linq.Queryable;
using static System.Linq.Enumerable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.MainCategoryRepos
{
    public class MainCategoryRepo : Repository<CategoryMain>, IMainCategoryRepo
    {
        public MainCategoryRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<PagedResult<MainCategoryViewModel>> GetAllPaging(MainCategoryPagingRequest pagingRequest)
        {
            var query = (from cm in context.CategoryMains
                         join sc in context.SubCategories on cm.Id equals sc.MainCategoryId into subCategories
                         from subCategory in subCategories.DefaultIfEmpty()
                         join pp in context.ProductParents on subCategory.Id equals pp.SubCategoryId into products
                         from product in products.DefaultIfEmpty()
                         group product by new { cm.Id, cm.CategoryName, cm.ImageUrl } into g
                         select new
                         {
                             Id = g.Key.Id,
                             CategoryName = g.Key.CategoryName,
                             ImageURL = g.Key.ImageUrl,
                             NoOfProduct = g.Count(pp => pp != null)
                         });

            if (!string.IsNullOrEmpty(pagingRequest.Name))
            {
                query = query.Where(x => x.CategoryName.Contains(pagingRequest.Name.Trim()));
            }

            int totalRows = await query.CountAsync();

            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                                  .Take(pagingRequest.pageItems)
                                  .Select(selector => new MainCategoryViewModel()
                                  {
                                      Id = selector.Id,
                                      ImageUrl = selector.ImageURL,
                                      CategoryName = selector.CategoryName,
                                      NoOfProducts = selector.NoOfProduct
                                  }).ToListAsync();

            var pagedResult = new PagedResult<MainCategoryViewModel>(data, totalRows, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pagedResult;
        }

        public Task<bool> UpdateStatus(string id, bool status)
        {
            throw new NotImplementedException();
        }
    }
}
