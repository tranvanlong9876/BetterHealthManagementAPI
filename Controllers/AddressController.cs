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

        [HttpGet("District/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistricts(string id)
        {
            var districts = await _addressService.GetAllDistricts(id);
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

        [HttpGet("Ward/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWards(string id)
        {
            var wards = await _addressService.GetAllWards(id);
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
    }
}
