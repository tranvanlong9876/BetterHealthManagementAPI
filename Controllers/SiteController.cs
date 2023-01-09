using System;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site;
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
    public class SiteController : ControllerBase
    {
        private ISiteService _siteService;
        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
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
        public async Task<IActionResult> UpdateSite(string SiteID, SiteViewModels stSiteViewModels)
        {
           
            try
            {
                var check = await _siteService.UpdateSite(SiteID, stSiteViewModels);
                if (check)
                {
                    return Ok("Update site successfully");
                }
                return BadRequest("Update site failed");
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

      

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSitebyID(string SiteID)
        {
            try
            {
                var site = await _siteService.GetSite(SiteID);
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

        //update IsActive siteinformation
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSiteActive(string SiteID, bool IsActive)
        {
            //update active site
            try
            {
                var check = await _siteService.UpdateSiteIsActive(SiteID, IsActive);
                if (check)
                {
                    return Ok("Update site successfully");
                }
                return BadRequest("Update site failed");
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSiteIsDelivery(string SiteID, bool IsDelivery)
        {
            //update active site
            try
            {
                var check = await _siteService.UpdateSiteIsDelivery(SiteID, IsDelivery);
                if (check)
                {
                    return Ok("Update site successfully");
                }
                return BadRequest("Update site failed");
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
