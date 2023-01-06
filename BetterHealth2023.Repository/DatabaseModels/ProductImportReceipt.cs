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
            ProductImportBatches = new HashSet<ProductImportBatch>();
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
        [Column("totalPrice", TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Column("isReleased")]
        public bool IsReleased { get; set; }

        [ForeignKey(nameof(ManagerId))]
        [InverseProperty(nameof(InternalUser.ProductImportReceipts))]
        public virtual InternalUser Manager { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.ProductImportReceipts))]
        public virtual SiteInformation Site { get; set; }
        [InverseProperty(nameof(ProductImportBatch.Receipt))]
        public virtual ICollection<ProductImportBatch> ProductImportBatches { get; set; }
    }
}
