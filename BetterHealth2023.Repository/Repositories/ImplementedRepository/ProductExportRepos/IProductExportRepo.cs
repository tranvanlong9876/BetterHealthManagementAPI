using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductExportRepos
{
    public interface IProductExportRepo : IRepository<SiteInventoryBatch>
    {
        public Task<PagedResult<ViewListExportProductModel>> GetListDamageProduct(GetPagingExportDamageProduct pagingRequest);

        public Task<ViewListExportProductModel> GetDamageProduct(string siteInventoryId);
    }

}
