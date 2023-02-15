using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos
{
    public interface IProductImageRepo : IRepository<ProductImage>
    {
        public Task<List<ProductImageView>> getProductImages(string productId);

        public Task<ProductImageView> GetProductImage(string productId);
        public Task<List<UpdateProductImageModel>> getProductImagesUpdate(string productId);

        public Task<bool> removeAllImages(string productId);

        public Task<bool> addMultipleImages(List<ProductImage> productImages);
    }
}
