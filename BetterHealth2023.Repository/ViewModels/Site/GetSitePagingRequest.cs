using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class GetSitePagingRequest : PagingRequestBase
    {
        public bool? IsActive { get; set; }
        public bool? IsDelivery { get; set; }
        public string CityID { get; set; }
    }
}
