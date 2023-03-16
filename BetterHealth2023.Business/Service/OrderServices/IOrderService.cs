using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices
{
    public interface IOrderService
    {
        public Task<ViewSiteToPickUpStatus> GetViewSiteToPickUps(CartEntrance cartEntrance);
        public Task<CreateOrderCheckOutStatus> CheckOutOrder(CheckOutOrderModel checkOutOrderModel, string CustomerId);

        public Task<string> GenerateOrderId();

        public Task<PagedResult<ViewOrderList>> GetAllOrders(GetOrderListPagingRequest pagingRequest, UserInformation userInformation);

        public Task<ViewOrderSpecific> GetSpecificOrder(string orderId, UserInformation userInformation);
    }
}
