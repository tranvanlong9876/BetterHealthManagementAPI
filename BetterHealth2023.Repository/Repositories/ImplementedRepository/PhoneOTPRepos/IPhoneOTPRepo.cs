using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.PhoneOTPRepos
{
    public interface IPhoneOTPRepo : IRepository<PhoneOtp>
    {
        public Task<bool> VerifyPhoneOTP(LoginCustomerModel loginPhoneOTP);
    }


}
