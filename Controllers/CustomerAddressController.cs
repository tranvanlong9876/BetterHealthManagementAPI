using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerAddressSer;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressService _customerAddressService;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;
        public CustomerAddressController(ICustomerAddressService customerAddressService,IDynamicAddressRepo dynamicAddressRepo)
        {
            _customerAddressService = customerAddressService;
            _dynamicAddressRepo = dynamicAddressRepo;
        }

        //delele customer address by id
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerAddressById(string id)
        {
            try { 
                var result = await _customerAddressService.RemoveCustomerAddressById(id);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Customers")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCustomerAddres([FromBody] AddressUpdateModel addressUpdateModel)
        {
            try
            {
                var customeraddress = await _dynamicAddressRepo.CheckAddressChangeById(addressUpdateModel);
                if (customeraddress == false) return BadRequest();
                return Ok("Update Success");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customers")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertCustomerAddres([FromBody] CustomerAddressInsertModel CustomerAddressInsertModel)
        {
            try
            {
                var customeraddress = await _customerAddressService.InseartCustomerAddress(CustomerAddressInsertModel);
                if (customeraddress == null) return BadRequest();
                return Ok("Update Success");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }
    }
}
