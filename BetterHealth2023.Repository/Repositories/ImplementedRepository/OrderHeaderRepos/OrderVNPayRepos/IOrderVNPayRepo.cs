using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderVNPayRepos
{
    public interface IOrderVNPayRepo : IRepository<OrderVnpay>
    {
        public Task<RefundVNPayModel> GetRefundVNPayModel(string orderId);

        public Task<OrderVnpay> GetTransaction(string orderId);
    }
}
