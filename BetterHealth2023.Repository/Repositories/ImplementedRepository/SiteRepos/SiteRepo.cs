using System;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.AspNetCore.Mvc;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos
{
    public class SiteRepo : Repository<SiteInformation>, ISiteRepo
    {
        public SiteRepo(BetterHealthManagementContext context) : base(context)
        {
        }

        public async Task<SiteInformation> InsertNewSite(SiteInformation siteInformation)
        {
             context.AddAsync(siteInformation);
            await Update();
            return siteInformation;
        }  
       

      
        
        public async Task<SiteInformation> UpdateSite(SiteInformation siteInformation)
        {
            context.Update(siteInformation);
            await Update();
            return siteInformation;
        }

      
        public async Task DeleteSite(SiteInformation siteInformation)
        {
            context.Remove(siteInformation);
            await Update();
          
        }

       
        public async Task<SiteInformation> GetSiteById(string id)
        {
           return await context.SiteInformations.FindAsync(id);
        }
    }
  
}
