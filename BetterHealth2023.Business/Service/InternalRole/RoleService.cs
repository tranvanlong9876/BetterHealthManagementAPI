using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
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

        public async Task<List<RoleViewModel>> GetRoleList(RoleFilterRequest filterRequest)
        {
            var roleList = await _roleRepo.GetAllRole();

            if (filterRequest.LoadWorkingRoleOnly.HasValue)
            {
                if (filterRequest.LoadWorkingRoleOnly.Value)
                {
                    roleList.Remove(roleList.Find(x => x.RoleName.Equals(Commons.ADMIN_NAME)));
                    roleList.Remove(roleList.Find(x => x.RoleName.Equals(Commons.OWNER_NAME)));
                }
                else
                {
                    roleList.Remove(roleList.Find(x => x.RoleName.Equals(Commons.PHARMACIST_NAME)));
                    roleList.Remove(roleList.Find(x => x.RoleName.Equals(Commons.MANAGER_NAME)));
                }
            }

            return roleList;
        }
    }
}
