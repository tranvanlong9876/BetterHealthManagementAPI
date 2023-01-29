using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class LoginCustomerModel
    {
        [Required]
        public string phoneNo { get; set; }

        [Required]
        [StringLength(maximumLength: 6, MinimumLength = 6, ErrorMessage = "Mã OTP phải đầy đủ 6 số.")]
        public string otpCode { get; set; }
    }
}
