using BetterHealthManagementAPI.BetterHealth2023.Business.Service.SubCategoryService;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels;
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
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubCategory([FromQuery] GetSubCategoryPagingRequest pagingRequest)
        {
            var subCateList = await _subCategoryService.GetAll(pagingRequest);
            if (subCateList == null) return NotFound();

            return Ok(subCateList);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubCategory(string id)
        {
            var subCate = await _subCategoryService.Get(id);
            if (subCate == null) return NotFound();

            return Ok(subCate);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewSubCategory(CreateSubCategoryModel createSubCategoryModel)
        {
            var check = await _subCategoryService.Create(createSubCategoryModel);

            return check ? Created("", "Đã tạo thêm danh mục thành công.") : BadRequest("Lỗi thêm dữ liệu");
        }

        [HttpPut]
        [AllowAnonymous]

        public async Task<IActionResult> UpdateCategory(UpdateSubCategoryModel updateSubCategoryModel)
        {
            var check = await _subCategoryService.Update(updateSubCategoryModel);

            return check ? Ok("Đã cập nhật danh mục thành công.") : BadRequest("Lỗi sửa dữ liệu");
        }
    }
}
