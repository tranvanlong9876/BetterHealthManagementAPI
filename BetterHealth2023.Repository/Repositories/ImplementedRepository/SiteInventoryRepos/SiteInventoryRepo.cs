using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos
{
    public class SiteInventoryRepo : Repository<SiteInventory>, ISiteInventoryRepo
    {
        public SiteInventoryRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<SiteInventory> GetSiteInventory(string siteID, string ProductID)
        {
            var query = from siteinven in context.SiteInventories.Where(x => x.ProductId.Equals(ProductID.Trim()) && x.SiteId.Equals(siteID.Trim()))
                        select siteinven;

            return await query.FirstOrDefaultAsync();
        }
    }
}
