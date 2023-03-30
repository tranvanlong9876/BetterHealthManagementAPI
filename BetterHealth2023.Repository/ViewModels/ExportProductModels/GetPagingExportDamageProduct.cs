using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels
{
    public class GetPagingExportDamageProduct : PagingRequestBase
    {
        [Required]
        public string SiteId { get; set; }
        public string ProductName { get; set; }
    }
}
