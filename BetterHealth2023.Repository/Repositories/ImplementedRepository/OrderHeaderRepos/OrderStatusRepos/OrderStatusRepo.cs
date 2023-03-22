using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderStatusModels;
using System;
using System.Collections.Generic;
using static System.Linq.Queryable;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderStatusRepos
{
    public class OrderStatusRepo : Repository<OrderStatus>, IOrderStatusRepo
    {
        public OrderStatusRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ViewOrderStatus>> GetAllOrderStatus(OrderStatusFilterRequest orderStatusFilterRequest)
        {
            return await context.OrderStatuses.Where(x => x.ApplyForType.Equals(orderStatusFilterRequest.OrderTypeId)).Select(selector => new ViewOrderStatus()
            {
                OrderStatusId = selector.Id,
                OrderStatusName = selector.OrderStatusName
            }).ToListAsync();
        }
    }
}
