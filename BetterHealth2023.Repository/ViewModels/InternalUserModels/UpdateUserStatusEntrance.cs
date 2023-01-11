using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels
{
    public class UpdateUserStatusEntrance
    {
        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Bật hoạt động: 1, Tắt hoạt động: 0")]
        public int Status { get; set; }
    }
}
