using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.RoleRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.RoleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalRole
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _roleRepo;

        public RoleService(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<List<RoleViewModel>> GetRoleList()
        {
            var roleList = await _roleRepo.GetAllRole();
            
            return roleList;
        }
    }
}
