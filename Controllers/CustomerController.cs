using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CustomerController>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginCustomer([FromBody] LoginCustomerModel loginCustomerModel)
        {
            try
            {
                var loginStatusModel = await _customerService.customerLoginPhoneOTP(loginCustomerModel);
                if (loginStatusModel.isError)
                {
                    if (loginStatusModel.InvalidPhoneOTP != null) return BadRequest(loginStatusModel);
                    if (loginStatusModel.CustomerNotFound != null) return NotFound();
                    if (loginStatusModel.CustomerInactive != null) return BadRequest(loginStatusModel);
                    else return BadRequest(loginStatusModel);
                }
                return Ok(loginStatusModel.customerToken);
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
    }
}
