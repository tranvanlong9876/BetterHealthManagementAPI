using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("ProductImport_Receipt")]
    public partial class ProductImportReceipt
    {
        public ProductImportReceipt()
        {
            ProductImportDetails = new HashSet<ProductImportDetail>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Site_ID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Column("Manager_ID")]
        [StringLength(50)]
        public string ManagerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ImportDate { get; set; }
        [Required]
        public string Note { get; set; }
        [Column("totalProductPrice")]
        public double TotalProductPrice { get; set; }
        [Column("taxPrice")]
        public double TaxPrice { get; set; }
        [Column("totalShippingFee")]
        public double TotalShippingFee { get; set; }
        [Column("totalPrice")]
        public double TotalPrice { get; set; }
        [Column("isReleased")]
        public bool IsReleased { get; set; }

        [ForeignKey(nameof(ManagerId))]
        [InverseProperty(nameof(InternalUser.ProductImportReceipts))]
        public virtual InternalUser Manager { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.ProductImportReceipts))]
        public virtual SiteInformation Site { get; set; }
        [InverseProperty(nameof(ProductImportDetail.Receipt))]
        public virtual ICollection<ProductImportDetail> ProductImportDetails { get; set; }
    }
}
