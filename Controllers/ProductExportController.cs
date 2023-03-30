using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductExportServices;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductExportController : ControllerBase
    {
        private readonly IProductExportService _productExportService;

        public ProductExportController(IProductExportService productExportService)
        {
            _productExportService = productExportService;
        }

        [HttpGet]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lấy danh sách các sản phẩm bị hư hỏng do quá hạn sử dụng. Role duy nhất: Manager", Description = "Đầu vào: Mã Chi Nhánh, Thông tin cần phân trang")]
        public async Task<IActionResult> GetAllDamagedProduct([FromQuery] GetPagingExportDamageProduct pagingRequest)
        {
            return await _productExportService.GetListProductDamaged(pagingRequest);
        }

        [HttpGet("{siteInventoryId}")]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Xem chi tiết sản phẩm bị hư hỏng do quá hạn sử dụng. Role duy nhất: Manager", Description = "Đầu vào: SiteInventoryId (lấy từ Get All Danh Sách Sản Phẩm Quá Hạn)")]
        public async Task<IActionResult> GetDamagedProduct(string siteInventoryId)
        {
            return await _productExportService.GetSpecificProductDamaged(siteInventoryId);
        }

        [HttpPut]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Api Xuất hỏng sản phẩm. Role duy nhất: Manager", Description = "Đầu vào: SiteInventoryId và số lượng sản phẩm cần xuất hỏng.")]
        public async Task<IActionResult> ExportProduct(ExportDamageProductEntrance exportDamageProduct)
        {
            return await _productExportService.ExportDamageProduct(exportDamageProduct);
        }
    }
}
