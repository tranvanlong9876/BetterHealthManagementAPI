using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels
{
    public class GetUnitPagingModel : PagingRequestBase
    {
        public bool? isCountable { get; set; }
        public string UnitName { get; set; }
    }
}
