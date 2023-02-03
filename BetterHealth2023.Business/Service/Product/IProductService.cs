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
        public Task<bool> CreateProduct(CreateProductModel createProductModel);

        public Task<ViewProductModel> GetAllProduct(ProductPagingRequest pagingRequest);
    }
}
