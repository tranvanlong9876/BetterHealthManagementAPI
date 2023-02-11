using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels
{
    public class CreateUnitModel
    {
        [Required]
        public string UnitName { get; set; }
    }
}
