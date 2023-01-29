using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels
{
    public class CustomerLoginStatus
    {
        public string CustomerNotFound { get; set; }
        public string CustomerInactive { get; set; }
        public string InvalidPhoneOTP { get; set; }

        public CustomerTokenModel customerToken { get; set; }

        public string OtherError { get; set; }
        public bool isError { get; set; } = false;
    }
}
