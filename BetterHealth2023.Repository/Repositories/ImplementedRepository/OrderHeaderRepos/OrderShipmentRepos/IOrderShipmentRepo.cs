using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderShipmentRepos
{
    public interface IOrderShipmentRepo : IRepository<OrderShipment>
    {
        public Task<ViewSpecificOrderDelivery> GetOrderDeliveryInformation(string orderId);
    }
}
