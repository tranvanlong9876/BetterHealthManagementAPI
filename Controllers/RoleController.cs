using BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalRole;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.RoleModels;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllRole([FromQuery] RoleFilterRequest filterRequest)
        {
            var listRole = await _roleService.GetRoleList(filterRequest);

            return Ok(listRole);
        }
    }
}
