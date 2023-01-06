using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UserWorkingSiteRepos
{
    public interface IUserWorkingSiteRepo : IRepository<InternalUserWorkingSite>
    {
        public Task<List<string>> getInternalUserIDOfWorkingSite(string siteID);
    }
}
