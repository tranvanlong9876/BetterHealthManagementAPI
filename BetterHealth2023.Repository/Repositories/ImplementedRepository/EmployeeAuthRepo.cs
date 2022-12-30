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

        public async Task<Employee> CheckLogin(LoginEmployee loginEmployee)
        {
            var query = from x in context.Employees
                        where x.Username.ToLower().Trim().Equals(loginEmployee.Username.ToLower().Trim())
                        select new { x };

            var employee = await query.Select(selector => new Employee()
            {
                Id = selector.x.Id,
                Username = selector.x.Username,
                Password = selector.x.Password,
                PasswordSalt = selector.x.PasswordSalt,
                Role = selector.x.Role,
                SiteId = selector.x.SiteId,
                RoleId = selector.x.RoleId,
                Status = selector.x.Status
            }).FirstOrDefaultAsync();

            return employee;
        }

        public async Task<string> RegisterEmployee(Employee employee)
        {
            //await context.AddAsync(employee);
            //await Update();
            var id = await Insert(employee);
            return id;
        }
    }
}
