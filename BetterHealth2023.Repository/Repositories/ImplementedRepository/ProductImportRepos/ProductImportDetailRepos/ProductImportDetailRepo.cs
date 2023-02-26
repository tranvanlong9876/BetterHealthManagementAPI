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

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportDetailRepos
{
    public class ProductImportDetailRepo : Repository<ProductImportDetail>, IProductImportDetailRepo
    {
        public ProductImportDetailRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ProductImportDetail>> GetProductImportDetails(string receiptID)
        {
            return await context.ProductImportDetails.Where(x => x.ReceiptId.Equals(receiptID)).ToListAsync();
        }

        public async Task<List<ViewSpecificProductImportDetails>> GetProductImportDetailsViewModel(string receiptID)
        {
            var data = await context.ProductImportDetails.Where(x => x.ReceiptId.Equals(receiptID)).ToListAsync(); 
            return data.Select(model => mapper.Map<ViewSpecificProductImportDetails>(model)).ToList();
        }

        public async Task<bool> RemoveRangesImportDetails(List<ProductImportDetail> productImportDetails)
        {
            context.ProductImportDetails.RemoveRange(productImportDetails);
            await Update();
            return true;
        }
    }
}
