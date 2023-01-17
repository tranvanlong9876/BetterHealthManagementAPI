using BetterHealthManagementAPI.BetterHealth2023.Business.Service.MainCategoryService;
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

            return Ok(listMainCategory);
        }
    }
}
