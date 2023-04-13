using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SiteInventoryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos
{
    public interface ISiteInventoryRepo : IRepository<SiteInventoryBatch>
    {
        public Task<List<OrderBatch>> GetAllSiteInventoryBatchFromOrderProductBatch(string productId, string siteId, string orderId);
        public Task<List<SiteInventoryBatch>> GetAllProductBatchesAvailable(string productId, string siteId);
        public Task<SiteInventoryBatch> GetSiteInventory(string siteID, string ProductID);
        public Task<SiteModelToPickUp> ViewSiteToPickUpsAsync(List<CartModel> cartModels, string cityId, string districtId);
        public Task<SiteInventoryModel> GetInventoryOfProductOfSite(string productId, string siteId, int quantityConvert);
        public Task<List<ViewSpecificMissingProduct>> CheckMissingProductOfSiteId(string SiteId, List<OrderProductLastUnitLevel> orderProducts);
    }
}
