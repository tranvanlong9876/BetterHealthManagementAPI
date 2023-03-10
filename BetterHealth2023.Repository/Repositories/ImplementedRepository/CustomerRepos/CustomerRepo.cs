using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

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


        public async Task<List<Customer>> GetAllCustomerModelView()
        {
            //getall customer
            List<Customer> list = await context.Customers.ToListAsync();
            return list;
        }

        public async Task<PagedResult<Customer>> GetAllCustomerModelViewPaging(string name, int pageindex, int pageitem)
        {
            var query = from customer in context.Customers
                        select customer;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Fullname.Contains(name) || x.Fullname.Contains(name));
            }
            int totalRow = await query.CountAsync();
            var list = await query.Skip((pageindex - 1) * pageitem).Take(pageitem).ToListAsync();
            var pageResult = new PagedResult<Customer>(list, totalRow, pageindex, pageitem);

            return pageResult;
        }
        public async Task<string> GetCustomerIdBasedOnPhoneNo(string phoneNo)
        {
            return await context.Customers.Where(x => x.PhoneNo.Equals(phoneNo)).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
