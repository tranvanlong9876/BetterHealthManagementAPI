using System;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Address;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SiteController : ControllerBase
    {
        private ISiteService _siteService;
        private IAddressService _addressService;
        public SiteController(ISiteService siteService, IAddressService addressService)
        {
            _siteService = siteService;
            _addressService = addressService;
        }

        [HttpGet()]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetAllSites()
        {
            try
            {
                var site = await _siteService.GetListSite();
                if (site == null)
                {
                    return BadRequest("Site is empty");
                }
                return Ok(site);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetSite(string id)
        {
            try
            {
                var site = await _siteService.GetSite(id);
                if (site == null)
                {
                    return BadRequest("SiteID not found");
                }
                return Ok(site);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertSite(SiteViewModels siteviewmodel)
        {
            try
            {
                var site = await _siteService.InsertSite(siteviewmodel);

                if (site == null)
                {
                    return BadRequest("AddressID not found");
                }
               
                return Created("", site);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpPut]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UpdateSite(UpdateSiteModel UpdateSiteModels)
        {

            try
            {
                var site = await _siteService.GetSite(UpdateSiteModels.SiteID);
                if (site == null)
                {
                    return BadRequest("SiteID not found");
                }

                var result = await _siteService.UpdateSite(UpdateSiteModels);
                return Ok(site);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        //update IsActive siteinformation
        [HttpPut("Active")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> UpdateSiteActive(UpdateStatusSiteModel siteU)
        {
            try
            {
                var updateSiteStatus = await _siteService.UpdateSiteIsActive(siteU.SiteID, siteU.Status);
                if (updateSiteStatus.isError)
                {
                    if (updateSiteStatus.SiteNotFound != null)
                    {
                        return NotFound("Không tìm thấy chi nhánh");
                    } else
                    {
                        return BadRequest(updateSiteStatus);
                    }

                }
                return Ok("Update Site's Operation Status Successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        [HttpPut("Delivery")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> UpdateSiteIsDelivery(UpdateStatusSiteModel site)
      
        {
            //update active site
            try
            {
                var updateSiteStatus = await _siteService.UpdateSiteIsDelivery(site.SiteID, site.Status);
                if (updateSiteStatus.isError)
                {
                    if (updateSiteStatus.SiteNotFound != null)
                    {
                        return NotFound("Không tìm thấy chi nhánh");
                    }
                    else
                    {
                        return BadRequest(updateSiteStatus);
                    }

                }
                return Ok("Update Site's Delivery Status Successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


    }
}
