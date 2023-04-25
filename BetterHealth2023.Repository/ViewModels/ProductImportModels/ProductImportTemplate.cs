using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class ProductImportTemplate
    {
        public int QuantityAfterConvert { get; set; }
        public string UnitAfterConvert { get; set; }
        public string Calculation { get; set; }
        public string TemplateMessage { get; set; }

        public string ProductIdAfterConvert { get; set; }
    }
}
