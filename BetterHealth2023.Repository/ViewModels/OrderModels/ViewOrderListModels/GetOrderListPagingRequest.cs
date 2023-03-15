using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels
{
    public class GetOrderListPagingRequest : PagingRequestBase
    {
        public int? OrderTypeId { get; set; }

        public bool? NotAcceptable { get; set; }
    }
}
