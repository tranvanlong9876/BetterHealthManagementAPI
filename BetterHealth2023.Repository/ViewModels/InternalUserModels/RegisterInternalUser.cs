using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels
{
    public class RegisterInternalUser
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNo { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string CityID { get; set; }
        [StringLength(50)]
        public string DistrictID { get; set; }
        [StringLength(50)]
        public string WardID { get; set; }
        [StringLength(300)]
        public string HomeNumber { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DOB { get; set; }
        [Required]
        [StringLength(50)]
        public string RoleId { get; set; }
        [StringLength(50)]
        public string SiteId { get; set; }

        [JsonIgnore]
        public string SiteName { get; set; }

        [JsonIgnore]
        public string SiteAddress { get; set; }

        [Range(0, 1, ErrorMessage = "Dữ liệu Gender phải là 0 -> 1. 0 là Nữ, 1 là Nam.")]
        public int Gender { get; set; }
    }
}
