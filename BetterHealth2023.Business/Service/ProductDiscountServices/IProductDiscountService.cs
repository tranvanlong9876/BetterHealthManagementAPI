using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using BetterHealthManagementAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductDiscountServices
{
    public interface IProductDiscountService
    {
        public Task<CreateProductDiscountStatus> CreateProductDiscount(CreateProductDiscountModel discountModel);

        public Task<UpdateProductDiscountStatus> UpdateGeneralInformation(UpdateProductDiscountModel discountModel);

        public Task<bool> AddProductToExistingDiscount(string discountId, ProductModel product);

        public Task<bool> RemoveProductFromExistingDiscount(string product);

        public Task<PagedResult<ViewProductDiscountList>> GetAllProductDiscountPaging(GetProductDiscountPagingRequest pagingRequest);

        public Task<ViewProductDiscountSpecific> GetProductDiscount(string discountId);

    }
}
