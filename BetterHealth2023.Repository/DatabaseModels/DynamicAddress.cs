using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Dynamic_Address")]
    public partial class DynamicAddress
    {
        public DynamicAddress()
        {
            InternalUsers = new HashSet<InternalUser>();
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
        [JsonIgnore]
        [ForeignKey(nameof(CityId))]
        [InverseProperty("DynamicAddresses")]
        public virtual City City { get; set; }
        [ForeignKey(nameof(DistrictId))]
        [InverseProperty("DynamicAddresses")]
        public virtual District District { get; set; }
        [ForeignKey(nameof(WardId))]
        [InverseProperty("DynamicAddresses")]
        public virtual Ward Ward { get; set; }
        [InverseProperty(nameof(InternalUser.Address))]
        public virtual ICollection<InternalUser> InternalUsers { get; set; }
        [InverseProperty(nameof(SiteInformation.Address))]
        public virtual ICollection<SiteInformation> SiteInformations { get; set; }
    }
}
