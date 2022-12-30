using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Employee;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeAuthController : ControllerBase
    {
        private IEmployeeAuthService _employeeAuthService;
        private readonly IConfiguration _configuration;

        public EmployeeAuthController(IEmployeeAuthService employeeAuthService)
        {
            _employeeAuthService = employeeAuthService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginInternal(LoginEmployee loginEmployee) {
            try
            {
                var employeeTokenModel = await _employeeAuthService.Login(loginEmployee);
                if (employeeTokenModel == null) return BadRequest("Lỗi tạo token JWT, vui lòng thử lại sau.");

                return Ok(employeeTokenModel);
            } catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut, Authorize(Roles = "Manager")]
        public async Task<IActionResult> RegisterNewEmployee(RegisterEmployee employee) 
        {
            try
            {
                var check = await _employeeAuthService.Register(employee);
                if(check.isError)
                {
                    return BadRequest(check);
                }
                return Created("", "Create new employee successfully.");
            } catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}


