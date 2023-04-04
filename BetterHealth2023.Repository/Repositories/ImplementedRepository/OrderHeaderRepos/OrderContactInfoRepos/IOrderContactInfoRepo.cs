using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderContactInfoRepos
{
    public interface IOrderContactInfoRepo : IRepository<OrderContactInfo>
    {
        public Task<OrderContactInfo> GetCustomerInfoBasedOnOrderId(string orderId);
    }
}
