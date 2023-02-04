using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductPagingRequest pagingRequest)
        {
            var listProduct = await _productService.GetAllProduct(pagingRequest);
            return Ok(listProduct);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductModel createProductModel)
        {
            var checkError = await _productService.CreateProduct(createProductModel);
            if (checkError.isError) return BadRequest(checkError);
            return Created("", "Sản phẩm mới đã tạo thành công");
        }
    }
}
