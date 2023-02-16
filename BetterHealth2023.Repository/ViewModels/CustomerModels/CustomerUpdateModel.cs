using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerUpdateMOdel
    {
        public string CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime? Dob { get; set; }
        public int Gender { get; set; }
        public string ImageUrl { get; set; }
       
       
        public string CityId { get; set; }
      
        public string DistrictId { get; set; }
       
        public string WardId { get; set; }
       
        public string HomeAddress { get; set; }
    }
}
