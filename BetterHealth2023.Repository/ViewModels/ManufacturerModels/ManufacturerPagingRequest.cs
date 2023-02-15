using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels
{
    public class ManufacturerPagingRequest : PagingRequestBase
    {
        public string manufacturerName { get; set; }
    }
}
