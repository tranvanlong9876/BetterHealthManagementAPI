using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos
{
    public interface ICustomerAddressRepo : IRepository<CustomerAddress>
    {
        public Task<List<CustomerAddress>> GetAllCustomerAddressByCustomerId(string id);
        public Task<ActionResult> InsertCustomerAddress(CustomerAddressInsertModel CustomerAddressInsertModel);
    }
}
