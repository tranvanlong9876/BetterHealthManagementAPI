using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportBatchRepos
{
    public interface IProductImportBatchRepo : IRepository<ProductImportBatch>
    {
        public Task<List<ProductImportBatch>> GetProductImportBatches(string importDetailID);

        public Task<List<ViewSpecificProductImportBatches>> GetProductImportBatchesViewModel(string importDetailID);

        public Task<bool> RemoveBatchesRange(List<ProductImportBatch> importBatches);
    }
}
