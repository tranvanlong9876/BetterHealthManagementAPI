using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DOB { get; set; }
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
        [Required]
        [StringLength(200)]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(50)]
        public string RoleId { get; set; }
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Dữ liệu Status phải là 0 hoặc 1. 1 là Nhân viên hoạt động, 0 là ngưng hoạt động")]
        public int Status { get; set; }

        [Range(0, 2, ErrorMessage = "Dữ liệu Status phải là 0 -> 1. 0 là không công khai giới tính, 1 là Nam, 2 là Nữ.")]
        public int Gender { get; set; }
    }
}
