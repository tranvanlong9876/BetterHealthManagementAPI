using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Site_Information")]
    public partial class SiteInformation
    {
        public SiteInformation()
        {
            InternalUserWorkingSites = new HashSet<InternalUserWorkingSite>();
            OrderHeaders = new HashSet<OrderHeader>();
            ProductImportReceipts = new HashSet<ProductImportReceipt>();
            SiteInventoryBatches = new HashSet<SiteInventoryBatch>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("ImageURL")]
        [StringLength(200)]
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(100)]
        public string SiteName { get; set; }
        [Required]
        [Column("Address_ID")]
        [StringLength(50)]
        public string AddressId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastUpdate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ContactInfo { get; set; }
        [Column("isActivate")]
        public bool IsActivate { get; set; }
        [Column("isDelivery")]
        public bool IsDelivery { get; set; }

        [ForeignKey(nameof(AddressId))]
        [InverseProperty(nameof(DynamicAddress.SiteInformations))]
        public virtual DynamicAddress Address { get; set; }
        [InverseProperty(nameof(InternalUserWorkingSite.Site))]
        public virtual ICollection<InternalUserWorkingSite> InternalUserWorkingSites { get; set; }
        [InverseProperty(nameof(OrderHeader.Site))]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
        [InverseProperty(nameof(ProductImportReceipt.Site))]
        public virtual ICollection<ProductImportReceipt> ProductImportReceipts { get; set; }
        [InverseProperty(nameof(SiteInventoryBatch.Site))]
        public virtual ICollection<SiteInventoryBatch> SiteInventoryBatches { get; set; }
    }
}
