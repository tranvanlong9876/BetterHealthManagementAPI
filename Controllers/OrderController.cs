using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
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

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok("Test");
        }

        [HttpGet("PickUp/Site")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSiteListPickUp([FromQuery] CartEntrance productEntrance)
        {
            var check = await _orderService.GetViewSiteToPickUps(productEntrance);
            if (check.isError)
            {
                return BadRequest(check);
            }

            return Ok(check.siteListPickUp);
        }
    }
}
