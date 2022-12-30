using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Manufacturer")]
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Manufacturer_Name")]
        [StringLength(500)]
        public string ManufacturerName { get; set; }

        [InverseProperty(nameof(Product.Manufacturer))]
        public virtual ICollection<Product> Products { get; set; }
    }
}
