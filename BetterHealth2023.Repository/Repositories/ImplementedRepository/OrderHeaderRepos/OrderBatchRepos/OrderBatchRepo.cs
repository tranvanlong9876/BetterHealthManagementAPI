using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderBatchRepos
{
    public class OrderBatchRepo : Repository<OrderBatch>, IOrderBatchRepo
    {
        public OrderBatchRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ViewSpecificOrderBatch>> GetViewSpecificOrderBatches(string orderId, string productId)
        {
            var query = from batch in context.OrderBatches
                        from siteInvenBatch in context.SiteInventoryBatches.Where(x => x.Id == batch.SiteInventoryBatchId).DefaultIfEmpty()
                        from productImportBatch in context.ProductImportBatches.Where(x => x.Id == siteInvenBatch.ImportBatchId).DefaultIfEmpty()
                        from productDetail in context.ProductDetails.Where(x => x.Id == siteInvenBatch.ProductId).DefaultIfEmpty()
                        from unit in context.Units.Where(x => x.Id == productDetail.UnitId).DefaultIfEmpty()
                        select new { batch, siteInvenBatch, productImportBatch, unit };

            query = query.Where(x => x.batch.OrderId.Equals(orderId) && x.siteInvenBatch.ProductId.Equals(productId));

            return await query.Select(selector => new ViewSpecificOrderBatch()
            {
                ExpireDate = selector.productImportBatch.ExpireDate,
                ManufacturerDate = selector.productImportBatch.ManufactureDate,
                Quantity = selector.batch.SoldQuantity,
                UnitName = selector.unit.UnitName
            }).ToListAsync();

        }
    }
}
