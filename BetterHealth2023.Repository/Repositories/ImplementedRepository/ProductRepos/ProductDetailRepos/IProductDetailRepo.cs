using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos
{
    public interface IProductDetailRepo : IRepository<ProductDetail>
    {
        public Task<PagedResult<ViewProductListModel>> GetAllProductsPaging(ProductPagingRequest pagingRequest);
        public Task<bool> CheckDuplicateBarCode(string BarCode);
    }
}
