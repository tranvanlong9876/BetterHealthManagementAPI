using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.ProductErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product
{
    public interface IProductService
    {
        public Task<CreateProductErrorModel> CreateProduct(CreateProductModel createProductModel);

        public Task<PagedResult<ViewProductListModel>> GetAllProduct(ProductPagingRequest pagingRequest);
    }
}
