using System;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels;
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
                siteviewmodel.DynamicAddModel.Id = Guid.NewGuid().ToString();
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
        public async Task<IActionResult> UpdateSite([FromBody]UpdateSiteModel UpdateSiteModels)
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

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetAllSite([FromBody] string id)
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

        //update IsActive siteinformation
        [HttpPut("Active")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> UpdateSiteActive([FromBody] UpdateStatusSiteModel siteU)
        {
            try
            {
                var site = await _siteService.UpdateSiteIsActive(siteU.SiteID, siteU.Status);
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


        [HttpPut("Delivery")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> UpdateSiteIsDelivery([FromBody] UpdateStatusSiteModel site)
      
        {
            //update active site
            try
            {
                var check = await _siteService.UpdateSiteIsDelivery(site.SiteID, site.Status);
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
