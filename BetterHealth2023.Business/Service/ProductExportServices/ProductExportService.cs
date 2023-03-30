using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductExportRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductExportServices
{
    public class ProductExportService : IProductExportService
    {
        private readonly IProductExportRepo _productExportRepo;
        private readonly ISiteInventoryRepo _siteInventoryRepo;

        public ProductExportService(IProductExportRepo productExportRepo, ISiteInventoryRepo siteInventoryRepo)
        {
            _productExportRepo = productExportRepo;
            _siteInventoryRepo = siteInventoryRepo;
        }

        public async Task<IActionResult> ExportDamageProduct(ExportDamageProductEntrance exportDamageProduct)
        {
            var siteInventoryModel = await _siteInventoryRepo.Get(exportDamageProduct.SiteInventoryId);

            if (siteInventoryModel == null) return new NotFoundObjectResult("Không tìm thấy tồn kho cần xuất hỏng");

            if (siteInventoryModel.Quantity < exportDamageProduct.ExportQuantity) return new BadRequestObjectResult("Số lượng cần xuất hỏng đang lớn hơn số lượng tồn kho đang có.");

            siteInventoryModel.Quantity -= exportDamageProduct.ExportQuantity;

            await _siteInventoryRepo.Update();

            return new OkObjectResult("Sản phẩm đã được xuất hỏng thành công!");
        }

        public async Task<IActionResult> GetListProductDamaged(GetPagingExportDamageProduct pagingRequest)
        {
            return new OkObjectResult(await _productExportRepo.GetListDamageProduct(pagingRequest));
        }

        public async Task<IActionResult> GetSpecificProductDamaged(string siteInventoryId)
        {
            var damageProductModel = await _productExportRepo.GetDamageProduct(siteInventoryId);

            if (damageProductModel == null) return new NotFoundObjectResult("Không tìm thấy tồn kho hết hạn!");

            return new OkObjectResult(damageProductModel);
        }
    }
}
