using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Shelves_Management")]
    public partial class ShelvesManagement
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Product_ID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        [Required]
        [Column("Site_ID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdateDate { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.ShelvesManagements))]
        public virtual ProductDetail Product { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.ShelvesManagements))]
        public virtual SiteInformation Site { get; set; }
    }
}
