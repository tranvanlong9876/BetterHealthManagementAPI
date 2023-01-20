using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels
{
    public class UpdateSubCategoryModel
    {
        [Required]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        public string SubCategoryName { get; set; }
        [Required]
        public string MainCategoryId { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
