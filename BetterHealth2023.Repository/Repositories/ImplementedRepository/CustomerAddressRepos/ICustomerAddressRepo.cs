using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos
{
    public interface ICustomerAddressRepo : IRepository<CustomerAddress>
    {
        public Task<bool> RemoveAllAddressCustomerbyID(string id);
    }
}
