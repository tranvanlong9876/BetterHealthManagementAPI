using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels
{
    public class RefundVNPayModel
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
    }
}
