﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Dynamic_Address")]
    public partial class DynamicAddress
    {
        public DynamicAddress()
        {
            Employees = new HashSet<Employee>();
            SiteInformations = new HashSet<SiteInformation>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Column("City_ID")]
        [StringLength(50)]
        public string CityId { get; set; }
        [Column("District_ID")]
        [StringLength(50)]
        public string DistrictId { get; set; }
        [Column("Ward_ID")]
        [StringLength(50)]
        public string WardId { get; set; }
        [StringLength(300)]
        public string HomeAddress { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("DynamicAddresses")]
        public virtual City City { get; set; }
        [ForeignKey(nameof(DistrictId))]
        [InverseProperty("DynamicAddresses")]
        public virtual District District { get; set; }
        [ForeignKey(nameof(WardId))]
        [InverseProperty("DynamicAddresses")]
        public virtual Ward Ward { get; set; }
        [InverseProperty(nameof(Employee.Address))]
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty(nameof(SiteInformation.Address))]
        public virtual ICollection<SiteInformation> SiteInformations { get; set; }
    }
}
