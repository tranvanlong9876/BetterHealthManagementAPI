using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels
{
    public class ViewProductDiscountList
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountMoney { get; set; }

        public int TotalProduct { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
