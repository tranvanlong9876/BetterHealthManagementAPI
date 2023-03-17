using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderShipmentRepos
{
    public class OrderShipmentRepo : Repository<OrderShipment>, IOrderShipmentRepo
    {
        public OrderShipmentRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<ViewSpecificOrderDelivery> GetOrderDeliveryInformation(string orderId)
        {
            var query = from deliveryOrder in context.OrderShipments
                        from address in context.DynamicAddresses.Where(x => x.Id == deliveryOrder.DestinationAddressId)
                        select new { deliveryOrder, address };

            query = query.Where(x => x.deliveryOrder.OrderId.Equals(orderId));

            return await query.Select(selector => new ViewSpecificOrderDelivery()
            {
                ShippingFee = selector.deliveryOrder.ShippingFee,
                CityId = selector.address.CityId,
                DistrictId = selector.address.DistrictId,
                WardId = selector.address.WardId,
                HomeNumber = selector.address.HomeAddress,
                AddressId = selector.address.Id
            }).FirstOrDefaultAsync();
        }
    }
}
