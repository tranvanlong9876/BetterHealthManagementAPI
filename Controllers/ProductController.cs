using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("UserTarget")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "API hỗ trợ load DropDown List cho đối tượng sản phẩm")]
        public async Task<IActionResult> GetAllProductUserTarget()
        {
            return await _productService.GetAllProductUserTarget();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductPagingRequest pagingRequest)
        {
            bool isInternal = CheckInternalUser();
            if (!isInternal)
            {
                return Ok(await _productService.GetAllProductsPagingForCustomer(pagingRequest));
            }
            else
            {
                return Ok(await _productService.GetAllProductsPagingForInternalUser(pagingRequest, GetWholeToken()));
            }
        }

        [HttpGet("HomePage")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "API load sản phẩm riêng trên trang Home, tạo tối ưu cho khách hàng.")]
        public async Task<IActionResult> GetAllProductForHomePage([FromQuery] ProductPagingHomePageRequest pagingRequest)
        {
            return await _productService.GetAllProductsPagingForHomePage(pagingRequest);
        }

        [HttpGet("View/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetViewProducts(string id)
        {
            bool isInternal = CheckInternalUser();
            var productView = await _productService.GetViewProduct(id, isInternal);
            if (productView == null) return NotFound("Không tìm thấy sản phẩm hoặc sản phẩm không được bày bán.");
            return Ok(productView);
        }

        [HttpGet("Update/{id}")]
        [Authorize(Roles = Commons.OWNER_NAME)]
        public async Task<IActionResult> GetViewUpdateProducts(string id)
        {
            var productView = await _productService.GetViewProductForUpdate(id);
            if (productView == null) return NotFound("Không tìm thấy sản phẩm hoặc đã bị xóa.");
            return Ok(productView);
        }

        [HttpPost]
        [Authorize(Roles = Commons.OWNER_NAME)]
        public async Task<IActionResult> CreateProduct(CreateProductModel createProductModel)
        {
            try
            {
                var checkError = await _productService.CreateProduct(createProductModel);
                if (checkError.isError) return BadRequest(checkError);
                return Created("", "Sản phẩm mới đã tạo thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = Commons.OWNER_NAME)]
        public async Task<IActionResult> UpdateProduct(UpdateProductEntranceModel updateProductModel)
        {
            try
            {
                var checkError = await _productService.UpdateProduct(updateProductModel);
                if (checkError.isError) return BadRequest(checkError);
                return Ok(checkError.productViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool CheckInternalUser()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                if (JwtUserToken.DecodeAPITokenToRole(token) == String.Empty || JwtUserToken.DecodeAPITokenToRole(token).Equals("Customer"))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private string GetWholeToken()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}
