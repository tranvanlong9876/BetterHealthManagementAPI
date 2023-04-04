using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Product_UserTarget")]
    public partial class ProductUserTarget
    {
        public ProductUserTarget()
        {
            ProductParents = new HashSet<ProductParent>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string UserTargetName { get; set; }

        [InverseProperty(nameof(ProductParent.UserTargetNavigation))]
        public virtual ICollection<ProductParent> ProductParents { get; set; }
    }
}
