using BetterHealthManagementAPI.BetterHealth2023.Business.Service.VNPay;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels;
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
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService _VNPayService;

        public VNPayController(IVNPayService vNPayService)
        {
            _VNPayService = vNPayService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateVNPayRedirectURL([FromQuery] VNPayInformationModel payInformationModel)
        {
            var url = _VNPayService.CreatePaymentUrl(payInformationModel);
            if(url == null) return BadRequest("Lỗi lấy URL");

            return Ok(url);
        }

        [HttpGet("Query")]
        [AllowAnonymous]
        public async Task<IActionResult> QueryExistingOrderId([FromQuery] QueryVNPayModel queryVNPayModel)
        {
            return await _VNPayService.QueryExistingPaymentAsync(queryVNPayModel);
        }
    }
}
