using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerViewSpecificModel
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? Dob { get; set; }
        public int Gender { get; set; }
        public string ImageUrl { get; set; }
        public List<CustomerAddressView> CustomerAddressList { get; set; } = Enumerable.Empty<CustomerAddressView>().ToList();
    }
}
