using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("Product_Batch_Warehouse")]
    public partial class ProductBatchWarehouse
    {
        [Required]
        [Column("Warehouse_ID")]
        [StringLength(50)]
        public string WarehouseId { get; set; }
        [Required]
        [Column("Batch_ID")]
        [StringLength(50)]
        public string BatchId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(BatchId))]
        public virtual WarehouseEntryReceiptProductBatch Batch { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public virtual WarehouseManagement Warehouse { get; set; }
    }
}
