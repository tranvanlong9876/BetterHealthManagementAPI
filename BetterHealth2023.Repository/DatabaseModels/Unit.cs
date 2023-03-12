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
            ProductIngredientDescriptions = new HashSet<ProductIngredientDescription>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Unit_Name")]
        [StringLength(100)]
        public string UnitName { get; set; }
        [Column("isCountable")]
        public bool IsCountable { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }

        [InverseProperty(nameof(ProductDetail.Unit))]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        [InverseProperty(nameof(ProductIngredientDescription.Unit))]
        public virtual ICollection<ProductIngredientDescription> ProductIngredientDescriptions { get; set; }
    }
}
