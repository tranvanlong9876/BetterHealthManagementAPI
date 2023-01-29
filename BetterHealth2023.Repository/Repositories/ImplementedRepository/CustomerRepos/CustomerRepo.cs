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

        public async Task<Customer> getCustomerBasedOnPhoneNo(string phoneNo)
        {
            return await context.Customers.Where(x => x.PhoneNo.Trim().Equals(phoneNo.Trim())).FirstOrDefaultAsync();
        }
    }
}
