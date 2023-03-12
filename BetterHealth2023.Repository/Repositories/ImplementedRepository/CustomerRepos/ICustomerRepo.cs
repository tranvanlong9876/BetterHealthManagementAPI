using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos
{
    public interface ICustomerRepo : IRepository<Customer>
    {
        public Task<string> GetCustomerIdBasedOnPhoneNo(string phoneNo);
        public Task<Customer> getCustomerBasedOnPhoneNo(string phoneNo);
        public Task<Customer> getCustomerBasedOnEmail(string Email);
        public Task<CustomerAddress> GetAddressCustomer(string id);
        
    }
}
