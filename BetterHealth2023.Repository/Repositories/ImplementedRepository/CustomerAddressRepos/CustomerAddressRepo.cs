using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos
{
    public class CustomerAddressRepo : Repository<CustomerAddress>, ICustomerAddressRepo
    {
        public CustomerAddressRepo(BetterHealthManagementContext context) : base(context)
        {
        }
        public async Task<List<CustomerAddress>> GetAllCustomerAddressByCustomerId(string id)
        {
            List<CustomerAddress> list = await context.CustomerAddresses.Where(x => x.CustomerId == id).ToListAsync();
            return list;
            
        public CustomerAddressRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<ActionResult> InsertCustomerAddress(CustomerAddressInsertModel CustomerAddressInsertModel)
        {
            DynamicAddress dynamicAddressnew = new()
            {
                Id = Guid.NewGuid().ToString(),
                CityId = CustomerAddressInsertModel.CityId,
                DistrictId = CustomerAddressInsertModel.DistrictId,
                WardId = CustomerAddressInsertModel.WardId,
                HomeAddress = CustomerAddressInsertModel.HomeAddress,

            };

            //insert customeraddress
            CustomerAddress customerAddress = new()
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = CustomerAddressInsertModel.CustomerId,
                AddressId = dynamicAddressnew.Id,
                MainAddress = true,
            };
            //update database
            context.DynamicAddresses.Add(dynamicAddressnew);
            context.CustomerAddresses.Add(customerAddress);
            await context.SaveChangesAsync();
            //return actionresult
            return new OkObjectResult("Insert success");
        }
    }
}
