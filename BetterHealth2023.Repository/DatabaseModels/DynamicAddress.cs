using System;
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
            CustomerAddresses = new HashSet<CustomerAddress>();
            InternalUsers = new HashSet<InternalUser>();
            OrderContactInfos = new HashSet<OrderContactInfo>();
            OrderShipmentDestinationAddresses = new HashSet<OrderShipment>();
            OrderShipmentStartAddresses = new HashSet<OrderShipment>();
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
        [InverseProperty(nameof(CustomerAddress.Address))]
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        [InverseProperty(nameof(InternalUser.Address))]
        public virtual ICollection<InternalUser> InternalUsers { get; set; }
        [InverseProperty(nameof(OrderContactInfo.Address))]
        public virtual ICollection<OrderContactInfo> OrderContactInfos { get; set; }
        [InverseProperty(nameof(OrderShipment.DestinationAddress))]
        public virtual ICollection<OrderShipment> OrderShipmentDestinationAddresses { get; set; }
        [InverseProperty(nameof(OrderShipment.StartAddress))]
        public virtual ICollection<OrderShipment> OrderShipmentStartAddresses { get; set; }
        [InverseProperty(nameof(SiteInformation.Address))]
        public virtual ICollection<SiteInformation> SiteInformations { get; set; }
    }
}
