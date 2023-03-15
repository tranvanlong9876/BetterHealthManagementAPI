using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels
{
    public class UserInformation
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public string SiteId { get; set; }
        public string SiteCityId { get; set; }
        public string SiteDistrictId { get; set; }
        public string SiteWardId { get; set; }
        public string UserAccessToken { get; set; }
    }
}
