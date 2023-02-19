using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CountryServices;
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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCountries([FromQuery] string name)
        {
            return Ok(await _countryService.GetAllCountries(name));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountry(string id)
        {
            var country = await _countryService.GetCountry(id);
            return country != null ? Ok(await _countryService.GetCountry(id)) : NotFound("Không tìm thấy Đất Nước theo ID.");
        }
    }
}
