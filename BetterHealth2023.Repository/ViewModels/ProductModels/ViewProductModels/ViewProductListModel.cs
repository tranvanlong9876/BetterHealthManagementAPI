using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ViewProductListModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string SubCategoryId { get; set; }

        public string ManufacturerId { get; set; }

        public bool IsPrescription { get; set; }

        public bool IsBatches { get; set; }

        public string UnitId { get; set; }
        public int UnitLevel { get; set; }
        public int Quantitative { get; set; }
        public int SellQuantity { get; set; }
        public double Price { get; set; }
        public double PriceAfterDiscount { get; set; }
        public bool IsSell { get; set; }

        public string BarCode { get; set; }
        public ProductImageView imageModel { get; set; }
        public ProductDiscountViewList discountModel { get; set; }

    }

    public class ProductImageView
    {
        public string Id { get; set; }
        public string ImageURL { get; set; }
    }

    public class ProductDiscountViewList
    {
        public string Title { get; set; }
        public string Reason { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountMoney { get; set; }

    }

    public class ViewProductListModelForInternal : ViewProductListModel
    {
        public List<ProductUnitModel> productUnitReferences { get; set; }

        public ProductInventoryModel productInventoryModel { get; set; }
    }

    public class ProductInventoryModel
    {
        public int Quantity { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
    }

}
