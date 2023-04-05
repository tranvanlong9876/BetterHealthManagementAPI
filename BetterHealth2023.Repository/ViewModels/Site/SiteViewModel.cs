using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class SiteViewModel
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string SiteName { get; set; }
        public string AddressId { get; set; }
        public string FullyAddress { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Description { get; set; }
        public string ContactInfo { get; set; }
        public bool IsActivate { get; set; }
        public bool IsDelivery { get; set; }
    }
}
