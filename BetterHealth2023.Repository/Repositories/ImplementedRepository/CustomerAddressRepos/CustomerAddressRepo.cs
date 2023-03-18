using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using static System.Linq.Queryable;

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
        public async Task<List<CustomerAddressView>> GetAllCustomerAddressByCustomerId(string id)
        {
            var query = from customerAddress in context.CustomerAddresses
                        from dynamicAddress in context.DynamicAddresses.Where(x => x.Id == customerAddress.AddressId)
                        select new { customerAddress, dynamicAddress };

            query = query.Where(x => x.customerAddress.CustomerId.Equals(id));

            return await query.Select(selector => new CustomerAddressView()
            {
                Id = selector.customerAddress.Id,
                AddressId = selector.customerAddress.AddressId,
                CityId = selector.dynamicAddress.CityId,
                DistrictId = selector.dynamicAddress.DistrictId,
                WardId = selector.dynamicAddress.WardId,
                HomeAddress = selector.dynamicAddress.HomeAddress,
                IsMainAddress = selector.customerAddress.MainAddress
            }).ToListAsync();
        }

        public async Task<int> GetTotalCurrentCustomerAddress(string customerId)
        {
            return await context.CustomerAddresses.Where(x => x.CustomerId.Equals(customerId) && x.MainAddress).CountAsync();
        }

        public async Task<ActionResult> InsertCustomerAddress(CustomerAddressInsertModel CustomerAddressInsertModel)
        {
            
            await context.SaveChangesAsync();
            //return actionresult
            return new OkObjectResult("Insert success");
        }
    }
}
