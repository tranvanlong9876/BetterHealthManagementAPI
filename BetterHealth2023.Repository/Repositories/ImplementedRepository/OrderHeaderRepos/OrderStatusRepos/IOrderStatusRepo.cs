using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderStatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderStatusRepos
{
    public interface IOrderStatusRepo : IRepository<OrderStatus>
    {
        public Task<List<ViewOrderStatus>> GetAllOrderStatus(OrderStatusFilterRequest orderStatusFilterRequest);
    }
}
