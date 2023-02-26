using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class UpdateProductImportStatus
    {
        public bool isError { get; set; }

        public string NotFound { get; set; }

        public string NotFoundBatches { get; set; }

        public string ProductIDNeedBatches { get; set; }

        public string AlreadyReleased { get; set; }
    }
}
