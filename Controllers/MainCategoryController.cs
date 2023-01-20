﻿using BetterHealthManagementAPI.BetterHealth2023.Business.Service.MainCategoryService;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
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
    public class MainCategoryController : ControllerBase
    {
        private IMainCategoryService _mainCategoryService;
        public MainCategoryController(IMainCategoryService mainCategoryService)
        {
            _mainCategoryService = mainCategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMainCategory()
        {
            var listMainCategory = await _mainCategoryService.GetAll();

            if (listMainCategory == null) return NotFound();

            return Ok(listMainCategory);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMainCategory(string id)
        {
            var mainCategory = await _mainCategoryService.Get(id);

            if (mainCategory == null) return NotFound();

            return Ok(mainCategory);
        }

        [HttpPost]
        [AllowAnonymous]
        
        public async Task<IActionResult> CreateNewCategory(CreateCategoryModel createCategoryModel)
        {
            var check = await _mainCategoryService.Create(createCategoryModel);

            return check ? Created("", "Đã tạo danh mục cha thành công.") : BadRequest("Lỗi thêm dữ liệu");
        }

        [HttpPut]
        [AllowAnonymous]

        public async Task<IActionResult> UpdateCategory(UpdateCategoryModel updateCategoryModel)
        {
            var check = await _mainCategoryService.Update(updateCategoryModel);

            return check ? Ok("Đã cập nhật danh mục cha thành công.") : BadRequest("Lỗi sửa dữ liệu");
        }
    }
}
