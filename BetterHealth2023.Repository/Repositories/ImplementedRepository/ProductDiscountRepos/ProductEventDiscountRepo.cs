using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos
{
    public class ProductEventDiscountRepo : Repository<EventProductDiscount>, IProductEventDiscountRepo
    {
        public ProductEventDiscountRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> CheckAlreadyExistProductDiscount(string productID)
        {
            var query = from discountevent in context.EventProductDiscounts
                        from discount in context.ProductDiscounts.Where(x => x.Id == discountevent.DiscountId).DefaultIfEmpty()
                        select new { discount, discountevent };

            var currentTime = CustomDateTime.Now;

            query = query.Where(x => currentTime > x.discount.StartDate && currentTime < x.discount.EndDate && x.discountevent.ProductId.Equals(productID) && !x.discount.IsDelete);

            return await query.CountAsync() > 0;
        }

        public static bool Between(DateTime currentTime, DateTime startTime, DateTime endTime)
        {
            return (currentTime > startTime && currentTime < endTime);
        }

        public async Task<ProductDiscountViewList> GetProductDiscount(string productID)
        {
            var query = from discountevent in context.EventProductDiscounts
                        from discount in context.ProductDiscounts.Where(x => x.Id == discountevent.DiscountId).DefaultIfEmpty()
                        select new { discount, discountevent };

            var currentTime = CustomDateTime.Now;

            query = query.Where(x => currentTime > x.discount.StartDate && currentTime < x.discount.EndDate && x.discountevent.ProductId.Equals(productID) && !x.discount.IsDelete);

            var data = await query.Select(selector => new ProductDiscountViewList()
            {
                DiscountMoney = selector.discount.DiscountMoney,
                DiscountPercent = selector.discount.DiscountPercent,
                Reason = selector.discount.Reason,
                Title = selector.discount.Title
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<string> GetIdEventProductDiscount(string productId)
        {
            var query = from discount in context.ProductDiscounts
                        from eventDiscount in context.EventProductDiscounts.Where(x => x.DiscountId == discount.Id)
                        select new { eventDiscount, discount };

            var currentTime = CustomDateTime.Now;

            query = query.Where(x => x.eventDiscount.ProductId.Equals(productId) && x.discount.StartDate < currentTime && x.discount.EndDate > currentTime && !x.discount.IsDelete);

            return await query.Select(selector => selector.eventDiscount.Id).FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalProductDiscountId(string discountId)
        {
            return await context.EventProductDiscounts.Where(x => x.DiscountId.Equals(discountId)).CountAsync();
        }

        public async Task<List<ProductDiscountView>> GetAllProductDiscountId(string discountId)
        {
            return await context.EventProductDiscounts.Where(x => x.DiscountId.Equals(discountId)).Select(x => new ProductDiscountView()
            {
                Id = x.Id,
                ProductId = x.ProductId
            }).ToListAsync();
        }
    }
}
