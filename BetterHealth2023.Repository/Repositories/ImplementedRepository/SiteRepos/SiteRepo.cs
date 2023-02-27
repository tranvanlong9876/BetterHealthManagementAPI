using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
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

        public async Task<PagedResult<SiteViewModel>> GetAllSitePaging(GetSitePagingRequest pagingRequest)
        {
            var query = from site in context.SiteInformations
                        from address in context.DynamicAddresses.Where(x => x.Id == site.AddressId).DefaultIfEmpty()
                        from city in context.Cities.Where(x => x.Id == address.CityId).DefaultIfEmpty()
                        from district in context.Districts.Where(x => x.Id == address.DistrictId).DefaultIfEmpty()
                        orderby site.IsDelivery descending, site.IsActivate descending
                        select new { site, address, city, district };

            if (pagingRequest.IsActive.HasValue)
            {
                query = query.Where(x => x.site.IsActivate.Equals(pagingRequest.IsActive));
            }

            if (pagingRequest.IsDelivery.HasValue)
            {
                query = query.Where(x => x.site.IsDelivery.Equals(pagingRequest.IsDelivery));
            }

            if (!string.IsNullOrEmpty(pagingRequest.CityID))
            {
                query = query.Where(x => x.city.Id.Equals(pagingRequest.CityID));
            }

            if (!string.IsNullOrEmpty(pagingRequest.DistrictID))
            {
                query = query.Where(x => x.district.Id.Equals(pagingRequest.DistrictID));
            }

            if (!string.IsNullOrEmpty(pagingRequest.SiteName))
            {
                query = query.Where(x => x.site.SiteName.Contains(pagingRequest.SiteName.Trim()));
            }

            int totalRow = await query.CountAsync();

            var siteList = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                .Take(pagingRequest.pageItems)
                .Select(selector => new SiteViewModel()
                {
                    Id = selector.site.Id,
                    ImageUrl = selector.site.ImageUrl,
                    SiteName = selector.site.SiteName,
                    AddressId = selector.site.AddressId,
                    LastUpdate = selector.site.LastUpdate,
                    Description = selector.site.Description,
                    ContactInfo = selector.site.ContactInfo,
                    IsActivate = selector.site.IsActivate,
                    IsDelivery = selector.site.IsDelivery,
                }).ToListAsync();

            var pageResult = new PagedResult<SiteViewModel>(siteList, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pageResult;
        }

        public Task<string> GetSiteAddressId(string siteId)
        {
            throw new NotImplementedException();
        }
    }

}
