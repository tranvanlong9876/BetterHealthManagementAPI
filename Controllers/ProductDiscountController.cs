using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductDiscountServices;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using Microsoft.AspNetCore.Authorization;
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
    public class ProductDiscountController : ControllerBase
    {

        private readonly IProductDiscountService _productDiscountService;

        public ProductDiscountController(IProductDiscountService productDiscountService)
        {
            _productDiscountService = productDiscountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProductDiscount(CreateProductDiscountModel discountModel)
        {
            var check = await _productDiscountService.CreateProductDiscount(discountModel);
            if (check.isError)
            {
                return BadRequest(check);
            }
            return Created("", "Tạo giảm giá sản phẩm thành công.");
        }

        [HttpPost("{DiscountId}/Product")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProductToExistingDiscount([FromRoute] string DiscountId, [FromBody] ProductModel product)
        {
            var check = await _productDiscountService.AddProductToExistingDiscount(DiscountId, product);

            if (!check) return BadRequest("Thông tin khuyến mãi không tồn tại hoặc đã kết thúc sự kiện.");
            return Ok("Thêm sản phẩm mới vào thông tin khuyến mãi thành công.");
        }

        [HttpPut]
        [AllowAnonymous]
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
}
