using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UnitRepos
{
    public interface IUnitRepo : IRepository<Unit>
    {
        public Task<PagedResult<ViewUnitModel>> GetAll(GetUnitPagingModel pagingModel);
        public Task<bool> CheckExistUnitName(string unitName);
        public Task<bool> CheckExistUnitNameUpdate(string id, string unitName);

    }
}
