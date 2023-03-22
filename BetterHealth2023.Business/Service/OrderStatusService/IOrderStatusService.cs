using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderStatusModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderStatusService
{
    public interface IOrderStatusService
    {
        public Task<IActionResult> GetOrderStatusBasedOnOrderType(OrderStatusFilterRequest filterRequest);
    }
}
