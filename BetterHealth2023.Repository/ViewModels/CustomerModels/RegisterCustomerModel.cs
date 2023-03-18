using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class RegisterCustomerModel
    {
        [Required]
        public string Fullname { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public int? Gender { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        public DateTime? Dob { get; set; }
        [Required]
        public string CityId { get; set; }
        [Required]
        public string DistrictId { get; set; }
        [Required]
        public string WardId { get; set; }
        [Required]
        public string HomeAddress { get; set; }
    }
}
