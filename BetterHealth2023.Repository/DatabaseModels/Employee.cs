using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Employee")]
    public partial class Employee
    {
        public Employee()
        {
            Blogs = new HashSet<Blog>();
            OrderHeaders = new HashSet<OrderHeader>();
            WarehouseEntryReceipts = new HashSet<WarehouseEntryReceipt>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
        [Required]
        [StringLength(200)]
        public string PasswordSalt { get; set; }
        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }
        [Required]
        [Column("RoleID")]
        [StringLength(50)]
        public string RoleId { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNo { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [Column("ImageURL")]
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [Required]
        [Column("SiteID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        public int Status { get; set; }
        [Column("AddressID")]
        [StringLength(50)]
        public string AddressId { get; set; }
        [Column("DOB", TypeName = "date")]
        public DateTime? Dob { get; set; }
        public int? Gender { get; set; }

        [ForeignKey(nameof(AddressId))]
        [InverseProperty(nameof(DynamicAddress.Employees))]
        public virtual DynamicAddress Address { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(RoleInternal.Employees))]
        public virtual RoleInternal Role { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.Employees))]
        public virtual SiteInformation Site { get; set; }
        [InverseProperty(nameof(Blog.Employee))]
        public virtual ICollection<Blog> Blogs { get; set; }
        [InverseProperty(nameof(OrderHeader.Employee))]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
        [InverseProperty(nameof(WarehouseEntryReceipt.Employee))]
        public virtual ICollection<WarehouseEntryReceipt> WarehouseEntryReceipts { get; set; }
    }
}
