using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos
{
    public interface IProductDetailRepo : IRepository<ProductDetail>
    {
        public Task<CartItem> AddMoreProductInformationToCart(string productId);
        public Task<bool> UpdateProductDetailRange(List<UpdateProductDetailModel> updateProductDetailModels);

        public Task<PagedResult<ViewProductListModel>> GetAllProductsPagingForHomePage(ProductPagingHomePageRequest pagingRequest);
        public Task<PagedResult<ViewProductListModel>> GetAllProductsPagingForCustomer(ProductPagingRequest pagingRequest);
        public Task<PagedResult<ViewProductListModelForInternal>> GetAllProductsPagingForInternalUser(ProductPagingRequest pagingRequest);
        public Task<ViewSpecificProductModel> GetSpecificProduct(string productID, bool isInternal);
        public Task<bool> CheckDuplicateBarCode(string BarCode);

        public Task<InformationToSendEmail> GetImageAndProductName(string productId);
        public Task<ProductUnitModelForDiscount> GetProductNameAndCurrentUnit(string productId);

        public Task<bool> CheckDuplicateBarCodeUpdate(string BarCode, string productID);

        public Task<List<ProductUnitModel>> GetProductLaterUnitWithFilter(string productID, int unitLevel, ProductUnitFilterRequest filterRequest);
        public Task<List<ProductUnitModel>> GetProductLaterUnit(string productID, int unitLevel);
        public Task<List<ProductUnitModel>> GetProductUnitButThis(string productID, int unitLevel);
        public Task<List<ViewProductListModel>> GetAllProductForInternal(string productParentID, bool? isSell);
        public Task<List<ViewProductListModel>> GetAllProductWithParent(string productParentID, int loadSellProduct);

        public Task<string> GetProductParentID(string productID);
        public Task<List<UpdateProductDetailModel>> GetProductDetailLists(string productParentID);

    }

    public class ProductUnitModelForDiscount
    {
        public string Name { get; set; }
        public int UnitLevel { get; set; }
        public double Price { get; set; }
    }
}
