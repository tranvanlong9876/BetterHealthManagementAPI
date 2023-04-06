using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerPointModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos
{
    public interface ICustomerPointRepo : IRepository<CustomerPoint>
    {
        public Task<int> GetCustomerPointBasedOnCustomerId(string customerId);
        public Task<int?> GetCustomerPointBasedOnPhoneNumber(string phoneNumber);
        public Task<PagedResult<CustomerPointList>> GetCustomerUsageHistoryPoint(CustomerPointPagingRequest pagingRequest, string customerId);
        
    }
}
