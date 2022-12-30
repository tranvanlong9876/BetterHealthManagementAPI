using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Employee;
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
        Task<RegisterEmployeeStatus> Register(RegisterEmployee employee);

        //thằng Nguyên edit
        Task<EmployeeTokenModel> Login(LoginEmployee loginEmployee);
    }
}
