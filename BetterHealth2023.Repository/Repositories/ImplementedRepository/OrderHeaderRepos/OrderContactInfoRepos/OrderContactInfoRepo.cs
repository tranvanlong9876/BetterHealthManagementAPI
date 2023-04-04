using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using static System.Linq.Queryable;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderContactInfoRepos
{
    public class OrderContactInfoRepo : Repository<OrderContactInfo>, IOrderContactInfoRepo
    {
        public OrderContactInfoRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<OrderContactInfo> GetCustomerInfoBasedOnOrderId(string orderId)
        {
            return await context.OrderContactInfos.Where(x => x.OrderId.Equals(orderId)).FirstOrDefaultAsync();
        }
    }
}
