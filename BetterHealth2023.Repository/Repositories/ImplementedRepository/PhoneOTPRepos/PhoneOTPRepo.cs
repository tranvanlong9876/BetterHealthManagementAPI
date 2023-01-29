using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.PhoneOTPRepos
{
    public class PhoneOTPRepo : Repository<PhoneOtp>, IPhoneOTPRepo
    {
        public PhoneOTPRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> VerifyPhoneOTP(LoginCustomerModel loginPhoneOTP)
        {
            var phoneOTP = await context.PhoneOtps.Where(x => (x.PhoneNo.Trim().Equals(loginPhoneOTP.phoneNo.Trim()))
                                            && (x.OtpCode.Trim().Equals(loginPhoneOTP.otpCode.Trim()))
                                            && (DateTime.Now < x.ExpireDate)).FirstOrDefaultAsync();
            if (phoneOTP != null) return true;

            return false;
                
        }
    }
}
