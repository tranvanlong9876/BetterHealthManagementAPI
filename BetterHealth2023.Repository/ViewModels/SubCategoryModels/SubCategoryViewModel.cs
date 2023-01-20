using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels
{
    public class SubCategoryViewModel
    {
        public string Id { get; set; }
        public string SubCategoryName { get; set; }
        public string MainCategoryId { get; set; }
        public string MainCategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
