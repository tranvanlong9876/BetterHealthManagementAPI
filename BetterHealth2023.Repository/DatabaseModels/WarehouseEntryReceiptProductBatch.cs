using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("WarehouseEntry_Receipt_ProductBatches")]
    public partial class WarehouseEntryReceiptProductBatch
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Receipt_ID")]
        [StringLength(50)]
        public string ReceiptId { get; set; }
        [Required]
        [Column("Product_ID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "date")]
        public DateTime ManufactureDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ExpireDate { get; set; }

        [ForeignKey(nameof(ReceiptId))]
        [InverseProperty(nameof(WarehouseEntryReceipt.WarehouseEntryReceiptProductBatches))]
        public virtual WarehouseEntryReceipt Receipt { get; set; }
    }
}
