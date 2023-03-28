using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels;
using System;
using System.Collections.Generic;
using static System.Linq.Queryable;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderVNPayRepos
{
    public class OrderVNPayRepo : Repository<OrderVnpay>, IOrderVNPayRepo
    {
        public OrderVNPayRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<RefundVNPayModel> GetRefundVNPayModel(string orderId)
        {
            var query = from header in context.OrderHeaders
                        from vnpay in context.OrderVnpays.Where(x => x.OrderId == header.Id).DefaultIfEmpty()
                        select new { header.TotalPrice, header.Id, vnpay };

            query = query.Where(x => x.Id.Equals(orderId));

            return await query.Select(selector => new RefundVNPayModel()
            {
                OrderId = orderId,
                Amount = selector.TotalPrice,
                TransactionDate = selector.vnpay.VnpPayDate,
                TransactionNo = selector.vnpay.VnpTransactionNo
            }).FirstOrDefaultAsync();
        }
    }
}
