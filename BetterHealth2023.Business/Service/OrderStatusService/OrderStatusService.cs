using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderStatusRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderStatusModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderStatusService
{
    public class OrderStatusService : IOrderStatusService
    {

        private readonly IOrderStatusRepo _orderStatusRepo;

        public OrderStatusService(IOrderStatusRepo orderStatusRepo)
        {
            _orderStatusRepo = orderStatusRepo;
        }

        public async Task<IActionResult> GetOrderStatusBasedOnOrderType(OrderStatusFilterRequest filterRequest)
        {
            return new OkObjectResult(await _orderStatusRepo.GetAllOrderStatus(filterRequest));
        }
    }
}
