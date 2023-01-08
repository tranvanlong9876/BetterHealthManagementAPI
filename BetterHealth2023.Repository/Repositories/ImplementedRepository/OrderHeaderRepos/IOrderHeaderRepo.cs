using System.Collections.Generic;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos
{
    public interface IOrderHeaderRepo : IRepository<OrderHeader>
    {
        public Task<List<OrderHeader>> GetOrderHeadersBySiteId(string siteId);
    }
}
