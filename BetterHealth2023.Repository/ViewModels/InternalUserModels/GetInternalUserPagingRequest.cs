using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels
{
    public class GetInternalUserPagingRequest : PagingRequestBase
    {
        public int? UserStatus { get; set; }
        public string RoleID { get; set; }
        public string FullName { get; set; }
    }
}
