using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductDiscountServices;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using Microsoft.AspNetCore.Authorization;
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
    public class ProductDiscountController : ControllerBase
    {

        private readonly IProductDiscountService _productDiscountService;

        public ProductDiscountController(IProductDiscountService productDiscountService)
        {
            _productDiscountService = productDiscountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductDiscount([FromQuery] GetProductDiscountPagingRequest pagingRequest)
        {
            var pagedResult = await _productDiscountService.GetAllProductDiscountPaging(pagingRequest);

            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductDiscount(string id)
        {
            var productDiscount = await _productDiscountService.GetProductDiscount(id);

            if (productDiscount == null) return NotFound("Không tìm thấy Khuyến Mãi theo Id.");

            return Ok(productDiscount);
        }

        [HttpPost("CheckExistDiscountProduct")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Kiểm tra xem sản phẩm đã có chương trình khuyến mãi hay chưa.")]
        public async Task<IActionResult> CheckExistProductDiscountId([FromBody] CheckExistDiscountProduct checkExistDiscountProduct)
        {
            return await _productDiscountService.CheckExistProductDiscount(checkExistDiscountProduct.discountId, checkExistDiscountProduct.productId);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProductDiscount(CreateProductDiscountModel discountModel)
        {
            return await _productDiscountService.CreateProductDiscount(discountModel);
        }

        [HttpPost("{DiscountId}/Product")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Thêm sản phẩm mới vào trong chương trình khuyến mãi hiện có")]
        public async Task<IActionResult> AddProductToExistingDiscount([FromRoute] string DiscountId, [FromBody] ProductModel product)
        {
            return await _productDiscountService.AddProductToExistingDiscount(DiscountId, product);
        }

        [HttpPut]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Thay đổi sản phẩm khác trong chương trình khuyến mãi hiện có")]
        public async Task<IActionResult> UpdateProductDiscount(UpdateProductDiscountModel discountModel)
        {
            var check = await _productDiscountService.UpdateGeneralInformation(discountModel);
            if (check.isError)
            {
                return BadRequest(check);
            }
            return Ok("Cập nhật thông tin khuyến mãi sản phẩm thành công.");
        }

        [HttpDelete("{ProductId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Xóa sản phẩm khỏi chương trình khuyến mãi hiện có")]
        public async Task<IActionResult> RemoveProductFromDiscount(string ProductId)
        {
            var check = await _productDiscountService.RemoveProductFromExistingDiscount(ProductId);

            if (!check) return BadRequest("Thông tin khuyến mãi không tồn tại hoặc đã kết thúc sự kiện.");
            return Ok("Xóa sản phẩm khỏi thông tin khuyến mãi thành công.");
        }
    }

    public class ProductModel
    {
        public string productId { get; set; }
    }

    public class CheckExistDiscountProduct
    {
        public string discountId { get; set; }
        public string productId { get; set; }
    }
}
