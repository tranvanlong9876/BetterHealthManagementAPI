using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System.Collections.Generic;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderValidateModels
{
    public class OrderProductValidate
    {
        public string ProductId { get; set; }

        public string ParentId { get; set; }

        public ProductDetail productDetail { get; set; }

        public List<ProductUnitModel> listUnit { get; set; }
        public ProductUnitModel lastUnit { get; set; }
        public int Quantity { get; set; }

        public int QuantityAfterConvert { get; set; }
    }
}
