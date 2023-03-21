using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels
{
    public class ViewSiteToPickUpStatus
    {
        public bool isError { get; set; }
        public string errorConvert { get; set; }

        public SiteModelToPickUp siteListPickUp { get; set; }
    }

    public class SiteModelToPickUp
    {
        public int totalSite { get; set; }
        public List<SiteListToPickUp> siteListToPickUps { get; set; }
    }

    public class SiteListToPickUp
    {
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string HomeAddress { get; set; }
        public string AddressId { get; set; }
        public string FullyAddress { get; set; }
    }
    
}
