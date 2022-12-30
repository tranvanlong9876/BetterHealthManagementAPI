using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository
{
    public class EmployeeAuthRepo : Repository<Employee>, IEmployeeAuthRepo
    {
        public EmployeeAuthRepo(BetterHealthManagementContext context) : base (context)
        {

        }

        public async Task<bool> CheckDuplicateUsername(string username)
        {
            var query = from x in context.Employees
                        where x.Username.ToLower().Trim().Equals(username.ToLower().Trim())
                        select new { x };
            var employee = await query.Select(selector => new Employee()).FirstOrDefaultAsync();
            if (employee != null) return true;
            return false;
        }

        public async Task<bool> CheckDuplicateEmail(string email, bool isUpdate)
        {
            var query = from x in context.Employees
                        where x.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                        select new { x };
            var employee = await query.Select(selector => new Employee()).FirstOrDefaultAsync();
            if (employee != null) return true;
            return false;
        }

        public async Task<bool> CheckDuplicatePhoneNo(string phoneNo, bool isUpdate)
        {
            var query = from x in context.Employees
                        where x.PhoneNo.ToLower().Trim().Equals(phoneNo.ToLower().Trim())
                        select new { x };
            var employee = await query.Select(selector => new Employee()).FirstOrDefaultAsync();
            if (employee != null) return true;
            return false;
        }

        public async Task<Employee> CheckLogin(LoginEmployee loginEmployee)
        {
            var query = from x in context.Employees
                        where x.Username.ToLower().Trim().Equals(loginEmployee.Username.ToLower().Trim())
                        select new { x };

            var employee = await query.Select(selector => new Employee()
            {
                Id = selector.x.Id,
                Username = selector.x.Username,
                Fullname = selector.x.Fullname,
                ImageUrl = selector.x.ImageUrl,
                Password = selector.x.Password,
                PasswordSalt = selector.x.PasswordSalt,
                Role = selector.x.Role,
                SiteId = selector.x.SiteId,
                RoleId = selector.x.RoleId,
                Status = selector.x.Status,
                Email = selector.x.Email
            }).FirstOrDefaultAsync();

            return employee;
        }

        public async Task<bool> RegisterEmployee(Employee employee)
        {
            await context.AddAsync(employee);
            await Update();
            return true;
        }
    }
}
