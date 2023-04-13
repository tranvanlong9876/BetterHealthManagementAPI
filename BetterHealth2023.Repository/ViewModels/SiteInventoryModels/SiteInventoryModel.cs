using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SiteInventoryModels
{
    public class SiteInventoryModel
    {
        public int TotalQuantity { get; set; }
        public int TotalQuantityForFirst { get; set; }

        public string Message { get; set; }
    }
}
