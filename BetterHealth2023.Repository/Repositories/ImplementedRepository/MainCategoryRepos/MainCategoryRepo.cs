using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.MainCategoryRepos
{
    public class MainCategoryRepo : RepositorySecond<CategoryMain, MainCategoryViewModel>, IMainCategoryRepo
    {
        public MainCategoryRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public Task<bool> UpdateStatus(string id, bool status)
        {
            throw new NotImplementedException();
        }
    }
}
