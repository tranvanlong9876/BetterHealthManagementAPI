using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using Microsoft.EntityFrameworkCore;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos
{
    public class OrderHeaderRepo : Repository<OrderHeader>, IOrderHeaderRepo
    {
        public OrderHeaderRepo(BetterHealthManagementContext context) : base(context)
        {
        }

        public async Task<List<OrderHeader>> GetOrderHeadersBySiteId(string siteId)
        {
            List<OrderHeader> list = await context.OrderHeaders.Where(x => x.SiteId == siteId).ToListAsync();
            return list;
        }
    }
}
