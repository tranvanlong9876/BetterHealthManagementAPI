using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.MainCategoryService
{
    public interface IMainCategoryService
    {
        public Task<List<MainCategoryViewModel>> GetAll();

        public Task<MainCategoryViewModel> Get(string id);
        public Task<bool> Create(CreateCategoryModel createCategoryModel);
        public Task<bool> Update(UpdateCategoryModel updateCategoryModel);
    }
}
