using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderExecutionRepos
{
    public interface IOrderExecutionRepo : IRepository<OrderExecution>
    {
        public Task<List<ViewOrderHistoryFromDB>> ViewOrderHistory(string orderId);

        public Task<UserExecution> GetUserOrderExedution(string userId, bool isInternal);
        public Task<StatusExecution> GetStatusOrderExecution(string statusId);
    }
}
