using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
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

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductModel createProductModel)
        {
            var check = await _productService.CreateProduct(createProductModel);
            if (!check) return BadRequest("Lỗi tạo sản phẩm");
            return Created("", "Sản phẩm mới đã tạo thành công");
        }
    }
}
