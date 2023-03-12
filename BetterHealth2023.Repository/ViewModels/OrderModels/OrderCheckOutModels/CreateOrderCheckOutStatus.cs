using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels
{
    public class CreateOrderCheckOutStatus
    {
        public bool isError { get; set; }
        public string missingSite { get; set; }
        public string missingPharmacist { get; set; }

        public string missingModel { get; set; }

        public string missingAddress { get; set; }
    }
}
