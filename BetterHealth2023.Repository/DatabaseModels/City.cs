using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("City")]
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
            DynamicAddresses = new HashSet<DynamicAddress>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("City_Name")]
        [StringLength(100)]
        public string CityName { get; set; }

        [InverseProperty(nameof(District.City))]
        public virtual ICollection<District> Districts { get; set; }
        [InverseProperty(nameof(DynamicAddress.City))]
        public virtual ICollection<DynamicAddress> DynamicAddresses { get; set; }
    }
}
