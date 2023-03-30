using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels
{
    public class ViewListExportProductModel
    {
        public string SiteInventoryId { get; set; }
        public string BatchId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Status { get; set; } = "Đã quá hạn sử dụng";

    }
}
