using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerPointModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos
{
    public class CustomerPointRepo : Repository<CustomerPoint>, ICustomerPointRepo
    {
        public CustomerPointRepo(BetterHealthManagementContext context) : base(context)
        {

        }

        public CustomerPointRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<int> GetCustomerPointBasedOnCustomerId(string customerId)
        {
            int? plusPoint = await context.CustomerPoints.Where(x => x.CustomerId.Equals(customerId) && x.IsPlus).SumAsync(x => x.Point);
            int? minusPoint = await context.CustomerPoints.Where(x => x.CustomerId.Equals(customerId) && !x.IsPlus).SumAsync(x => x.Point);

            if(!plusPoint.HasValue && !minusPoint.HasValue)
            {
                return 0;
            }

            plusPoint = !plusPoint.HasValue ? 0 : plusPoint;
            minusPoint = !minusPoint.HasValue ? 0 : minusPoint;

            return plusPoint.Value - minusPoint.Value;
        }

        public async Task<int?> GetCustomerPointBasedOnPhoneNumber(string phoneNumber)
        {
            var customerId = await context.Customers.Where(x => x.PhoneNo.Equals(phoneNumber)).Select(x => x.Id).SingleOrDefaultAsync();

            if (customerId == null) return null;

            return await GetCustomerPointBasedOnCustomerId(customerId);
        }

        public async Task<PagedResult<CustomerPointList>> GetCustomerUsageHistoryPoint(CustomerPointPagingRequest pagingRequest, string customerId)
        {
            var query = from point in context.CustomerPoints.Where(x => x.CustomerId == customerId)
                        select point;

            if (pagingRequest.sortDateBySoonest)
            {
                query = query.OrderByDescending(x => x.CreateDate);
            } else
            {
                query = query.OrderBy(x => x.CreateDate);
            }

            if (pagingRequest.FilterIsPlus.HasValue)
            {
                query = query.Where(x => x.IsPlus == pagingRequest.FilterIsPlus.Value);
            }

            var totalRow = await query.CountAsync();

            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems).Take(pagingRequest.pageItems).Select(selector => new CustomerPointList()
            {
                Id = selector.Id,
                Description = selector.Description,
                Point = selector.Point
            }).ToListAsync();

            return new PagedResult<CustomerPointList>(data, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);
        }
    }
}
