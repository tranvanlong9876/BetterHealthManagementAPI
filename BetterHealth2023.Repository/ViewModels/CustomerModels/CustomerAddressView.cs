using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using System.Collections.Generic;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerAddressView
    {
        public string Id { get; set; }
        public string AddressId { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string HomeAddress { get; set; }
        public string FullyAddress { get; set; }
        public bool IsMainAddress { get; set; }
    }
}
