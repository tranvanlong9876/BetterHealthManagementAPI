using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ExportProductModels
{
    public class ExportDamageProductEntrance
    {
        /// <summary>
        /// Mã Site Inventory cần export.
        /// </summary>
        public string SiteInventoryId { get; set; }
        /// <summary>
        /// Số lượng cần export/hủy sản phẩm.
        /// </summary>
        public int ExportQuantity { get; set; }
    }
}
