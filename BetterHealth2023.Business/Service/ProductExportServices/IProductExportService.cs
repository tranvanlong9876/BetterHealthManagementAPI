using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductExportServices
{
    public interface IProductExportService
    {
        public Task<IActionResult> GetListProductDamaged(GetPagingExportDamageProduct pagingRequest);

        public Task<IActionResult> GetSpecificProductDamaged(string siteInventoryId);

        public Task<IActionResult> ExportDamageProduct(ExportDamageProductEntrance exportDamageProduct);
    }
}
