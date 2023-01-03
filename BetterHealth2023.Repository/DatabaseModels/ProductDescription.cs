using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_Description")]
    public partial class ProductDescription
    {
        public ProductDescription()
        {
            ProductParents = new HashSet<ProductParent>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        public string Effect { get; set; }
        public string Instruction { get; set; }
        public string SideEffect { get; set; }
        public string Contraindications { get; set; }
        public string Preserve { get; set; }

        [InverseProperty(nameof(ProductParent.DrugDescription))]
        public virtual ICollection<ProductParent> ProductParents { get; set; }
    }
}
