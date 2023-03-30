using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerAddressSer;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressService _customerAddressService;

        public CustomerAddressController(ICustomerAddressService customerAddressService)
        {
            _customerAddressService = customerAddressService;
        }


        //delele customer address by id
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Xóa địa chỉ của khách hàng, lưu ý truyền vào mã Id của CustomerAddressId, không phải AddressId hay CustomerId")]
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
        [Authorize(Roles = Commons.CUSTOMER_NAME)]
        [SwaggerOperation(Summary = "Cập nhật địa chỉ của khách hàng, lưu ý truyền vào mã Id của CustomerAddressId, và dữ liệu mới của địa chỉ.")]

        public async Task<IActionResult> UpdateCustomerAddress([FromBody] AddressUpdateModel addressUpdateModel)
        {
            try
            {
                return await _customerAddressService.UpdateCustomerAddress(addressUpdateModel);
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
        [Authorize(Roles = Commons.CUSTOMER_NAME)]
        [SwaggerOperation(Summary = "Thêm địa chỉ cho khách hàng")]

        public async Task<IActionResult> InsertCustomerAddress([FromBody] CustomerAddressInsertModel CustomerAddressInsertModel)
        {
            try
            {
                return await _customerAddressService.InsertCustomerAddress(CustomerAddressInsertModel);
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
