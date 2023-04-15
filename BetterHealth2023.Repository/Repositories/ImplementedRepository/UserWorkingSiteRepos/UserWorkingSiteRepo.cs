using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static System.Linq.Queryable;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UserWorkingSiteRepos
{
    public class UserWorkingSiteRepo : Repository<InternalUserWorkingSite>, IUserWorkingSiteRepo
    {
        public UserWorkingSiteRepo(BetterHealthManagementContext context) : base(context)
        {

        }

        public async Task<string> GetInternalUserWorkingSite(string userID)
        {
            var query = from x in context.InternalUserWorkingSites
                        where x.UserId.Trim().Equals(userID.Trim()) && x.IsWorking.Equals(true)
                        select new { x };
            var workingSite = await query.Select(selector => new InternalUserWorkingSite() { 
                Id = selector.x.Id,
                SiteId = selector.x.SiteId,
                UserId = selector.x.UserId
            }).FirstOrDefaultAsync();
            if(workingSite != null)
            {
                return workingSite.SiteId;
            }
            return null;
        }

        public async Task<SiteViewModel> GetInternalUserWorkingSiteModel(string userID)
        {
            var query = from x in context.InternalUserWorkingSites
                        from site in context.SiteInformations.Where(sites => sites.Id == x.SiteId)
                        where x.UserId.Trim().Equals(userID.Trim()) && x.IsWorking.Equals(true)
                        select new { x, site };
            var workingSiteModel = await query.Select(selector => new SiteViewModel()
            {
                Id = selector.site.Id,
                SiteName = selector.site.SiteName
            }).FirstOrDefaultAsync();
            
            return workingSiteModel;
        }

        public async Task<List<InternalUserWorkingSite>> GetTotalManager(string siteID)
        {
            var query = from x in context.InternalUserWorkingSites
                        join manager in context.InternalUsers on x.UserId equals manager.Id
                        where x.SiteId.Trim().Equals(siteID.Trim()) && manager.RoleId.Equals(Commons.Commons.MANAGER) && manager.Status.Equals(1) && x.IsWorking.Equals(true)
                        select new { x };
            var workingSite = await query.Select(selector => new InternalUserWorkingSite()).ToListAsync();

            return workingSite;
        }

        public async Task<List<InternalUserWorkingSite>> GetTotalPharmacist(string siteID)
        {
            var query = from x in context.InternalUserWorkingSites
                        join pharmacist in context.InternalUsers on x.UserId equals pharmacist.Id
                        where x.SiteId.Trim().Equals(siteID.Trim()) && pharmacist.RoleId.Equals(Commons.Commons.PHARMACIST) && pharmacist.Status.Equals(1) && x.IsWorking.Equals(true)
                        select new { x };
            var workingSite = await query.Select(selector => new InternalUserWorkingSite()).ToListAsync();

            return workingSite;

        }

        public async Task<PagedResult<UserWorkingSiteModel>> GetUserWorkingAtSite(EmployeeWorkingSitePagingRequest pagingRequest, string siteId)
        {
            var query = from workingSite in context.InternalUserWorkingSites.Where(x => x.IsWorking)
                        from user in context.InternalUsers.Where(x => x.Id == workingSite.UserId)
                        from role in context.RoleInternals.Where(x => x.Id == user.RoleId)
                        select new { user, workingSite, role };

            //Filter
            query = query.Where(x => x.workingSite.SiteId.Equals(siteId));

            if (!string.IsNullOrEmpty(pagingRequest.RoleId))
            {
                query = query.Where(x => x.user.RoleId.Equals(pagingRequest.RoleId));
            }

            if (!string.IsNullOrEmpty(pagingRequest.FullName))
            {
                query = query.Where(x => x.user.Fullname.Contains(pagingRequest.FullName));
            }

            query = query.OrderBy(x => x.user.Fullname);

            var totalRow = await query.CountAsync();

            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems).Take(pagingRequest.pageItems)
                        .Select(selector => new UserWorkingSiteModel()
                        {
                            Id = selector.workingSite.Id,
                            FullName = selector.user.Fullname,
                            RoleId = selector.user.RoleId,
                            RoleName = selector.role.RoleName,
                            UserId = selector.user.Id,
                            SiteId = selector.workingSite.SiteId
                        }).ToListAsync();
            return new PagedResult<UserWorkingSiteModel>(data, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);
        }

        public async Task<bool> InsertWorkingSite(InternalUserWorkingSite internalUserWorkingSite)
        {
            await context.AddAsync(internalUserWorkingSite);
            await Update();
            return true;
        }
    }
}
