using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class UserWorkingSiteModel
    {
        public string Id { get; set; }
        public string SiteId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }

        public string RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
