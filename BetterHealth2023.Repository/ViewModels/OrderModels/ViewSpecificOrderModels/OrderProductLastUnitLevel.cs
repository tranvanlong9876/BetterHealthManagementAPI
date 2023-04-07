using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels
{
    public class OrderProductLastUnitLevel
    {
        public string productId { get; set; }
        public int productQuantity { get; set; }

        public string UnitName { get; set; }
    }
}
