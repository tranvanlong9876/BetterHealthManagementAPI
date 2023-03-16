using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderBatchRepos
{
    public interface IOrderBatchRepo : IRepository<OrderBatch>
    {
        //Đã convert về đơn vị thấp nhất
        public Task<List<ViewSpecificOrderBatch>> GetViewSpecificOrderBatches(string orderId, string productId);
    }
}
