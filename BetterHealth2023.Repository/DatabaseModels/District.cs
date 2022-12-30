using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("District")]
    public partial class District
    {
        public District()
        {
            DynamicAddresses = new HashSet<DynamicAddress>();
            Wards = new HashSet<Ward>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("District_Name")]
        [StringLength(100)]
        public string DistrictName { get; set; }
        [Required]
        [Column("City_ID")]
        [StringLength(50)]
        public string CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Districts")]
        public virtual City City { get; set; }
        [InverseProperty(nameof(DynamicAddress.District))]
        public virtual ICollection<DynamicAddress> DynamicAddresses { get; set; }
        [InverseProperty(nameof(Ward.District))]
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
