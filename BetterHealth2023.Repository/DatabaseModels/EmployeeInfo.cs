using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("Employee_Info")]
    public partial class EmployeeInfo
    {
        [Required]
        [Column("Employee_ID")]
        [StringLength(50)]
        public string EmployeeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNo { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Column("Address_ID")]
        [StringLength(50)]
        public string AddressId { get; set; }
        [Required]
        [StringLength(100)]
        public string ImageUrl { get; set; }
        [Column("DOB", TypeName = "date")]
        public DateTime? Dob { get; set; }

        [ForeignKey(nameof(AddressId))]
        public virtual DynamicAddress Address { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }
}
