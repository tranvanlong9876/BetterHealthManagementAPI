using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels
{
    public class UpdateProductDiscountStatus
    {
        public bool isError { get; set; }
        public string VariablesError { get; set; }
        public string NotFoundDiscountModel { get; set; }
        public string NotAllowed { get; set; }
    }
}
