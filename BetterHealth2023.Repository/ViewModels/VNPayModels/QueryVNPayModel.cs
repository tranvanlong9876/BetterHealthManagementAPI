using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels
{
    public class QueryVNPayModel
    {
        public string OrderId { get; set; }
        public string TransactionDate { get; set; }
        public string IpAddress { get; set; }
    }
}
