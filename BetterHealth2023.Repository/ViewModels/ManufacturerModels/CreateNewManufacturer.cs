using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels
{
    public class CreateNewManufacturer
    {
        [Required]
        public string Name { get; set; }
        public string CountryID { get; set; }
    }
}
