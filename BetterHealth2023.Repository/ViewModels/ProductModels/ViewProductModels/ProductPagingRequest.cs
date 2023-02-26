using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ProductPagingRequest : PagingRequestBase
    {
        public bool? isPrescription { get; set; }
        public bool? isSell { get; set; }
        public string subCategoryID { get; set; }
        public string productName { get; set; }
        //search theo Name, BarCode
        public string manufacturerID { get; set; }
    }
}
