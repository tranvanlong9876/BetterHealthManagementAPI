using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderStatusService;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderStatusModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lấy ra các trạng thái đơn hàng phụ thuộc vào loại đơn hàng (OrderTypeId). 1 là Tại Chỗ, 2 là Đến lấy, 3 là Giao hàng.")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderStatusByOrderType([FromQuery] OrderStatusFilterRequest orderStatusFilterRequest)
        {
            return await _orderStatusService.GetOrderStatusBasedOnOrderType(orderStatusFilterRequest);
        }
    }
}
