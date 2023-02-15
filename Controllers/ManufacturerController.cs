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
        //[HttpGet("{id}")]
        private string Get(int id)
        {
            return "value";
        }

        // POST api/<ManufacturerController>
        //[HttpPost]
        private void Post()
        {
        }

        // PUT api/<ManufacturerController>/5
        //[HttpPut("{id}")]
        private void Put()
        {
        }

        // DELETE api/<ManufacturerController>/5
        //[HttpDelete("{id}")]
        private void Delete(int id)
        {
        }
    }
}
