using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductImportService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
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
    public class ProductImportController : ControllerBase
    {
        private readonly IProductImportService _productImportService;

        public ProductImportController(IProductImportService productImportService)
        {
            _productImportService = productImportService;
        }

        [HttpGet]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        public async Task<IActionResult> ViewListProductImport([FromQuery] GetProductImportPagingRequest pagingRequest)
        {
            var token = GetUserToken();
            var siteId = JwtUserToken.GetWorkingSiteFromManagerAndPharmacist(token);
            var managerId = JwtUserToken.GetUserID(token);
            pagingRequest.SiteID = siteId;
            pagingRequest.ManagerID = managerId;
            var pagedResult = await _productImportService.ViewListProductImportPaging(pagingRequest);

            return Ok(pagedResult);
        }
        [SwaggerOperation(Summary = "Lấy ra tin nhắn message từ ProductId và số lượng sản phẩm nhập kho.")]
        [HttpGet("Message")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductImportMessage([FromQuery] ProductImportMessageEntrance messageEntrance)
        {
            return await _productImportService.GetProductCalculateTemplate(messageEntrance);
        }

        [HttpPost]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        public async Task<IActionResult> CreateProductImport(CreateProductImportModel importModel)
        {
            var siteID = JwtUserToken.GetWorkingSiteFromManagerAndPharmacist(GetUserToken());
            var managerID = JwtUserToken.GetUserID(GetUserToken());

            if (String.IsNullOrWhiteSpace(siteID)) return BadRequest("Không tìm thấy chi nhánh dựa trên token");
            if (String.IsNullOrWhiteSpace(managerID)) return BadRequest("Không tìm thấy mã nhân viên dựa trên token");

            importModel.SiteId = siteID;
            importModel.ManagerId = managerID;

            var statusModel = await _productImportService.CreateProductImport(importModel);

            if(statusModel.isError)
            {
                return BadRequest(statusModel);
            }

            return Created("", "Thêm đơn nhập hàng thành công.");
        }

        [HttpPut]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        public async Task<IActionResult> UpdateProductImport(UpdateProductImportModel updateProductImportModel)
        {
            var siteID = JwtUserToken.GetWorkingSiteFromManagerAndPharmacist(GetUserToken());
            var managerID = JwtUserToken.GetUserID(GetUserToken());

            if (String.IsNullOrWhiteSpace(siteID)) return BadRequest("Không tìm thấy chi nhánh dựa trên token");
            if (String.IsNullOrWhiteSpace(managerID)) return BadRequest("Không tìm thấy mã nhân viên dựa trên token");

            updateProductImportModel.SiteId = siteID;
            updateProductImportModel.ManagerId = managerID;

            var statusModel = await _productImportService.UpdateProductImport(updateProductImportModel);
            if (statusModel.isError)
            {
                if (statusModel.NotFound != null) return NotFound(statusModel);
                if (statusModel.NotFoundBatches != null) return NotFound(statusModel);

                return BadRequest(statusModel);
            }

            return Ok("Đơn nhập hàng đã được cập nhật xong.");
        }

        [HttpPut("Release")]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        public async Task<IActionResult> ReleaseProductImport(ReleaseProductImport releaseProductImport)
        {
            var siteID = JwtUserToken.GetWorkingSiteFromManagerAndPharmacist(GetUserToken());
            var managerID = JwtUserToken.GetUserID(GetUserToken());

            if (String.IsNullOrWhiteSpace(siteID)) return BadRequest("Không tìm thấy chi nhánh dựa trên token");
            if (String.IsNullOrWhiteSpace(managerID)) return BadRequest("Không tìm thấy mã nhân viên dựa trên token");

            var check = await _productImportService.ReleaseProductImportController(releaseProductImport.Id, siteID);
            if (!check) return BadRequest("Đơn nhập hàng đã được duyệt sẵn trước đây, không thể thao tác nữa.");
            return Ok("Release đơn nhập hàng thành công");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Commons.MANAGER_NAME)]
        public async Task<IActionResult> ViewProductImport(string id)
        {
            var productImportModel = await _productImportService.ViewSpecificProductImport(id);

            if (productImportModel == null) return NotFound("Không tìm thấy thông tin đơn nhập hàng.");

            return Ok(productImportModel);
        }

        
        private string GetUserToken()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                return (Request.Headers)["Authorization"].ToString().Split(" ")[1];
            }
            else
            {
                return null;
            }
        }
    }
}
