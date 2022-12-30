using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Ward")]
    public partial class Ward
    {
        public Ward()
        {
            DynamicAddresses = new HashSet<DynamicAddress>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Ward_Name")]
        [StringLength(100)]
        public string WardName { get; set; }
        [Required]
        [Column("District_ID")]
        [StringLength(50)]
        public string DistrictId { get; set; }

        [ForeignKey(nameof(DistrictId))]
        [InverseProperty("Wards")]
        public virtual District District { get; set; }
        [InverseProperty(nameof(DynamicAddress.Ward))]
        public virtual ICollection<DynamicAddress> DynamicAddresses { get; set; }
    }
}
