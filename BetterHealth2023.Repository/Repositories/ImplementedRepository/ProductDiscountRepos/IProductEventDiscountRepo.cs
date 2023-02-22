using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos
{
    public interface IProductEventDiscountRepo : IRepository<EventProductDiscount>
    {
        public Task<bool> CheckAlreadyExistProductDiscount(string productID);

        public Task<ProductDiscountViewList> GetProductDiscount(string productID);

        public Task<string> GetIdEventProductDiscount(string productId);
    }
}
