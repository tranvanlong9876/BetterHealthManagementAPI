using BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalUser;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/User")]
    [ApiController]
    [AllowAnonymous]
    public class InternalUserAuthController : ControllerBase
    {
        private IInternalUserAuthService _employeeAuthService;
        public InternalUserAuthController(IInternalUserAuthService employeeAuthService)
        {
            _employeeAuthService = employeeAuthService;
        }

        [HttpGet()]
        [Authorize(Roles = Commons.ADMIN_NAME)]
        public async Task<IActionResult> GetAllUserPaging([FromQuery] GetInternalUserPagingRequest request)
        {
            var userList = await _employeeAuthService.GetAllUserPaging(request);
            if (userList == null) return NotFound();

            return Ok(userList);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Commons.TOTAL_INTERNAL_ROLE_NAME)]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            var userInfo = await _employeeAuthService.GetUserInfoModel(id);
            if (userInfo == null) return NotFound("Không tìm thấy thông tin nhân viên.");

            return Ok(userInfo);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginInternal(LoginInternalUser loginEmployee) {
            try
            {
                var userStatusModel = await _employeeAuthService.Login(loginEmployee);

                if(userStatusModel.isError)
                {
                    if (userStatusModel.UserInactive != null) return BadRequest(userStatusModel);
                    if (userStatusModel.UserNotFound != null) return NotFound(userStatusModel);
                    if (userStatusModel.WrongPassword != null) return Unauthorized(userStatusModel);
                }

                if (userStatusModel.userToken == null) return BadRequest("Lỗi tạo token JWT, vui lòng thử lại sau.");

                return Ok(userStatusModel.userToken);
            } catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("Register"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterNewEmployee(RegisterInternalUser employee) 
        {
            try
            {
                var check = await _employeeAuthService.Register(employee);
                if(check.isError)
                {
                    return BadRequest(check);
                }
                return Created("", "Create new employee successfully.");
            } catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("Update"), Authorize]
        public async Task<IActionResult> UpdateInternalUser(UpdateInternalUser updateInternalUser)
        {
            try
            {
                var check = await _employeeAuthService.UpdateInternalUser(updateInternalUser);
                if(check.isError)
                {
                    return BadRequest(check);
                }
                return NoContent();
            } catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpDelete("Status"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserStatus(UpdateUserStatusEntrance updateUserStatusEntrance)
        {
            try
            {
                var check = await _employeeAuthService.UpdateAccountStatus(updateUserStatusEntrance.UserID, updateUserStatusEntrance.Status);
                if (check.isError)
                {
                    return BadRequest(check);
                }
                return Ok("Đã ngắt hoạt động nhân viên nội bộ.");
            } catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}


