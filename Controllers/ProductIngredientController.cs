using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductIngredientService;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductIngredientModels;
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
    public class ProductIngredientController : ControllerBase
    {
        private readonly IProductIngredientService _productIngredientService;

        public ProductIngredientController(IProductIngredientService productIngredientService)
        {
            _productIngredientService = productIngredientService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductIngredients([FromQuery] ProductIngredientPagingRequest pagingRequest)
        {
            var pageResult = await _productIngredientService.GetProductIngredientPaging(pagingRequest);

            return Ok(pageResult);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductIngredient(string id)
        {
            var productIngredient = await _productIngredientService.GetProductIngredient(id);

            if (productIngredient == null) return NotFound("Không tìm thấy thành phần sản phẩm.");

            return Ok(productIngredient);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProductIngredient(CreateProductIngredient createProductIngredient)
        {
            var productIngredient = await _productIngredientService.CreateProductIngredient(createProductIngredient);

            if (productIngredient == null) return BadRequest("Trùng tên thành phần sản phẩm đã có hoặc insert lỗi.");

            return Created("", productIngredient);
        }
    }
}
