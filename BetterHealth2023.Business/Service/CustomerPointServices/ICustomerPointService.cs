using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerPointServices
{
    public interface ICustomerPointService
    {
        public Task<IActionResult> GetCustomerAvailablePoint(string phoneNo);
    }
}
