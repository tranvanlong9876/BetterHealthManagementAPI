using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.ProductErrorModels
{
    public class UpdateProductErrorModel
    {
        public bool isError { get; set; }
        public string DuplicateBarCode { get; set; }
        public string BarCodeError { get; set; }

        public UpdateProductViewModel productViewModel { get; set; }
    }
}
