using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
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

        public async Task<PagedResult<CustomerViewListModel>> GetAllCustomerModelViewPaging(CustomerPagingRequest pagingRequest)
        {
            var query = from customer in context.Customers
                        select customer;

            if (!string.IsNullOrEmpty(pagingRequest.NameOrPhone))
            {
                query = query.Where(x => x.Fullname.Contains(pagingRequest.NameOrPhone) || x.PhoneNo.Equals(pagingRequest.NameOrPhone));
            }

            int totalRow = await query.CountAsync();

            var list = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems).Take(pagingRequest.pageItems).Select(selector => new CustomerViewListModel()
            {
                Status = selector.Status,
                PhoneNo = selector.PhoneNo,
                Dob = selector.Dob,
                Email = selector.Email,
                Fullname = selector.Fullname,
                Gender = selector.Gender,
                Id = selector.Id,
                ImageUrl = selector.ImageUrl
            }).ToListAsync();

            var pageResult = new PagedResult<CustomerViewListModel>(list, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pageResult;
        }
        public async Task<string> GetCustomerIdBasedOnPhoneNo(string phoneNo)
        {
            return await context.Customers.Where(x => x.PhoneNo.Equals(phoneNo)).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
