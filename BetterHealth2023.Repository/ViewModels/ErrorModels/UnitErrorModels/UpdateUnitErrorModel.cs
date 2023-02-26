using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.UnitErrorModels
{
    public class UpdateUnitErrorModel
    {
        public bool isError { get; set; } = false;
        public string duplicateName { get; set; }
    }
}
