using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class CreateProductImportStatus
    {
        public bool isError { get; set; }
        public string notFoundBatches { get; set; }

        public string ProductIDNeedBatches { get; set; }
    }
}
