using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductExportRepos
{
    public class ProductExportRepo : Repository<SiteInventoryBatch>, IProductExportRepo
    {
        public ProductExportRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<ViewListExportProductModel> GetDamageProduct(string siteInventoryId)
        {
            var query = from inventory in context.SiteInventoryBatches
                        from batch in context.ProductImportBatches.Where(x => x.Id == inventory.ImportBatchId)
                        from productDetail in context.ProductDetails.Where(x => x.Id == inventory.ProductId).DefaultIfEmpty()
                        from productParent in context.ProductParents.Where(x => x.Id == productDetail.ProductIdParent).DefaultIfEmpty()
                        from productImage in context.ProductImages.Where(x => x.ProductId == productParent.Id && x.IsFirstImage).DefaultIfEmpty()
                        from unit in context.Units.Where(x => x.Id == productDetail.UnitId).DefaultIfEmpty()
                        select new { inventory, batch, productDetail, productParent, unit, productImage };

            query = query.Where(x => x.batch.ExpireDate < CustomDateTime.Now && x.inventory.Id.Equals(siteInventoryId) && x.inventory.Quantity > 0);

            return await query
                .Select(selector => new ViewListExportProductModel()
                {
                    BatchId = selector.inventory.ImportBatchId,
                    ExpireDate = selector.batch.ExpireDate,
                    ProductId = selector.inventory.ProductId,
                    ProductImage = selector.productImage.ImageUrl,
                    ProductName = selector.productParent.Name,
                    Quantity = selector.inventory.Quantity,
                    SiteInventoryId = selector.inventory.Id,
                    UnitName = selector.unit.UnitName
                }).FirstOrDefaultAsync();
        }

        public async Task<PagedResult<ViewListExportProductModel>> GetListDamageProduct(GetPagingExportDamageProduct pagingRequest)
        {
            var query = from inventory in context.SiteInventoryBatches
                        from batch in context.ProductImportBatches.Where(x => x.Id == inventory.ImportBatchId)
                        from productDetail in context.ProductDetails.Where(x => x.Id == inventory.ProductId).DefaultIfEmpty()
                        from productParent in context.ProductParents.Where(x => x.Id == productDetail.ProductIdParent).DefaultIfEmpty()
                        from productImage in context.ProductImages.Where(x => x.ProductId == productParent.Id && x.IsFirstImage).DefaultIfEmpty()
                        from unit in context.Units.Where(x => x.Id == productDetail.UnitId).DefaultIfEmpty()
                        select new { inventory, batch, productDetail, productParent, unit, productImage };

            query = query.Where(x => x.batch.ExpireDate < CustomDateTime.Now && x.inventory.SiteId.Equals(pagingRequest.SiteId) && x.inventory.Quantity > 0);

            if (!string.IsNullOrEmpty(pagingRequest.ProductName))
            {
                query = query.Where(x => x.productParent.Name.Contains(pagingRequest.ProductName));
            }

            query.OrderBy(x => x.batch.ExpireDate);

            var totalRow = await query.CountAsync();

            var productList = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                .Take(pagingRequest.pageItems)
                .Select(selector => new ViewListExportProductModel()
                {
                    BatchId = selector.inventory.ImportBatchId,
                    ExpireDate = selector.batch.ExpireDate,
                    ProductId = selector.inventory.ProductId,
                    ProductImage = selector.productImage.ImageUrl,
                    ProductName = selector.productParent.Name,
                    Quantity = selector.inventory.Quantity,
                    SiteInventoryId = selector.inventory.Id,
                    UnitName = selector.unit.UnitName
                }).ToListAsync();

            return new PagedResult<ViewListExportProductModel>(productList, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);
        }
    }
}
