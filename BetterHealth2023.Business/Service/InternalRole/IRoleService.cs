using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.RoleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalRole
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetRoleList(RoleFilterRequest filterRequest);
    }
}
