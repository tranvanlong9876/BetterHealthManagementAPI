using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        // GET: api/<CustomerController>
     

        // GET api/<CustomerController>/5
      

        // POST api/<CustomerController>

        
    
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegisView customerRegisView)
        {
            try
            {
                var customer = await _customerService.CreateCustomer(customerRegisView);
                if (customer == null) return BadRequest();
                return Ok(customer);
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

        //update customer
        [HttpPut("Update")]
        [Authorize(Roles = "Customers")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdateMOdel customerUpdateModel)
        {
            try
            {
                var customer = await _customerService.UpdateCustomer(customerUpdateModel);
                if (customer == false) return BadRequest();
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
       
       
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCustomer()
        {
            try
            {
                List<CustomerUpdateMOdel> customer = await _customerService.GetCustomerPaging();
                if(customer == null)
                {
                    return BadRequest();
                }
                return Ok(customer);
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
        [HttpGet("Paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCustomerPaging(string name, string email, string phoneNo, int pageindex,int pageitem)
        {
            try
            {
                PagedResult<CustomerUpdateMOdel> customer = await _customerService.GetCustomerPaging2(name,email,phoneNo,pageindex,pageitem);
                if (customer == null)
                {
                    return BadRequest();
                }
                return Ok(customer);
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

        [HttpGet("{id}")]
        [AllowAnonymous]
        //find customer by customerid
        public async Task<IActionResult> GetCustomerById(string id)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(id);
                if (customer == null) return NotFound();
                return Ok(customer);
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
