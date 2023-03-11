using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Site_Inventory_Batch")]
    public partial class SiteInventoryBatch
    {
        public SiteInventoryBatch()
        {
            OrderBatches = new HashSet<OrderBatch>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Site_ID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Column("Product_ID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        [Column("Import_Batch_ID")]
        [StringLength(50)]
        public string ImportBatchId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedDate { get; set; }

        [ForeignKey(nameof(ImportBatchId))]
        [InverseProperty(nameof(ProductImportBatch.SiteInventoryBatches))]
        public virtual ProductImportBatch ImportBatch { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.SiteInventoryBatches))]
        public virtual ProductDetail Product { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.SiteInventoryBatches))]
        public virtual SiteInformation Site { get; set; }
        [InverseProperty(nameof(OrderBatch.SiteInventoryBatch))]
        public virtual ICollection<OrderBatch> OrderBatches { get; set; }
    }
}
