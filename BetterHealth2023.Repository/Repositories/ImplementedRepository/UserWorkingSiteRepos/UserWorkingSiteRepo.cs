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

        public async Task<bool> InsertWorkingSite(InternalUserWorkingSite internalUserWorkingSite)
        {
            await context.AddAsync(internalUserWorkingSite);
            await Update();
            return true;
        }
    }
}
