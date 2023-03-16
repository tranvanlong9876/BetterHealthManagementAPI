using System.Collections.Generic;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos
{
    public interface IOrderHeaderRepo : IRepository<OrderHeader>
    {
        public Task<List<OrderHeader>> GetOrderHeadersBySiteId(string siteId);

        public Task<List<OrderHeader>> GetExecutingOrdersByPharmacistId(string pharID);

        public Task<bool> CheckDuplicateOrderId(string orderId);

        public Task<PagedResult<ViewOrderList>> GetAllOrders(GetOrderListPagingRequest pagingRequest, UserInformation userInformation);

        public Task<ViewOrderSpecific> GetSpecificOrder(string orderId);
    }
}
