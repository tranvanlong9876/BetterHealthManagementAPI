using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Category_Main")]
    public partial class CategoryMain
    {
        public CategoryMain()
        {
            SubCategories = new HashSet<SubCategory>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        [Column("ImageURL")]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(SubCategory.MainCategory))]
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
