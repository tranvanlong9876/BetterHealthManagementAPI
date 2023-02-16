using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos
{
    public class CustomerRepo : Repository<Customer>, ICustomerRepo
    {
        public CustomerRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public Task<CustomerAddress> GetAddressCustomer(string id)
        {
            //get customeraddress by customerid
            return context.CustomerAddresses.Where(x => x.CustomerId.Trim().Equals(id.Trim())).FirstOrDefaultAsync();
        }

        public async Task<Customer> getCustomerBasedOnEmail(string Email)
        {
            return await context.Customers.Where(x => x.Email.Trim().Equals(Email.Trim())).FirstOrDefaultAsync();
        }

        public async Task<Customer> getCustomerBasedOnPhoneNo(string phoneNo)
        {
            return await context.Customers.Where(x => x.PhoneNo.Trim().Equals(phoneNo.Trim())).FirstOrDefaultAsync();
        }

      
    }
}
