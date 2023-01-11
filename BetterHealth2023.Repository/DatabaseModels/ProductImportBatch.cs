﻿using System;
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
            OrderBatches = new HashSet<OrderBatch>();
        }

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
        [Column("isOutOfStock")]
        public bool IsOutOfStock { get; set; }

        [ForeignKey(nameof(ReceiptId))]
        [InverseProperty(nameof(ProductImportReceipt.ProductImportBatches))]
        public virtual ProductImportReceipt Receipt { get; set; }
        [InverseProperty(nameof(OrderBatch.Batch))]
        public virtual ICollection<OrderBatch> OrderBatches { get; set; }
    }
}