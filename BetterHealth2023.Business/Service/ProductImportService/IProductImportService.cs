using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductImportService
{
    public interface IProductImportService
    {
        public Task<CreateProductImportStatus> CreateProductImport(CreateProductImportModel importModel);

        public Task<UpdateProductImportStatus> UpdateProductImport(UpdateProductImportModel updateProductImport);

        public Task<bool> ReleaseProductImportController(string productImportID, string siteID);

        public Task<ViewSpecificProductImportModel> ViewSpecificProductImport(string productImportID);

        public Task<PagedResult<ViewListProductImportModel>> ViewListProductImportPaging(GetProductImportPagingRequest pagingRequest);
        
    }
}
