using BetterHealthManagementAPI.BetterHealth2023.Business.Service.VNPay;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderVNPayRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels;
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
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService _VNPayService;
        private readonly IOrderVNPayRepo _orderVNPayRepo;

        public VNPayController(IVNPayService vNPayService, IOrderVNPayRepo orderVNPayRepo)
        {
            _VNPayService = vNPayService;
            _orderVNPayRepo = orderVNPayRepo;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Tạo ra đường link để redirect khách hàng đến trang Thanh Toán VN Pay")]
        public IActionResult CreateVNPayRedirectURL([FromQuery] VNPayInformationModel payInformationModel)
        {
            var url = _VNPayService.CreatePaymentUrl(payInformationModel);
            if(string.IsNullOrEmpty(url)) return BadRequest("Lỗi lấy URL");

            return Ok(url);
        }

        [HttpGet("Query")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "API tham chiếu dữ liệu thanh toán VN Pay cho các đơn hàng VN Pay.", Description = "Dữ liệu đầu vào: Mã Đơn Hàng và Địa Chỉ Ip người truy vấn.")]
        public async Task<IActionResult> QueryExistingOrderId([FromQuery] QueryVNPayModel queryVNPayModel)
        {
            var vnpayInformation = await _orderVNPayRepo.GetTransaction(queryVNPayModel.OrderId);

            if (vnpayInformation == null) return NotFound("Không tìm thấy thông tin thanh toán VN Pay dựa trên mã đơn hàng.");

            queryVNPayModel.TransactionDate = vnpayInformation.VnpPayDate;

            return await _VNPayService.QueryExistingPaymentAsync(queryVNPayModel);
        }
    }
}
