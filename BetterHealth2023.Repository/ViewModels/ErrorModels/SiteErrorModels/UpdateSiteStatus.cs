using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.SiteErrorModels
{
    public class UpdateSiteStatus
    {
        public string SiteNotFound { get; set; }
        public string SiteNotEnoughPharmacist { get; set; }
        public string SiteNotEnoughManager { get; set; }

        public string OtherError { get; set; }
        public bool isError { get; set; } = false;
    }
}
