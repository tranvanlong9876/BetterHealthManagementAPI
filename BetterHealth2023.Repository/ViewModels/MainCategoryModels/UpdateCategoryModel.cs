using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.MainCategoryModels
{
    public class UpdateCategoryModel
    {
        [Required]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }
    }
}
