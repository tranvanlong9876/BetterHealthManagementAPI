using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository
{
    public interface IEmployeeInfoRepo : IRepository<EmployeeInfo>
    {
        public Task<EmployeeInfo> GetEmployeeInfo(string id);
        public Task<bool> InsertEmployeeInfo(EmployeeInfo employeeInfo);
        public Task<bool> CheckDuplicatePhoneNo(string phoneNo, bool isUpdate);
        public Task<bool> CheckDuplicateEmail(string email, bool isUpdate);
    }
}
