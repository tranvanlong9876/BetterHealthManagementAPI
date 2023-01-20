using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SubCategoryRepos
{
    public class SubCategoryRepo : Repository<SubCategory>, ISubCategoryRepo
    {
        public SubCategoryRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<PagedResult<SubCategoryViewModel>> GetAllSubCategory(GetSubCategoryPagingRequest pagingRequest)
        {
            var query = from sub_cate in context.SubCategories
                        from main_cate in context.CategoryMains.Where(main => main.Id == sub_cate.MainCategoryId).DefaultIfEmpty()
                        select new { sub_cate, main_cate };

            if (!string.IsNullOrEmpty(pagingRequest.MainCategoryID))
            {
                query = query.Where(x => x.sub_cate.MainCategoryId.Equals(pagingRequest.MainCategoryID.Trim()));
            }

            int totalRows = await query.CountAsync();

            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                                  .Take(pagingRequest.pageItems)
                                  .Select(selector => new SubCategoryViewModel()
            {
                Id = selector.sub_cate.Id,
                ImageUrl = selector.sub_cate.ImageUrl,
                MainCategoryId = selector.sub_cate.MainCategoryId,
                MainCategoryName = selector.main_cate.CategoryName,
                SubCategoryName = selector.sub_cate.SubCategoryName
            }).ToListAsync();

            var pagedResult = new PagedResult<SubCategoryViewModel>(data, totalRows, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pagedResult;

        }
    }
}
