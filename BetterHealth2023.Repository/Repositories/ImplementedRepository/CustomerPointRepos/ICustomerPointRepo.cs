using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos
{
    public interface ICustomerPointRepo : IRepository<CustomerPoint>
    {
        public Task<int?> GetCustomerPointBasedOnCustomerId(string customerId);
        public Task<int?> GetCustomerPointBasedOnPhoneNumber(string phoneNumber);
    }
}
