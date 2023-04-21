using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCancelModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderValidateModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices
{
    public interface IOrderService
    {
        public Task<IActionResult> GetOrderExecutionHistory(string orderId);
        public Task<IActionResult> ExecuteOrder(OrderExecutionModel orderExecutionModel, string pharmacistToken);
        public Task<IActionResult> ValidateOrder(ValidateOrderModel validateOrderModel, string pharmacistToken);
        public Task<ViewSiteToPickUpStatus> GetViewSiteToPickUps(CartEntrance cartEntrance);
        public Task<IActionResult> CheckOutOrder(CheckOutOrderModel checkOutOrderModel, string token);

        public Task<IActionResult> CancelOrder(OrderCancelModel orderCancelModel, UserInformation userInformation);
        public Task<string> GenerateOrderId();

        public Task<PagedResult<ViewOrderList>> GetAllOrders(GetOrderListPagingRequest pagingRequest, UserInformation userInformation);

        public Task<ViewOrderSpecific> GetSpecificOrder(string orderId, UserInformation userInformation);

        public Task<IActionResult> UpdateOrderProductNoteModel(List<UpdateOrderProductNoteModel> ListNoteModel);
    }
}
