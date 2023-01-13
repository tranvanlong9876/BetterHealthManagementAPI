using System.ComponentModel.DataAnnotations;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class UpdateStatusSiteModel
    {
        [Required]
        [StringLength(50)]
        public string SiteID { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}