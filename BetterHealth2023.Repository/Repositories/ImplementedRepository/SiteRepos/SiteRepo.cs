using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos
{
    public class SiteRepo : Repository<SiteInformation>, ISiteRepo
    {
        public SiteRepo(BetterHealthManagementContext context) : base(context)
        {

        }


        public async Task<bool> UpdateSite(SiteInformation siteInformation)
        {
            context.Update(siteInformation);
            await Update();
            return true;
        }

        public async Task DeleteSite(SiteInformation siteInformation)
        {
            context.Remove(siteInformation);
            await Update();
        }

        public async Task<SiteViewModel> GetSiteById(string id)
        {
            var query = from x in context.SiteInformations
                        where x.Id.Trim().Equals(id.Trim())
                        select new { x };
            var siteView = await query.Select(selector => new SiteViewModel()
            {
                Id = selector.x.Id,
                ImageUrl = selector.x.ImageUrl,
                SiteName = selector.x.SiteName,
                AddressId = selector.x.AddressId,
                LastUpdate = selector.x.LastUpdate,
                Description = selector.x.Description,
                ContactInfo = selector.x.ContactInfo,
                IsActivate = selector.x.IsActivate,
                IsDelivery = selector.x.IsDelivery
            }).FirstOrDefaultAsync();
            return siteView;
        }

        public async Task<List<SiteViewModel>> GetAllSite()
        {
            var query = from x in context.SiteInformations
                        select new { x };
            var siteList = await query.Select(selector => new SiteViewModel()
            {
                Id = selector.x.Id,
                ImageUrl = selector.x.ImageUrl,
                SiteName = selector.x.SiteName,
                AddressId = selector.x.AddressId,
                LastUpdate = selector.x.LastUpdate,
                Description = selector.x.Description,
                ContactInfo = selector.x.ContactInfo,
                IsActivate = selector.x.IsActivate,
                IsDelivery = selector.x.IsDelivery
            }).ToListAsync();
            return siteList;
        }
    }

}
