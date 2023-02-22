using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using BetterHealthManagementAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductDiscountServices
{
    public class ProductDiscountService : IProductDiscountService
    {
        private readonly IProductDiscountRepo _productDiscountRepo;
        private readonly IProductEventDiscountRepo _productEventDiscountRepo;

        public ProductDiscountService(IProductDiscountRepo productDiscountRepo, IProductEventDiscountRepo productEventDiscountRepo)
        {
            _productDiscountRepo = productDiscountRepo;
            _productEventDiscountRepo = productEventDiscountRepo;
        }

        public async Task<bool> AddProductToExistingDiscount(string discountId, ProductModel product)
        {
            var discountModel = await _productDiscountRepo.Get(discountId);
            if (discountModel == null) return false;
            if (discountModel.IsDelete) return false;
            if (discountModel.EndDate < DateTime.Now) return false;

            if (await _productEventDiscountRepo.CheckAlreadyExistProductDiscount(discountId)) return false;

            var eventDiscount = new EventProductDiscount()
            {
                DiscountId = discountId,
                Id = Guid.NewGuid().ToString(),
                ProductId = product.productId
            };

            await _productEventDiscountRepo.Insert(eventDiscount);

            return true;

        }

        public async Task<CreateProductDiscountStatus> CreateProductDiscount(CreateProductDiscountModel discountModel)
        {
            var checkError = new CreateProductDiscountStatus();

            if (!discountModel.DiscountMoney.HasValue && !discountModel.DiscountPercent.HasValue)
            {
                checkError.isError = true;
                checkError.VariablesError = "Phải có tối thiểu 1 trường dữ liệu DiscountMoney hoặc DiscountPercent.";
                return checkError;
            }
            if (discountModel.DiscountMoney.HasValue && discountModel.DiscountPercent.HasValue)
            {
                checkError.isError = true;
                checkError.VariablesError = "Chỉ 1 trong 2 dữ liệu DiscountMoney hoặc DiscountPercent được phép có.";
                return checkError;
            }
            if (discountModel.StartDate >= discountModel.EndDate)
            {
                checkError.isError = true;
                checkError.VariablesError = "Ngày kết thúc phải sau ngày bắt đầu.";
                return checkError;
            }

            for (int i = 0; i < discountModel.Products.Count; i++)
            {
                var productID = discountModel.Products[i].ProductId;
                if(await CheckAlreadyExistDiscount(productID))
                {
                    checkError.isError = true;
                    checkError.alreadyExistDiscount = "Sản phẩm đã tồn tại sự kiện giảm giá, không thể thêm nữa";
                    checkError.productError = productID;
                    return checkError;
                }
            }

            //insert Header
            var productDiscountHeaderDB = _productDiscountRepo.TransferBetweenTwoModels<CreateProductDiscountModel, ProductDiscount>(discountModel);
            productDiscountHeaderDB.Id = Guid.NewGuid().ToString();
            productDiscountHeaderDB.CreatedDate = DateTime.Now;
            productDiscountHeaderDB.IsDelete = false;

            var ProductList = new List<EventProductDiscount>();
            for (int i = 0; i < discountModel.Products.Count; i++)
            {
                ProductList.Add(new EventProductDiscount()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = discountModel.Products[i].ProductId,
                    DiscountId = productDiscountHeaderDB.Id
                });
            }
            await _productDiscountRepo.Insert(productDiscountHeaderDB);
            await _productEventDiscountRepo.InsertRange(ProductList);

            checkError.isError = false;
            return checkError;
        }

        public async Task<bool> RemoveProductFromExistingDiscount(string product)
        {
            var eventId = await _productEventDiscountRepo.GetIdEventProductDiscount(product);
            if (eventId == null) return false;

            return await _productEventDiscountRepo.Remove(await _productEventDiscountRepo.Get(eventId));

        }

        public async Task<UpdateProductDiscountStatus> UpdateGeneralInformation(UpdateProductDiscountModel discountModel)
        {
            var checkError = new UpdateProductDiscountStatus();
            if(!discountModel.DiscountMoney.HasValue && !discountModel.DiscountPercent.HasValue)
            {
                checkError.isError = true;
                checkError.VariablesError = "Phải có tối thiểu 1 trường dữ liệu DiscountMoney hoặc DiscountPercent.";
                return checkError;
            }
            if (discountModel.DiscountMoney.HasValue && discountModel.DiscountPercent.HasValue)
            {
                checkError.isError = true;
                checkError.VariablesError = "Chỉ 1 trong 2 dữ liệu DiscountMoney hoặc DiscountPercent được phép có.";
                return checkError;
            }
            if(discountModel.StartDate >= discountModel.EndDate)
            {
                checkError.isError = true;
                checkError.VariablesError = "Ngày kết thúc phải sau ngày bắt đầu.";
                return checkError;
            }
            var productDiscountDB = await _productDiscountRepo.Get(discountModel.Id);
            if(productDiscountDB == null)
            {
                checkError.isError = true;
                checkError.NotFoundDiscountModel = "Không tìm thấy thông tin khuyến mãi, vui lòng kiểm tra lại ID.";
                return checkError;
            }

            if(productDiscountDB.EndDate < DateTime.Now)
            {
                checkError.isError = true;
                checkError.NotAllowed = "Sự kiện khuyến mãi đã kết thúc, vui lòng thêm mới một sự kiện khác.";
                return checkError;
            }

            productDiscountDB.StartDate = discountModel.StartDate;
            productDiscountDB.Title = discountModel.Title;
            productDiscountDB.EndDate = discountModel.EndDate;
            productDiscountDB.DiscountMoney = discountModel.DiscountMoney;
            productDiscountDB.DiscountPercent = discountModel.DiscountPercent;
            productDiscountDB.Reason = discountModel.Reason;

            await _productDiscountRepo.Update();

            checkError.isError = false;
            return checkError;

        }

        private Task<bool> CheckAlreadyExistDiscount(string productID)
        {
            return _productEventDiscountRepo.CheckAlreadyExistProductDiscount(productID);
        }
    }
}
