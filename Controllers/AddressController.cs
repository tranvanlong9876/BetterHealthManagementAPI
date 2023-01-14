using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Address;
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
    public class AddressController : ControllerBase
    {
        private IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAddressById(string id)
        {
            var addressModel = await _addressService.GetAddressById(id);
            try
            {
                if (addressModel != null)
                {
                    return Ok(addressModel);
                }
                else
                {
                    return NotFound("Không tìm thấy thông tin Địa Chỉ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("City")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _addressService.GetAllCitys();
            try
            {
                if (cities != null)
                {
                    return Ok(cities);
                } else
                {
                    return NotFound("Lỗi lấy dữ liệu");
                }
            } catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("City/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecificCity(string id)
        {
            var city = await _addressService.GetSpecificCity(id);
            try
            {
                if (city != null)
                {
                    return Ok(city);
                }
                else
                {
                    return NotFound("Không tìm thấy thành phố.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{CityID}/District")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistricts(string CityID)
        {
            var districts = await _addressService.GetAllDistricts(CityID);
            try
            {
                if (districts != null)
                {
                    return Ok(districts);
                }
                else
                {
                    return NotFound("Lỗi lấy dữ liệu");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("District/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecificDistrict(string id)
        {
            var districts = await _addressService.GetSpecificDistrict(id);
            try
            {
                if (districts != null)
                {
                    return Ok(districts);
                }
                else
                {
                    return NotFound("Không tìm thấy Quận/Huyện dựa trên ID cung cấp.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{DistrictID}/Ward")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWards(string DistrictID)
        {
            var wards = await _addressService.GetAllWards(DistrictID);
            try
            {
                if (wards != null)
                {
                    return Ok(wards);
                }
                else
                {
                    return NotFound("Lỗi lấy dữ liệu");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("Ward/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecificWard(string id)
        {
            var ward = await _addressService.GetSpecificWard(id);
            try
            {
                if (ward != null)
                {
                    return Ok(ward);
                }
                else
                {
                    return NotFound("Không tìm thấy phường.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
