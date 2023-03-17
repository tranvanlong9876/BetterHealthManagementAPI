using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderPickupRepos
{
    public class OrderPickUpRepo : Repository<OrderPickUp>, IOrderPickUpRepo
    {
        public OrderPickUpRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public Task<ViewSpecificOrderPickUp> GetOrderPickUpInformation(string orderId)
        {
            return context.OrderPickUps.Where(x => x.OrderId.Equals(orderId)).Select(selector => new ViewSpecificOrderPickUp()
            {
                DatePickUp = selector.DatePickUp,
                TimePickUp = selector.TimePickUp
            }).FirstOrDefaultAsync();
        }
    }
}
