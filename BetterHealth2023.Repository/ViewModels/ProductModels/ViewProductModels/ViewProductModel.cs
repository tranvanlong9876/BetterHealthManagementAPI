using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ViewProductModel
    {

        public string Name { get; set; }

        public string NameWithUnit { get; set; }

        public string SubCategoryId { get; set; }

        public string ManufacturerId { get; set; }

        public bool IsPrescription { get; set; }

        public bool IsBatches { get; set; }

        public string UnitId { get; set; }
        public int UnitLevel { get; set; }
        public int Quantitative { get; set; }
        public int SellQuantity { get; set; }
        public double Price { get; set; }
        public bool IsSell { get; set; }
        public List<string> imageURL { get; set; }

    }
}
