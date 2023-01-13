using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class SiteEntranceModels
    {

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
