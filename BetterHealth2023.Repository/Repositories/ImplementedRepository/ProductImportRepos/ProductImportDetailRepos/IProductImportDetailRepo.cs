using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportDetailRepos
{
    public interface IProductImportDetailRepo : IRepository<ProductImportDetail>
    {
        public Task<List<ProductImportDetail>> GetProductImportDetails(string receiptID);

        public Task<List<ViewSpecificProductImportDetails>> GetProductImportDetailsViewModel(string receiptID);

        public Task<bool> RemoveRangesImportDetails(List<ProductImportDetail> productImportDetails);
    }
}
