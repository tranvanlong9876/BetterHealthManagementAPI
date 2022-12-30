using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("WarehouseEntry_Receipt")]
    public partial class WarehouseEntryReceipt
    {
        public WarehouseEntryReceipt()
        {
            WarehouseEntryReceiptProductBatches = new HashSet<WarehouseEntryReceiptProductBatch>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Site_ID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Column("Employee_ID")]
        [StringLength(50)]
        public string EmployeeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ImportDate { get; set; }
        [Required]
        public string Note { get; set; }
        [Column("totalPrice", TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Column("isReleased")]
        public bool IsReleased { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty("WarehouseEntryReceipts")]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.WarehouseEntryReceipts))]
        public virtual SiteInformation Site { get; set; }
        [InverseProperty(nameof(WarehouseEntryReceiptProductBatch.Receipt))]
        public virtual ICollection<WarehouseEntryReceiptProductBatch> WarehouseEntryReceiptProductBatches { get; set; }
    }
}
