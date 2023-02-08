using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var loginStatusModel = await _customerService.customerLoginPhoneOTP(loginCustomerModel);
            if(loginStatusModel.isError)
            {
                if (loginStatusModel.InvalidPhoneOTP != null) return BadRequest(loginStatusModel);
                if (loginStatusModel.CustomerNotFound != null) return NotFound();
                if (loginStatusModel.CustomerInactive != null) return BadRequest(loginStatusModel);
                else return BadRequest(loginStatusModel);
            }
            return Ok(loginStatusModel.customerToken);
        }

        
    
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegisView customerRegisView)
        {
            var customer = await _customerService.CreateCustomer(customerRegisView);
            if (customer == null) return BadRequest();
            return Ok(customer);
        }
    }
}
