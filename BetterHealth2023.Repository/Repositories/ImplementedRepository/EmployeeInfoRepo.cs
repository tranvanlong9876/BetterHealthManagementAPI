using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository
{
    public class EmployeeInfoRepo : Repository<EmployeeInfo>, IEmployeeInfoRepo
    {
        public EmployeeInfoRepo(BetterHealthManagementContext context) : base(context)
        {

        }

        public async Task<bool> CheckDuplicateEmail(string email, bool isUpdate)
        {
            var query = from x in context.EmployeeInfos
                        where x.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                        select new { x };
            var employeeInfo = await query.Select(selector => new EmployeeInfo()).FirstOrDefaultAsync();
            if (employeeInfo != null) return true;
            return false;
        }

        public async Task<bool> CheckDuplicatePhoneNo(string phoneNo, bool isUpdate)
        {
            var query = from x in context.EmployeeInfos
                        where x.PhoneNo.ToLower().Trim().Equals(phoneNo.ToLower().Trim())
                        select new { x };
            var employeeInfo = await query.Select(selector => new EmployeeInfo()).FirstOrDefaultAsync();
            if (employeeInfo != null) return true;
            return false;
        }

        public async Task<EmployeeInfo> GetEmployeeInfo(string id)
        {
            var employeeInfo = await context.EmployeeInfos.Where(x => x.EmployeeId == id.Trim()).FirstOrDefaultAsync();
            return employeeInfo;
        }

        public async Task<bool> InsertEmployeeInfo(EmployeeInfo employeeInfo)
        {
            string query = "INSERT INTO Employee_Info VALUES(@employeeID, @fullName, @phoneNo, @email, @addressID, @imageURL, @DOB)";
            SqlParameter[] sqlParameter = new SqlParameter[7];
            sqlParameter[0] = new SqlParameter("@employeeID", SqlDbType.NVarChar, 50) { Value = employeeInfo.EmployeeId };
            sqlParameter[1] = new SqlParameter("@fullName", SqlDbType.NVarChar, 100) { Value = employeeInfo.Fullname };
            sqlParameter[2] = new SqlParameter("@phoneNo", SqlDbType.NVarChar, 20) { Value = employeeInfo.PhoneNo };
            sqlParameter[3] = new SqlParameter("@email", SqlDbType.NVarChar, 100) { Value = employeeInfo.Email };
            sqlParameter[4] = new SqlParameter("@addressID", SqlDbType.NVarChar, 50) { Value = (employeeInfo.AddressId == null ? DBNull.Value : employeeInfo.AddressId) };
            sqlParameter[5] = new SqlParameter("@imageURL", SqlDbType.NVarChar, 100) { Value = employeeInfo.ImageUrl };
            sqlParameter[6] = new SqlParameter("@DOB", SqlDbType.Date) { Value = (employeeInfo.Dob == null ? DBNull.Value : employeeInfo.Dob) };
            context.Database.ExecuteSqlRaw(query, sqlParameter);
            await Update();
            return true;
        }
    }
}
