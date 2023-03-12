using System.Collections.Generic;
using static System.Linq.Queryable;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos
{
    public class OrderHeaderRepo : Repository<OrderHeader>, IOrderHeaderRepo
    {
        public OrderHeaderRepo(BetterHealthManagementContext context) : base(context)
        {
        }

        public async Task<bool> CheckDuplicateOrderId(string orderId)
        {
            return await context.OrderHeaders.Where(x => x.Id.Equals(orderId)).CountAsync() >= 1 ? true : false;
        }

        public async Task<List<OrderHeader>> GetExecutingOrdersByPharmacistId(string pharID)
        {
            //pick up order are executing by pharmacist ID
            var query = from order in context.OrderHeaders
                        where (order.OrderStatus.Equals("2") || order.OrderStatus.Equals("3") || order.OrderStatus.Equals("5") || order.OrderStatus.Equals("6") || order.OrderStatus.Equals("7"))
                        && (order.PharmacistId.Trim().Equals(pharID.Trim()))
                        select new { order };
            var orders = await query.Select(selector => new OrderHeader()).ToListAsync();
            
            return orders;
        }

        public async Task<List<OrderHeader>> GetOrderHeadersBySiteId(string siteId)
        {
            List<OrderHeader> list = await context.OrderHeaders.Where(x => x.SiteId == siteId).ToListAsync();
            return list;
        }
    }
}
