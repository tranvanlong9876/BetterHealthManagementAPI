using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_Batches")]
    public partial class OrderBatch
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Site_InventoryBatch_ID")]
        [StringLength(50)]
        public string SiteInventoryBatchId { get; set; }
        [Required]
        [Column("Order_ID")]
        [StringLength(50)]
        public string OrderId { get; set; }
        public int SoldQuantity { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderBatches))]
        public virtual OrderHeader Order { get; set; }
        [ForeignKey(nameof(SiteInventoryBatchId))]
        [InverseProperty("OrderBatches")]
        public virtual SiteInventoryBatch SiteInventoryBatch { get; set; }
    }
}
