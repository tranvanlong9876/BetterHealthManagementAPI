using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels
{
    public class UpdateInternalUser
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
        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string CityID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string DistrictID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string WardID { get; set; }
        [StringLength(300)]
        [Required(ErrorMessage = "Cập nhật thông tin phải có đầy đủ địa chỉ.")]
        public string HomeNumber { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DOB { get; set; }

        [Required]
        public bool isChangePassword { get; set; }

        [StringLength(200)]
        public string OldPassword { get; set; }

        [StringLength(200)]
        public string NewPassword { get; set; }

        [StringLength(200)]
        public string ConfirmPassword { get; set; }

        [Range(0, 2, ErrorMessage = "Dữ liệu Status phải là 0 -> 1. 0 là không công khai giới tính, 1 là Nam, 2 là Nữ.")]
        public int Gender { get; set; }
    }
}
