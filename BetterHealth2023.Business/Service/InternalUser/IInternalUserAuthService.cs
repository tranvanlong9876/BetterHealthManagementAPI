using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalUser
{
    public interface IInternalUserAuthService
    {
        Task<UserInfoModel> GetUserInfoModel(string guid);
        //Tạo nhân viên
        Task<RegisterInternalUserStatus> Register(RegisterInternalUser employee);

        //Cập nhật thông tin tài khoản
        Task<UpdateInternalUserStatus> UpdateInternalUser(UpdateInternalUser user);
        
        //Đăng nhập nội bộ
        Task<LoginUserStatus> Login(LoginInternalUser loginEmployee);

        Task<List<Repository.DatabaseModels.InternalUser>> GetEmployeeById(string id);

        Task<UpdateUserStatus> UpdateAccountStatus(string guid, int status);
    }
}
