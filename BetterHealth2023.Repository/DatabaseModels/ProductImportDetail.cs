using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("ProductImport_Details")]
    public partial class ProductImportDetail
    {
        public ProductImportDetail()
        {
            ProductImportBatches = new HashSet<ProductImportBatch>();
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
        [Column("importPrice", TypeName = "money")]
        public decimal ImportPrice { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.ProductImportDetails))]
        public virtual ProductDetail Product { get; set; }
        [ForeignKey(nameof(ReceiptId))]
        [InverseProperty(nameof(ProductImportReceipt.ProductImportDetails))]
        public virtual ProductImportReceipt Receipt { get; set; }
        [InverseProperty(nameof(ProductImportBatch.ImportDetail))]
        public virtual ICollection<ProductImportBatch> ProductImportBatches { get; set; }
    }
}
