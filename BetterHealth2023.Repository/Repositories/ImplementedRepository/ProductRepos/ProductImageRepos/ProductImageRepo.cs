using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos
{
    public class ProductImageRepo : Repository<ProductImage>, IProductImageRepo
    {
        public ProductImageRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ProductImageView>> getProductImages(string productId)
        {
            var query = from x in context.ProductImages.Where(x => x.ProductId.Equals(productId.Trim()))
                        select x;

            var images = await query.Select(selector => new ProductImageView()
            {
                Id = selector.Id,
                ImageURL = selector.ImageUrl
            }).ToListAsync();

            return images;
        }
    }
}
