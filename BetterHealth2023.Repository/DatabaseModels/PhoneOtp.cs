using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("PhoneOTP")]
    public partial class PhoneOtp
    {
        [Key]
        [StringLength(50)]
        public string PhoneNo { get; set; }
        [Required]
        [Column("OTP_Code")]
        [StringLength(10)]
        public string OtpCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpireDate { get; set; }
    }
}
