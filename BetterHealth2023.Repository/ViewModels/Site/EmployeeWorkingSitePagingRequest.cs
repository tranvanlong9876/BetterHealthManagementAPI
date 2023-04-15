using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class EmployeeWorkingSitePagingRequest : PagingRequestBase
    {
        /// <summary>
        /// Filter theo Role. 1 là Manager, 2 là Pharmacist
        /// </summary>
        public string RoleId { get; set; }


        /// <summary>
        /// Search Tên
        /// </summary>
        public string FullName { get; set; }
    }
}
