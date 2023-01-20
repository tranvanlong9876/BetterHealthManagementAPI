using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.SubCategoryModels
{
    public class GetSubCategoryPagingRequest : PagingRequestBase
    {
        public string MainCategoryID { get; set; }
    }
}
