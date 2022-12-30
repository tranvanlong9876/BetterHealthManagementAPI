using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository
{
    public interface IEmployeeAuthRepo : IRepository<Employee>
    {
        public Task<bool> RegisterEmployee(Employee employee);
        public Task<Employee> CheckLogin(LoginEmployee loginEmployee);

        public Task<bool> CheckDuplicateUsername(string username);

        public Task<bool> CheckDuplicatePhoneNo(string phoneNo, bool isUpdate);

        public Task<bool> CheckDuplicateEmail(string email, bool isUpdate);
    }
}
