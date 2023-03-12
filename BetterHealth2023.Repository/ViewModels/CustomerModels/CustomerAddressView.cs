using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using System.Collections.Generic;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerAddressView
    {
        public string ?Id { get; set; }
        public string ?CustomerId { get; set; }
        public string ?AddressId { get; set; }
        public DynamicAddressCustomerView? DynamicAddressCustomerView { get; set; }
    }
}
