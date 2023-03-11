using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos
{
    public class CustomerAddressRepo : Repository<CustomerAddress>, ICustomerAddressRepo
    {
        public CustomerAddressRepo(BetterHealthManagementContext context) : base(context)
        {
        }

        public CustomerAddressRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> RemoveAllAddressCustomerbyID(string id)
        {
            //remove all customer address by customer id if dynamicaddress change
           
            return false;
        }
    }
}
