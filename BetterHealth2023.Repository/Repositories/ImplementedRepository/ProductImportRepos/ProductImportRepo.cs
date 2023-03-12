using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos
{
    public class ProductImportRepo : Repository<ProductImportReceipt>, IProductImportRepo
    {
        public ProductImportRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> checkProductManageByBatches(string productID)
        {
            var query = from detail in context.ProductDetails.Where(x => x.Id.Equals(productID))
                        from parent in context.ProductParents.Where(x => x.Id == detail.ProductIdParent)
                        select parent;

            var data = await query.Select(x => x.IsBatches).FirstOrDefaultAsync();

            return data;
        }

        public async Task<PagedResult<ViewListProductImportModel>> ViewListProductImportPaging(GetProductImportPagingRequest pagingRequest)
        {
            var query = from import in context.ProductImportReceipts
                        from manager in context.InternalUsers.Where(x => x.Id == import.ManagerId).DefaultIfEmpty()
                        orderby import.ImportDate descending
                        select new { import, manager };

            if (!String.IsNullOrWhiteSpace(pagingRequest.ManagerID))
            {
                query = query.Where(x => x.import.ManagerId.Equals(pagingRequest.ManagerID));
            }

            if (!String.IsNullOrWhiteSpace(pagingRequest.SiteID))
            {
                query = query.Where(x => x.import.SiteId.Equals(pagingRequest.SiteID));
            }

            if (pagingRequest.isRelease.HasValue)
            {
                query = query.Where(x => x.import.IsReleased.Equals(pagingRequest.isRelease));
            }

            var totalRow = await query.CountAsync();

            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems).Take(pagingRequest.pageItems)
                .Select(selector => new ViewListProductImportModel()
                {
                    Id = selector.import.Id,
                    ImportDate = selector.import.ImportDate,
                    IsReleased = selector.import.IsReleased,
                    ManagerId = selector.import.ManagerId,
                    SiteId = selector.import.SiteId,
                    ManagerName = selector.manager.Fullname,
                    TotalPrice = selector.import.TotalPrice
                }).ToListAsync();

            return new PagedResult<ViewListProductImportModel>(data, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);
        }
    }
}
