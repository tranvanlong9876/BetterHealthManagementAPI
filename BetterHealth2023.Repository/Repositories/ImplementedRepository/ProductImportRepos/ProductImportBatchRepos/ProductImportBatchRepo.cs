using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportBatchRepos
{
    public class ProductImportBatchRepo : Repository<ProductImportBatch>, IProductImportBatchRepo
    {
        public ProductImportBatchRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ProductImportBatch>> GetProductImportBatches(string importDetailID)
        {
            return await context.ProductImportBatches.Where(x => x.ImportDetailId.Equals(importDetailID)).ToListAsync();
        }

        public async Task<List<ViewSpecificProductImportBatches>> GetProductImportBatchesViewModel(string importDetailID)
        {
            var data = await context.ProductImportBatches.Where(x => x.ImportDetailId.Equals(importDetailID)).ToListAsync();
            if (data == null) return null;

            return data.Select(model => mapper.Map<ViewSpecificProductImportBatches>(model)).ToList();
        }

        public async Task<bool> RemoveBatchesRange(List<ProductImportBatch> importBatches)
        {
            context.ProductImportBatches.RemoveRange(importBatches);
            await Update();
            return true;
        }
    }
}
