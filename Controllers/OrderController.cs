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
    public class OrderController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok("Có cc");
        }
    }
}
