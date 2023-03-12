using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerRegisView
    {
        public string Fullname { get; set; }
        
       
        public string PhoneNo { get; set; }
     
     
        public string Email { get; set; }
    
        public int? Gender { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? Dob { get; set; }

        public string CityId { get; set; }

        public string DistrictId { get; set; }

        public string WardId { get; set; }

        public string HomeAddress { get; set; }
        
      

    }
}
