using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalUser;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IInternalUserAuthService _employeeAuthService;
        public MemberController(ICustomerService customerService, IInternalUserAuthService employeeAuthService)
        {
            _customerService = customerService;
            _employeeAuthService = employeeAuthService;
        }
        [HttpPost("Customer/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginCustomer([FromBody] LoginCustomerModel loginCustomerModel)
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

        [HttpPost("InternalUser/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginInternal(LoginInternalUser loginEmployee)
        {
            try
            {
                var userStatusModel = await _employeeAuthService.Login(loginEmployee);

                if (userStatusModel.isError)
                {
                    if (userStatusModel.UserInactive != null) return BadRequest(userStatusModel);
                    if (userStatusModel.UserNotFound != null) return NotFound(userStatusModel);
                    if (userStatusModel.WrongPassword != null) return Unauthorized(userStatusModel);
                    if (userStatusModel.NoWorkingSite != null) return Unauthorized(userStatusModel);
                }

                if (userStatusModel.userToken == null) return BadRequest("Lỗi tạo token JWT, vui lòng thử lại sau.");

                return Ok(userStatusModel.userToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
