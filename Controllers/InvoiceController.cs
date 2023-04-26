using BetterHealthManagementAPI.BetterHealth2023.Business.Service.InvoiceServices;
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
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{OrderId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Xuất hóa đơn PDF, chỉ dành cho đơn tại chỗ.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Trả về đường link file PDF.")]

        public async Task<IActionResult> GenerateInvoice(string OrderId)
        {
            return await _invoiceService.PrintInvoicePdf(OrderId);
        }
    }
}
