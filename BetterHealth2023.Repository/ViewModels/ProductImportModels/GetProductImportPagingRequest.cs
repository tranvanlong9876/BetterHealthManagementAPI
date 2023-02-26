using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class GetProductImportPagingRequest : PagingRequestBase
    {
        public string SiteID { get; set; }
        public string ManagerID { get; set; }

        public bool? isRelease { get; set; }
    }
}
