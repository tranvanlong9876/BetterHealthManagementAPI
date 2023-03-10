using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductPagingRequest pagingRequest)
        {
            bool isInternal = CheckInternalUser();
            if(!isInternal)
            {
                return Ok(await _productService.GetAllProductsPagingForCustomer(pagingRequest));
            } else
            {
                return Ok(await _productService.GetAllProductsPagingForInternalUser(pagingRequest));
            }
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
            var checkError = await _productService.CreateProduct(createProductModel);
            if (checkError.isError) return BadRequest(checkError);
            return Created("", "Sản phẩm mới đã tạo thành công");
        }

        [HttpPut]
        [Authorize(Roles = Commons.OWNER_NAME)]
        public async Task<IActionResult> UpdateProduct(UpdateProductEntranceModel updateProductModel)
        {
            var checkError = await _productService.UpdateProduct(updateProductModel);
            if (checkError.isError) return BadRequest(checkError);
            return Ok(checkError.productViewModel);
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
    }
}
