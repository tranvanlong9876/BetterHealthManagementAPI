using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ManufactureService;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ManufacturerController : ControllerBase
    {
        public IManufactureService _manufactureService;

        public ManufacturerController(IManufactureService manufactureService)
        {
            _manufactureService = manufactureService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetManufacturers([FromQuery] ManufacturerPagingRequest pagingRequest)
        {
            var pageResult = await _manufactureService.GetManuFacturers(pagingRequest);
            return Ok(pageResult);
        }

        // GET api/<ManufacturerController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetManufacturer(string id)
        {
            var manuFactModel = await _manufactureService.GetManufacturer(id);
            if (manuFactModel == null) return NotFound("Không tìm thấy Nhà Sản Xuất.");
            return Ok(manuFactModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateManufacturer(CreateNewManufacturer newManufacturer)
        {
            var check = await _manufactureService.CreateManufacturer(newManufacturer);

            return check ? Created("", "Nhà sản xuất mới đã tạo thành công!") : BadRequest("Some thing wrong.");
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateManufacturer(UpdateManufacturer updateManufacturer)
        {
            var check = await _manufactureService.UpdateManufacturer(updateManufacturer);

            return check ? Ok("Cập nhật thông tin nhà sản xuất thành công.") : BadRequest("Something wrong!");
        }
    }
}
