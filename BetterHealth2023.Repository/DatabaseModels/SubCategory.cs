using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Sub_Category")]
    public partial class SubCategory
    {
        public SubCategory()
        {
            ProductParents = new HashSet<ProductParent>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string SubCategoryName { get; set; }
        [Required]
        [Column("Main_CategoryID")]
        [StringLength(50)]
        public string MainCategoryId { get; set; }
        [Column("ImageURL")]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(MainCategoryId))]
        [InverseProperty(nameof(CategoryMain.SubCategories))]
        public virtual CategoryMain MainCategory { get; set; }
        [InverseProperty(nameof(ProductParent.SubCategory))]
        public virtual ICollection<ProductParent> ProductParents { get; set; }
    }
}
