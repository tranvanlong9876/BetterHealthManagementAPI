using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Unit")]
    public partial class Unit
    {
        public Unit()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Unit_Name")]
        [StringLength(100)]
        public string UnitName { get; set; }

        [InverseProperty(nameof(ProductDetail.Unit))]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
