using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerPointModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerPointServices
{
    public interface ICustomerPointService
    {
        public Task<IActionResult> GetCustomerPointHistory(string phoneNo, CustomerPointPagingRequest pagingRequest);
        public Task<IActionResult> GetCustomerAvailablePoint(string phoneNo);
    }
}
