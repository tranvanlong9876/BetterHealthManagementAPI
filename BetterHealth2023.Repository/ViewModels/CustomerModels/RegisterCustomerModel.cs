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
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public int? Gender { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [DataType(DataType.Date)]
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
