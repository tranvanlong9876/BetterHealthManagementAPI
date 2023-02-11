﻿using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos
{
    public class CustomerPointRepo : Repository<CustomerPoint>, ICustomerPointRepo
    {
        public CustomerPointRepo(BetterHealthManagementContext context) : base(context)
        {

        }
    }
}