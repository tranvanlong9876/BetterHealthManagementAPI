using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.UnitErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Unit
{
    public interface IUnitService
    {
        public Task<PagedResult<ViewUnitModel>> GetAll(GetUnitPagingModel pagingModel);
        public Task<ViewUnitModel> Get(string id);
        public Task<CreateUnitErrorModel> Insert(CreateUnitModel createUnitModel);
        public Task<UpdateUnitErrorModel> Update(UpdateUnitModel updateUnitModel);
    }
}
