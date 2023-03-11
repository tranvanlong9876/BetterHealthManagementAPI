using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos
{
    public class ProductDiscountRepo : Repository<ProductDiscount>, IProductDiscountRepo
    {
        public ProductDiscountRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<PagedResult<ViewProductDiscountList>> GetAllProductDiscountPaging(GetProductDiscountPagingRequest pagingRequest)
        {
            var query = from discount in context.ProductDiscounts
                        orderby discount.CreatedDate descending
                        select discount;

            var totalRow = await query.CountAsync();

            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                                  .Take(pagingRequest.pageItems)
                                  .Select(selector => new ViewProductDiscountList()
                                  {
                                      Id = selector.Id,
                                      StartDate = selector.StartDate,
                                      EndDate = selector.EndDate,
                                      DiscountMoney = selector.DiscountMoney,
                                      DiscountPercent = selector.DiscountPercent,
                                      Title = selector.Title
                                  }).ToListAsync();

            return new PagedResult<ViewProductDiscountList>(data, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);
        }
    }
}
