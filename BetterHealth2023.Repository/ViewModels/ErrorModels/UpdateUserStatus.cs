using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels
{
    public class UpdateUserStatus
    {
        public string NotEnoughManager { get; set; }

        public string NotEnoughPharmacist { get; set; }

        public string PharmacistHaveOrder { get; set; }

        public string OtherError { get; set; }
        public bool isError { get; set; } = false;
    }
}
