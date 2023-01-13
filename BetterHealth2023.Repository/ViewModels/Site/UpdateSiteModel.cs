using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using System.ComponentModel.DataAnnotations;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class UpdateSiteModel
    {
        [Required]
        [StringLength(50)]
        public string SiteID { get; set; }
        [Required]
        [StringLength(100)]
        public string SiteName { get; set; }
        [Required]
        [StringLength(4000)]
        public string Description { get; set; }
        [Required]
        [StringLength(4000)]
        public string ContactInfo { get; set; }
        [Required]
        [StringLength(200)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string cityId { get; set; }

        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string districtId { get; set; }

        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string wardId { get; set; }
        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string homeAddress { get; set; }
    }
}
