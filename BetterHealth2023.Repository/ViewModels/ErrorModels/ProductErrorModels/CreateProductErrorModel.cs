using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.ProductErrorModels
{
    public class CreateProductErrorModel
    {
        public string DuplicateBarCode { get; set; }
        public string BarCodeError { get; set; }
        public string OtherError { get; set; }
        public bool isError { get; set; } = false;

    }
}
