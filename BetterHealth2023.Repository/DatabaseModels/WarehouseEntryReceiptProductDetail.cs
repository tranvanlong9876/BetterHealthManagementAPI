using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("WarehouseEntry_Receipt_ProductDetails")]
    public partial class WarehouseEntryReceiptProductDetail
    {
        [Required]
        [Column("Receipt_ID")]
        [StringLength(50)]
        public string ReceiptId { get; set; }
        [Required]
        [Column("Product_ID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        [Column("importPrice", TypeName = "money")]
        public decimal ImportPrice { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual ProductDetail Product { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual WarehouseEntryReceiptProductBatch ProductNavigation { get; set; }
        [ForeignKey(nameof(ReceiptId))]
        public virtual WarehouseEntryReceipt Receipt { get; set; }
    }
}
