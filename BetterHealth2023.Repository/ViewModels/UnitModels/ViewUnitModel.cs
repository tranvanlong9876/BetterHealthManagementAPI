using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels
{
    public class ViewUnitModel
    {
        public string Id { get; set; }
        public string UnitName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
