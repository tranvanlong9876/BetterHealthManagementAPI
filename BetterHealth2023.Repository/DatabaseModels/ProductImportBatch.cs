using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("ProductImport_Batches")]
    public partial class ProductImportBatch
    {
        public ProductImportBatch()
        {
            SiteInventoryBatches = new HashSet<SiteInventoryBatch>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("ImportDetail_ID")]
        [StringLength(50)]
        public string ImportDetailId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "date")]
        public DateTime ManufactureDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ExpireDate { get; set; }

        [ForeignKey(nameof(ImportDetailId))]
        [InverseProperty(nameof(ProductImportDetail.ProductImportBatches))]
        public virtual ProductImportDetail ImportDetail { get; set; }
        [InverseProperty(nameof(SiteInventoryBatch.ImportBatch))]
        public virtual ICollection<SiteInventoryBatch> SiteInventoryBatches { get; set; }
    }
}
