using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Warehouse_Management")]
    public partial class WarehouseManagement
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Product_ID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        [Required]
        [Column("SiteID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdateDate { get; set; }
        [Required]
        [StringLength(10)]
        public string Quantity { get; set; }
        public int Status { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.WarehouseManagements))]
        public virtual ProductDetail Product { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.WarehouseManagements))]
        public virtual SiteInformation Site { get; set; }
    }
}
