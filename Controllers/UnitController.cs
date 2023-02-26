using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Unit;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnits([FromQuery] GetUnitPagingModel pagingModel)
        {
            var unitLists = await _unitService.GetAll(pagingModel);

            return Ok(unitLists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnit(string id)
        {
            var unit = await _unitService.Get(id);
            if (unit == null) return NotFound();
            return Ok(unit);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUnit(CreateUnitModel unitModel)
        {
            var model = await _unitService.Insert(unitModel.UnitName);
            if (model.isError) return BadRequest(model);
            return Created("", "Đơn vị tính mới đã được tạo thành công.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUnit(UpdateUnitModel updateUnitModel)
        {
            var model = await _unitService.Update(updateUnitModel.Id, updateUnitModel.UnitName);
            if (model.isError) return BadRequest(model);
            return Created("", "Đơn vị tính mới đã được tạo thành công.");
        }
    }
}
