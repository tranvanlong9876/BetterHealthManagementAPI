using System.Collections.Generic;
using static System.Linq.Queryable;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels;
using System;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using static System.Linq.Enumerable;

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

        public async Task<PagedResult<ViewOrderList>> GetAllOrders(GetOrderListPagingRequest pagingRequest, UserInformation userInformation)
        {
            var query = from header in context.OrderHeaders
                        from info in context.OrderContactInfos.Where(x => x.OrderId == header.Id).DefaultIfEmpty()
                        from site in context.SiteInformations.Where(x => x.Id == header.SiteId).DefaultIfEmpty()
                        from orderStatus in context.OrderStatuses.Where(x => x.Id == header.OrderStatus).DefaultIfEmpty()
                        from delivery in context.OrderShipments.Where(x => x.OrderId == header.Id && header.OrderTypeId.Equals(Commons.Commons.ORDER_TYPE_DELIVERY)).DefaultIfEmpty()
                        from address in context.DynamicAddresses.Where(x => x.Id == delivery.DestinationAddressId && header.OrderTypeId.Equals(Commons.Commons.ORDER_TYPE_DELIVERY)).DefaultIfEmpty()
                        select new { header, info, address, orderStatus };

            switch (userInformation.RoleName)
            {
                case Commons.Commons.CUSTOMER_NAME:
                    query = query.Where(x => x.info.CustomerId.Equals(userInformation.UserId)).OrderByDescending(o => o.header.CreatedDate);
                    break;
                case Commons.Commons.PHARMACIST_NAME:
                case Commons.Commons.MANAGER_NAME:
                    query = query.Where(x => (x.header.SiteId.Equals(userInformation.SiteId) || (x.header.OrderTypeId.Equals(Commons.Commons.ORDER_TYPE_DELIVERY) && x.header.SiteId == null && x.address.CityId.Equals(userInformation.SiteCityId))));

                    bool hasOrderTypeId = false;
                    bool hasBoolAccepable = false;
                    if (pagingRequest.OrderTypeId.HasValue)
                    {
                        query = query.Where(x => x.header.OrderTypeId.Equals(pagingRequest.OrderTypeId.Value));
                        hasOrderTypeId = true;
                    }

                    if (pagingRequest.NotAcceptable.HasValue)
                    {
                        hasBoolAccepable = true;
                        if (pagingRequest.NotAcceptable.Value)
                        {
                            query = query.Where(x => x.header.IsApproved == null);
                        }
                        else
                        {
                            query = query.Where(x => x.header.IsApproved != null);
                        }

                    }
                    bool usedOrderBy = false;
                    //Override OrderBy
                    if (hasOrderTypeId && hasBoolAccepable)
                    {
                        //Đang muốn nhận đơn giao
                        if (pagingRequest.NotAcceptable.Value && pagingRequest.OrderTypeId.Value == Commons.Commons.ORDER_TYPE_DELIVERY)
                        {
                            query = query.OrderBy(o => o.header.OrderTypeId.Equals(Commons.Commons.ORDER_TYPE_DELIVERY) && o.address.WardId.Equals(userInformation.SiteWardId) ? 1 :
                                                  o.header.OrderTypeId.Equals(Commons.Commons.ORDER_TYPE_DELIVERY) && o.address.DistrictId.Equals(userInformation.SiteDistrictId) ? 2 : 3)
                                 .ThenBy(o => o.header.CreatedDate);
                            usedOrderBy = true;
                        }
                    }

                    if (!usedOrderBy)
                    {
                        if (hasBoolAccepable)
                        {
                            if (pagingRequest.NotAcceptable.Value)
                            {
                                query = query.OrderBy(x => x.header.CreatedDate);
                            }
                            else
                            {
                                query = query.OrderByDescending(x => x.header.CreatedDate);
                            }
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.header.CreatedDate);
                        }
                    }

                    break;
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                                  .Take(pagingRequest.pageItems).Select(selector => new ViewOrderList()
                                  {
                                      Id = selector.header.Id,
                                      UsedPoint = selector.header.UsedPoint,
                                      OrderTypeName = Commons.Commons.ConvertToOrderTypeString((Commons.Commons.OrderType)selector.header.OrderTypeId),
                                      OrderTypeId = selector.header.OrderTypeId,
                                      PaymentMethod = Commons.Commons.ConvertToOrderPayTypeString((Commons.Commons.OrderPayType)selector.header.PayType),
                                      PaymentMethodId = selector.header.PayType,
                                      IsPaid = selector.header.IsPaid,
                                      CreatedDate = selector.header.CreatedDate,
                                      TotalPrice = selector.header.TotalPrice,
                                      OrderStatus = selector.header.OrderStatus,
                                      NeedAcceptance = !selector.header.IsApproved.HasValue,
                                      PharmacistId = selector.header.PharmacistId,
                                      SiteId = selector.header.SiteId,
                                      OrderStatusName = selector.orderStatus.OrderStatusName
                                  }).ToListAsync();

            return new PagedResult<ViewOrderList>(data, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);
        }

        public async Task<List<OrderHeader>> GetExecutingOrdersByPharmacistId(string pharID)
        {
            //pick up order are executing by pharmacist ID
            var query = from order in context.OrderHeaders
                        where (!Commons.Commons.COMPLETED_ORDERSTATUS_ID.Contains(order.OrderStatus))
                        && (order.PharmacistId.Trim().Equals(pharID.Trim()))
                        select new { order };
            var orders = await query.Select(selector => new OrderHeader() {
            }).ToListAsync();

            return orders;
        }

        public async Task<List<OrderHeader>> GetOrderHeadersBySiteId(string siteId)
        {
            List<OrderHeader> list = await context.OrderHeaders.Where(x => x.SiteId == siteId).ToListAsync();
            return list;
        }

        public async Task<ViewOrderSpecific> GetSpecificOrder(string orderId)
        {
            var query = from header in context.OrderHeaders
                        from contactInfo in context.OrderContactInfos.Where(x => x.OrderId == header.Id).DefaultIfEmpty()
                        from orderStatus in context.OrderStatuses.Where(x => x.Id == header.OrderStatus).DefaultIfEmpty()
                        select new { header, contactInfo, orderStatus};
            
            query = query.Where(x => x.header.Id.Equals(orderId));

            var data = await query.Select(selector => new ViewOrderSpecific() {
                Id = orderId,
                NeedAcceptance = selector.header.IsApproved == null,
                IsPaid = selector.header.IsPaid,
                Note = selector.header.Note,
                CreatedDate = selector.header.CreatedDate,
                OrderTypeId = selector.header.OrderTypeId,
                PaymentMethodId = selector.header.PayType,
                PharmacistId = selector.header.PharmacistId,
                OrderStatus = selector.header.OrderStatus,
                OrderStatusName = selector.orderStatus.OrderStatusName,
                PaymentMethod = Commons.Commons.ConvertToOrderPayTypeString((Commons.Commons.OrderPayType)selector.header.PayType),
                OrderTypeName = Commons.Commons.ConvertToOrderTypeString((Commons.Commons.OrderType)selector.header.OrderTypeId),
                SiteId = selector.header.SiteId,
                TotalPrice = selector.header.TotalPrice,
                UsedPoint = selector.header.UsedPoint,
                orderContactInfo = new ViewSpecificOrderContactInfo()
                {
                    Email = selector.contactInfo.Email,
                    Fullname = selector.contactInfo.Fullname,
                    PhoneNumber = selector.contactInfo.PhoneNo
                }
            }).FirstOrDefaultAsync();

            return data;
        }
    }
}
