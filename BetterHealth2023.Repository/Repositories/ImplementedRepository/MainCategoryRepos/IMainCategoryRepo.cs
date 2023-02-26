using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.MainCategoryRepos
{
    public interface IMainCategoryRepo : IRepository<CategoryMain>
    {
        public Task<PagedResult<MainCategoryViewModel>> GetAllPaging(MainCategoryPagingRequest pagingRequest);

        public Task<bool> UpdateStatus(string id, bool status);
    }
}
