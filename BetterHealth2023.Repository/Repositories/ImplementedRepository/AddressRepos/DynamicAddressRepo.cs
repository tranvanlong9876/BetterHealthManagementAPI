﻿using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos
{
    public class DynamicAddressRepo : Repository<DynamicAddress>, IDynamicAddressRepo
    {
        public DynamicAddressRepo(BetterHealthManagementContext context) : base(context)
        {

        }
        public async Task<bool> InsertNewAddress(DynamicAddress dynamicAddress)
        {
            await context.AddAsync(dynamicAddress);
            await Update();
            return true;
        }
    }
}
