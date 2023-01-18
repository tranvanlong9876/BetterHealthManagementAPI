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
        public async Task<List<MainCategoryViewModel>> GetAll()
        {
            return await _mainCategoryRepo.GetAll<MainCategoryViewModel>();
        }
    }
}
