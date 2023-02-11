using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.UnitErrorModels
{
    public class CreateUnitErrorModel
    {
        public bool isError { get; set; } = false;
        public string alreadyExist { get; set; }
    }
}
