using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Employee
{
    public interface IEmployeeAuthService
    {
        //Tạo nhân viên
        Task<RegisterInternalUserStatus> Register(RegisterInternalUser employee);
        
        //Đăng nhập nội bộ
        Task<InternalUserTokenModel> Login(LoginInternalUser loginEmployee);

        Task<List<Repository.DatabaseModels.InternalUser>> GetEmployeeById(string id);
        
    }
}
