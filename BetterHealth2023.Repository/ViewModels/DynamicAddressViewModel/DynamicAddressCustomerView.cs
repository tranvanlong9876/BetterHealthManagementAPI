using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using System;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel
{
    public class DynamicAddressCustomerView
    {
        public string ?AddressId { get; set; }
        public string ?CityId { get; set; }
        public string ?DistrictId { get; set; }
        public string ?WardId { get; set; }
        public string ?HomeAddress { get; set; }

     
    }
}
