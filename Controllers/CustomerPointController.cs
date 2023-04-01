using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerPointServices;
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
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerPointController : ControllerBase
    {
        private readonly ICustomerPointService _customerPointService;

        public CustomerPointController(ICustomerPointService customerPointService)
        {
            _customerPointService = customerPointService;
        }

        [HttpGet("{phoneNo}/CustomerAvailablePoint")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lấy điểm khả dụng của khách hàng. Trả về kiểu int (điểm khả dụng), mã lỗi 404 nếu khách hàng không tồn tại trong hệ thống.")]
        public async Task<IActionResult> GetCustomerAvailablePoint(string phoneNo)
        {
            return await _customerPointService.GetCustomerAvailablePoint(phoneNo);
        }
    }
}
