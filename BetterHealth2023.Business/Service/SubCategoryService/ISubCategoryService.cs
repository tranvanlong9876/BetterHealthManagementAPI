using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.SubCategoryService
{
    public interface ISubCategoryService
    {
        public Task<PagedResult<SubCategoryViewModel>> GetAll(GetSubCategoryPagingRequest pagingRequest);
        public Task<SubCategoryViewModel> Get(string id);
        public Task<bool> Create(CreateSubCategoryModel categoryModel);
        public Task<bool> Update(UpdateSubCategoryModel categoryModel);
    }
}
