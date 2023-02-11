using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ProductUnitModel
    {
        public string Id { get; set; }

        public string UnitId { get; set; }

        public string UnitName { get; set; }
        public int UnitLevel { get; set; }

        public int Quantitative { get; set; }
    }
}
