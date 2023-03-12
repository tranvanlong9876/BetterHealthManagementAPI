using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.RoleModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static System.Linq.Queryable;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.RoleRepos
{
    public class RoleRepo : Repository<RoleInternal>, IRoleRepo
    {
        public RoleRepo(BetterHealthManagementContext context) : base(context)
        {
        }

        public async Task<List<RoleViewModel>> GetAllRole()
        {
            var query = from x in context.RoleInternals
                        select new { x };

            var roleView = await query.Select(selector => new RoleViewModel() { 
                RoleID = selector.x.Id,
                RoleName = selector.x.RoleName
            }).ToListAsync();

            return roleView;
        }
    }
}
