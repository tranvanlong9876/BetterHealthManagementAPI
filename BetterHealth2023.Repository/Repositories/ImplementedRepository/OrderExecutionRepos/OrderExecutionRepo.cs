using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderExecutionRepos
{
    public class OrderExecutionRepo : Repository<OrderExecution>, IOrderExecutionRepo
    {
        public OrderExecutionRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<StatusExecution> GetStatusOrderExecution(string statusId)
        {
            return await context.OrderStatuses.Where(x => x.Id.Equals(statusId)).Select(selector => new StatusExecution()
            {
                StatusId = statusId,
                StatusName = selector.OrderStatusName
            }).FirstOrDefaultAsync();
        }

        public async Task<UserExecution> GetUserOrderExedution(string userId, bool isInternal)
        {
            if (isInternal) return await context.InternalUsers.Where(x => x.Id.Equals(userId)).Select(selector => new UserExecution()
            {
                UserId = userId,
                UserName = selector.Fullname
            }).FirstOrDefaultAsync();

            return await context.Customers.Where(x => x.Id.Equals(userId)).Select(selector => new UserExecution()
            {
                UserId = userId,
                UserName = selector.Fullname
            }).FirstOrDefaultAsync();
        }

        public async Task<List<ViewOrderHistoryFromDB>> ViewOrderHistory(string orderId)
        {
            var query = from oe in context.OrderExecutions
                        orderby oe.DateOfCreate descending
                        select oe;

            return await query.Where(x => x.OrderId.Equals(orderId)).Select(selector => new ViewOrderHistoryFromDB()
            {
                StatusId = selector.StatusChangeTo,
                Description = selector.Description,
                UserId = selector.UserId,
                Time = selector.DateOfCreate,
                IsInternal = selector.IsInternalUser
            }).ToListAsync();
        }
    }
}
