using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos
{
    public class ProductImageRepo : Repository<ProductImage>, IProductImageRepo
    {
        public ProductImageRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {

        }

        //Truyền Id Parent
        public async Task<bool> addMultipleImages(List<ProductImage> productImages)
        {
            await context.AddRangeAsync(productImages);
            await Update();
            return true;
        }

        public async Task<ProductImageView> GetProductImage(string productId)
        {
            var query = from x in context.ProductImages.Where(x => x.ProductId.Equals(productId.Trim()) && x.IsFirstImage)
                        select x;

            var image = await query.Select(selector => new ProductImageView()
            {
                Id = selector.Id,
                ImageURL = selector.ImageUrl
            }).FirstOrDefaultAsync();

            if(image == null)
            {
                image = await context.ProductImages.Where(x => x.ProductId.Equals(productId.Trim())).Select(selector => new ProductImageView() {
                    Id = selector.Id,
                    ImageURL = selector.ImageUrl
                }).FirstOrDefaultAsync();
            }

            return image;
        }

        public Task<List<ProductImage>> GetProductImageDBs(string productId)
        {
            return context.ProductImages.Where(x => x.ProductId.Equals(productId)).ToListAsync();
        }

        public async Task<List<ProductImageView>> getProductImages(string productId)
        {
            var query = from x in context.ProductImages.Where(x => x.ProductId.Equals(productId.Trim()))
                        orderby x.IsFirstImage descending
                        select x;

            var images = await query.Select(selector => new ProductImageView()
            {
                Id = selector.Id,
                ImageURL = selector.ImageUrl
            }).ToListAsync();

            return images;
        }

        public async Task<List<UpdateProductImageModel>> getProductImagesUpdate(string productId)
        {
            var query = from x in context.ProductImages.Where(x => x.ProductId.Equals(productId.Trim()))
                        select x;

            var images = await query.Select(selector => new UpdateProductImageModel()
            {
                Id = selector.Id,
                ImageUrl = selector.ImageUrl,
                IsFirstImage = selector.IsFirstImage
            }).ToListAsync();

            return images;
        }

        public async Task<bool> removeAllImages(List<ProductImage> productImages)
        {
            if(productImages.Count > 0)
            {
                context.RemoveRange(productImages);
                await context.SaveChangesAsync();
            }
            return true;
        }
    }
}
