using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("ProductImport_Details")]
    public partial class ProductImportDetail
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
        public virtual ProductImportBatch ProductNavigation { get; set; }
        [ForeignKey(nameof(ReceiptId))]
        public virtual ProductImportReceipt Receipt { get; set; }
    }
}
