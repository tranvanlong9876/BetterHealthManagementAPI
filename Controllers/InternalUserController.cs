using BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalUser;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/User")]
    [ApiController]
    [Authorize]
    public class InternalUserController : ControllerBase
    {
        private IInternalUserAuthService _employeeAuthService;
        public InternalUserController(IInternalUserAuthService employeeAuthService)
        {
            _employeeAuthService = employeeAuthService;
        }

        [HttpGet]
        [Authorize(Roles = Commons.ADMIN_NAME)]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Xem danh sách nhân viên, chỉ Admin được xem.")]

        public async Task<IActionResult> GetAllUserPaging([FromQuery] GetInternalUserPagingRequest request)
        {
            var userList = await _employeeAuthService.GetAllUserPaging(request);
            if (userList == null) return NotFound();

            return Ok(userList);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Commons.TOTAL_INTERNAL_ROLE_NAME)]
        [SwaggerOperation(Summary = "Xem thông tin cá nhân trong nội bộ. Admin có thể xem toàn bộ, role khác chỉ được xem chính mình. Customer không dùng API này.")]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            var userInfo = await _employeeAuthService.GetUserInfoModel(id);
            if (userInfo == null) return NotFound("Không tìm thấy thông tin nhân viên.");

            return Ok(userInfo);
        }

        [HttpPost("Register"), Authorize(Roles = Commons.ADMIN_NAME)]
        [SwaggerOperation(Summary = "Tạo tài khoản cho nội bộ. Role duy nhất được phép: Admin")]

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

        [HttpPut("Update"), Authorize(Roles = Commons.TOTAL_INTERNAL_ROLE_NAME)]
        [SwaggerOperation(Summary = "Cập nhật thông tin cá nhân, chỉ được cập nhật chính người đang đăng nhập. Username cần update lấy từ Token người dùng.")]
        public async Task<IActionResult> UpdateInternalUser(UpdateInternalUser updateInternalUser)
        {
            try
            {
                var check = await _employeeAuthService.UpdateInternalUser(updateInternalUser);
                if(check.isError)
                {
                    return BadRequest(check);
                }
                return Ok("Cập nhật thông tin cá nhân thành công.");
            } catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("Status"), Authorize(Roles = Commons.ADMIN_NAME)]
        [SwaggerOperation(Summary = "Cập nhật tình trạng hoạt động nhân viên")]
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


