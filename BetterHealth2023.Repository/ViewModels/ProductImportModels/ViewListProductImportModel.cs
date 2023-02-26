using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class ViewListProductImportModel
    {
        public string Id { get; set; }
        public string SiteId { get; set; }

        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public DateTime ImportDate { get; set; }
        public double TotalPrice { get; set; }
        public bool IsReleased { get; set; }
    }
}
