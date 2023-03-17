using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.ProductErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product
{
    public interface IProductService
    {
        public Task<CartItem> AddMoreProductInformationToCart(string productId);
        public Task<CreateProductErrorModel> CreateProduct(CreateProductModel createProductModel);

        public Task<PagedResult<ViewProductListModel>> GetAllProductsPagingForCustomer(ProductPagingRequest pagingRequest);
        public Task<PagedResult<ViewProductListModelForInternal>> GetAllProductsPagingForInternalUser(ProductPagingRequest pagingRequest);
        public Task<ViewSpecificProductModel> GetViewProduct(string productId, bool isInternal);

        public Task<UpdateProductViewModel> GetViewProductForUpdate(string productId);

        public Task<UpdateProductErrorModel> UpdateProduct(UpdateProductEntranceModel updateProductModel);
    }
}
