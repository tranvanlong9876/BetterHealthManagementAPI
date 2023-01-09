using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class UpdateSiteModel
    {
        public string SiteID { get; set; }
        
        public string SiteName { get; set; }

        public string Description { get; set; }

        public string ContactInfo { get; set; }

        public DynamicAddress DynamicAddress { get; set; }
    }
}
