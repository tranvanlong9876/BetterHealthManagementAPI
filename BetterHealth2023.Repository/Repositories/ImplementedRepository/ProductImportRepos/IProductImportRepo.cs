using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos
{
    public interface IProductImportRepo : IRepository<ProductImportReceipt>
    {
        public Task<bool> checkProductManageByBatches(string productID);

        public Task<PagedResult<ViewListProductImportModel>> ViewListProductImportPaging(GetProductImportPagingRequest pagingRequest);

    }
}
