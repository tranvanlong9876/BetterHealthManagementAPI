using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.MainCategoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.MainCategoryService
{
    public class MainCategoryService : IMainCategoryService
    {
        private readonly IMainCategoryRepo _mainCategoryRepo;

        public MainCategoryService(IMainCategoryRepo mainCategoryRepo)
        {
            _mainCategoryRepo = mainCategoryRepo;
        }

        public async Task<bool> Create(CreateCategoryModel createCategoryModel)
        {
            //swap model
            var id = Guid.NewGuid().ToString();

            var categoryMain = _mainCategoryRepo.TransferBetweenTwoModels<CreateCategoryModel, CategoryMain>(createCategoryModel);
            categoryMain.Id = id;
            var check = await _mainCategoryRepo.Insert(categoryMain);
            return check;
        }

        public async Task<MainCategoryViewModel> Get(string id)
        {
            return await _mainCategoryRepo.GetViewModel<MainCategoryViewModel>(id);
        }

        public async Task<List<MainCategoryViewModel>> GetAll()
        {
            return await _mainCategoryRepo.GetAll<MainCategoryViewModel>();
        }

        public async Task<bool> Update(UpdateCategoryModel updateCategoryModel)
        {
            var categoryMain = await _mainCategoryRepo.Get(updateCategoryModel.Id);
            categoryMain.CategoryName = updateCategoryModel.CategoryName;
            categoryMain.ImageUrl = updateCategoryModel.ImageUrl;
            
            return await _mainCategoryRepo.Update();
        }
    }
}
