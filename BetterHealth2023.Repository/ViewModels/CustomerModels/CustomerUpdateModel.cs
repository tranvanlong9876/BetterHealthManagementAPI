using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerUpdateModel
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public int Gender { get; set; }
        [Required]
        public string ImageUrl { get; set; }


    }
}
