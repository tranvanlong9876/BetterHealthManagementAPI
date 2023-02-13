using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos
{
    public interface IProductDetailRepo : IRepository<ProductDetail>
    {
        public Task<bool> UpdateProductDetailRange(List<UpdateProductDetailModel> updateProductDetailModels);
        public Task<PagedResult<ViewProductListModel>> GetAllProductsPaging(ProductPagingRequest pagingRequest);
        public Task<ViewSpecificProductModel> GetSpecificProduct(string productID, bool isInternal);
        public Task<bool> CheckDuplicateBarCode(string BarCode);

        public Task<bool> CheckDuplicateBarCodeUpdate(string BarCode, string productID);

        public Task<List<ProductUnitModel>> GetProductLaterUnit(string productID, int unitLevel);
        public Task<List<ProductUnitModel>> GetProductUnitButThis(string productID, int unitLevel);

        public Task<string> GetProductParentID(string productID);
        public Task<List<UpdateProductDetailModel>> GetProductDetailLists(string productParentID);
    }
}
