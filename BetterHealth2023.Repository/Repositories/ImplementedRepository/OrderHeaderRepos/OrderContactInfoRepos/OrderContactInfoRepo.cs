using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderContactInfoRepos
{
    public class OrderContactInfoRepo : Repository<OrderContactInfo>, IOrderContactInfoRepo
    {
        public OrderContactInfoRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
