using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerUpdateMOdel
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public int Gender { get; set; }
        public string ImageUrl { get; set; }
        public List<CustomerAddressView> CustomerAddressList { get; set; }
    }
}
