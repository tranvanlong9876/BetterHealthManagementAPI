using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SiteInventoryModels
{
    public class ExistProductModel
    {
        public string productId { get; set; }
        public int Quantity { get; set; }
        public bool checkResultIsEnough { get; set; }
    }
}
