using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Role_Internal")]
    public partial class RoleInternal
    {
        public RoleInternal()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(70)]
        public string RoleName { get; set; }

        [InverseProperty(nameof(Employee.Role))]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
