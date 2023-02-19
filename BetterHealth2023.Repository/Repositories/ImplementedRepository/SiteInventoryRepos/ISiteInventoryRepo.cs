using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos
{
    public interface ISiteInventoryRepo : IRepository<SiteInventory>
    {
        public Task<SiteInventory> GetSiteInventory(string siteID, string ProductID);

    }
}
