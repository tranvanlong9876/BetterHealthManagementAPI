using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SubCategoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.SubCategoryService
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepo _subCategoryRepo;

        public SubCategoryService(ISubCategoryRepo subCategoryRepo)
        {
            _subCategoryRepo = subCategoryRepo;
        }
        public async Task<bool> Create(CreateSubCategoryModel categoryModel)
        {
            var subID = Guid.NewGuid().ToString();

            var subCateDBModel = _subCategoryRepo.TransferBetweenTwoModels<CreateSubCategoryModel, SubCategory>(categoryModel);
            subCateDBModel.Id = subID;

            return await _subCategoryRepo.Insert(subCateDBModel);

        }

        public async Task<SubCategoryViewModel> Get(string id)
        {
            return await _subCategoryRepo.GetViewModel<SubCategoryViewModel>(id);
        }

        public async Task<PagedResult<SubCategoryViewModel>> GetAll(GetSubCategoryPagingRequest pagingRequest)
        {
            return await _subCategoryRepo.GetAllSubCategory(pagingRequest);
        }

        public async Task<bool> Update(UpdateSubCategoryModel categoryModel)
        {
            var categoryDBModel = await _subCategoryRepo.Get(categoryModel.Id);
            categoryDBModel.ImageUrl = categoryModel.ImageUrl;
            categoryDBModel.MainCategoryId = categoryModel.MainCategoryId;
            categoryDBModel.SubCategoryName = categoryModel.SubCategoryName;

            return await _subCategoryRepo.Update();
        }
    }
}
