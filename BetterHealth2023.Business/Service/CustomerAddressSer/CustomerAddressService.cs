using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerAddressSer
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly ICustomerAddressRepo _customerAddressRepo;

        public CustomerAddressService(ICustomerAddressRepo customerAddressRepo)
        {
            {
                _customerAddressRepo = customerAddressRepo;
            }
        }

        public async Task<ActionResult> InseartCustomerAddress(CustomerAddressInsertModel CustomerAddressInsertModel)
        {
            var action = await _customerAddressRepo.InsertCustomerAddress(CustomerAddressInsertModel);
            return action;
        }

        public async Task<ActionResult> RemoveCustomerAddressById(string id)
        {
            var customerAddress = await _customerAddressRepo.Get(id);
            if (customerAddress == null)
            {
                //return null
                return new NotFoundObjectResult(new { message = "Customer Address Not Found" });
            }
            else
            {
                _customerAddressRepo.Remove(customerAddress);

                //return action result success
                return new OkObjectResult(new { message = "Customer Address Deleted Successfully" });

            }
        }
    }
}
