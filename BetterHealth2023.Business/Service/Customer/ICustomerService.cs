using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer
{
    public interface ICustomerService
    {
        public Task<CustomerLoginStatus> customerLoginPhoneOTP(LoginCustomerModel loginPhoneOTPModel);
    }
}
