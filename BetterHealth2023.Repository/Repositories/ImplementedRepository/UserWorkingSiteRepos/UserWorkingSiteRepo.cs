using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                        where x.UserId.Trim().Equals(userID.Trim())
                        select new { x };
            var workingSite = await query.Select(selector => new InternalUserWorkingSite()).FirstOrDefaultAsync();

            if(workingSite != null)
            {
                return workingSite.SiteId;
            }
            return null;
        }

        public async Task<List<InternalUserWorkingSite>> GetTotalManager(string siteID)
        {
            var query = from x in context.InternalUserWorkingSites
                        join manager in context.InternalUsers on x.UserId equals manager.Id
                        where x.SiteId.Trim().Equals(siteID.Trim()) && manager.Role.Equals(Commons.Commons.MANAGER)
                        select new { x };
            var workingSite = await query.Select(selector => new InternalUserWorkingSite()).ToListAsync();

            return workingSite;
        }

        public async Task<List<InternalUserWorkingSite>> GetTotalPharmacist(string siteID)
        {
            var query = from x in context.InternalUserWorkingSites
                        join pharmacist in context.InternalUsers on x.UserId equals pharmacist.Id
                        where x.SiteId.Trim().Equals(siteID.Trim()) && pharmacist.Role.Equals(Commons.Commons.PHARMACIST)
                        select new { x };
            var workingSite = await query.Select(selector => new InternalUserWorkingSite()).ToListAsync();

            return workingSite;

        }

        public async Task<bool> InsertWorkingSite(InternalUserWorkingSite internalUserWorkingSite)
        {
            await context.AddAsync(internalUserWorkingSite);
            await Update();
            return true;
        }
    }
}
