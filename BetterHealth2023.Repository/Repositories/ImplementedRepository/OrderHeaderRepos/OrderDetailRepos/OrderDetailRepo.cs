using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using System.Collections.Generic;
using static System.Linq.Queryable;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderDetailRepos
{
    public class OrderDetailRepo : Repository<OrderDetail>, IOrderDetailRepo
    {
        public OrderDetailRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ViewSpecificOrderProduct>> GetViewSpecificOrderProducts(string orderId)
        {
            var query = from detail in context.OrderDetails
                        from productDetail in context.ProductDetails.Where(x => x.Id == detail.ProductId).DefaultIfEmpty()
                        from productParent in context.ProductParents.Where(x => x.Id == productDetail.ProductIdParent).DefaultIfEmpty()
                        from unit in context.Units.Where(x => x.Id == productDetail.UnitId).DefaultIfEmpty()
                        from productImage in context.ProductImages.Where(x => x.ProductId == productParent.Id && x.IsFirstImage).DefaultIfEmpty()
                        select new { detail, productDetail, productParent, unit, productImage };

            query = query.Where(x => x.detail.OrderId.Equals(orderId));


            return await query.Select(selector => new ViewSpecificOrderProduct()
            {
                Id = selector.detail.Id,
                OriginalPrice = selector.detail.OriginalPrice,
                DiscountPrice = selector.detail.DiscountPrice,
                Quantity = selector.detail.Quantity,
                PriceTotal = selector.detail.DiscountPrice * selector.detail.Quantity,
                ProductId = selector.detail.ProductId,
                ProductName = selector.productParent.Name,
                UnitName = selector.unit.UnitName,
                ProductNoteFromPharmacist = selector.detail.Note,
                IsBatches = selector.productParent.IsBatches,
                ImageUrl = selector.productImage.ImageUrl
            }).ToListAsync();
        }
    }
}
