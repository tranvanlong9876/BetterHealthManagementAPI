using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerUpdateMOdel
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? Dob { get; set; }
        public int Gender { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public List<CustomerAddressView> CustomerAddressList { get; set; } = null;

    }
}
