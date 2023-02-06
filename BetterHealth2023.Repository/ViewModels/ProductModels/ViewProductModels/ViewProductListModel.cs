﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ViewProductListModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string NameWithUnit { get; set; }
        public string TotalUnitOnly { get; set; }

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
        public List<ProductImageView> imageModels { get; set; }

    }

    public class ProductImageView
    {
        public string Id { get; set; }
        public string ImageURL { get; set; }
    }
}
