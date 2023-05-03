using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using BetterHealthManagementAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IProductImageRepo _productImageRepo;
        private readonly IProductDetailRepo _productDetailRepo;

        public ProductDiscountService(IProductDiscountRepo productDiscountRepo, IProductEventDiscountRepo productEventDiscountRepo, IProductImageRepo productImageRepo, IProductDetailRepo productDetailRepo)
        {
            _productDiscountRepo = productDiscountRepo;
            _productEventDiscountRepo = productEventDiscountRepo;
            _productImageRepo = productImageRepo;
            _productDetailRepo = productDetailRepo;
        }

        public async Task<IActionResult> AddProductToExistingDiscount(string discountId, ProductModel product)
        {
            var discountModel = await _productDiscountRepo.Get(discountId);
            if (discountModel == null) return new NotFoundObjectResult("Không tìm thấy chương trình giảm giá này!");
            if (discountModel.IsDelete) return new NotFoundObjectResult("Không tìm thấy chương trình giảm giá hoặc đã bị xóa!");
            if (discountModel.EndDate < CustomDateTime.Now) return new BadRequestObjectResult("Chương trình giảm giá đã kết thúc, vui lòng tạo một chương trình khác!");

            if (await _productEventDiscountRepo.CheckAlreadyExistProductDiscount(product.productId, discountId)) return new BadRequestObjectResult("Sản phẩm này đang được giảm giá ở chương trình khác, không thể thêm!");

            var eventDiscount = new EventProductDiscount()
            {
                DiscountId = discountId,
                Id = Guid.NewGuid().ToString(),
                ProductId = product.productId
            };

            await _productEventDiscountRepo.Insert(eventDiscount);

            return new OkObjectResult("Thêm sản phẩm mới vào chương trình khuyến mãi thành công!");

        }

        public async Task<IActionResult> CreateProductDiscount(CreateProductDiscountModel discountModel)
        {
            if (!discountModel.DiscountMoney.HasValue && !discountModel.DiscountPercent.HasValue)
            {
                return new BadRequestObjectResult("Phải có tối thiểu 1 trường dữ liệu DiscountMoney hoặc DiscountPercent.");
            }
            if (discountModel.DiscountMoney.HasValue && discountModel.DiscountPercent.HasValue)
            {
                return new BadRequestObjectResult("Chỉ 1 trong 2 dữ liệu DiscountMoney hoặc DiscountPercent được phép có.");
            }
            if (discountModel.StartDate >= discountModel.EndDate)
            {
                return new BadRequestObjectResult("Ngày kết thúc phải sau ngày bắt đầu.");
            }

            for (int i = 0; i < discountModel.Products.Count; i++)
            {
                var productID = discountModel.Products[i].ProductId;
                if(await CheckAlreadyExistDiscount(productID, ""))
                {
                    return new BadRequestObjectResult("Sản phẩm đã tồn tại sự kiện giảm giá, không thể thêm nữa");
                }
            }

            //insert Header
            var productDiscountHeaderDB = _productDiscountRepo.TransferBetweenTwoModels<CreateProductDiscountModel, ProductDiscount>(discountModel);
            productDiscountHeaderDB.Id = Guid.NewGuid().ToString();
            productDiscountHeaderDB.CreatedDate = CustomDateTime.Now;
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

            return new CreatedResult("", "Thêm Chương Trình Giảm Giá Thành Công!");
        }

        public async Task<PagedResult<ViewProductDiscountList>> GetAllProductDiscountPaging(GetProductDiscountPagingRequest pagingRequest)
        {
            var pagedResult = await _productDiscountRepo.GetAllProductDiscountPaging(pagingRequest);
            
            if(pagedResult.Items.Count >= 1)
            {
                for (int i = 0; i < pagedResult.Items.Count; i++)
                {
                    var startDate = pagedResult.Items[i].StartDate;
                    var endDate = pagedResult.Items[i].EndDate;
                    var currentDate = CustomDateTime.Now;

                    pagedResult.Items[i].Status = GetStatus(startDate, currentDate, endDate);
                    pagedResult.Items[i].TotalProduct = await GetTotalProductFromDiscountId(pagedResult.Items[i].Id);

                }
            }

            return pagedResult;
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

            if(productDiscountDB.EndDate < CustomDateTime.Now)
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

        private async Task<int> GetTotalProductFromDiscountId(string discountId)
        {
            return await _productEventDiscountRepo.GetTotalProductDiscountId(discountId.Trim());
        }

        private string GetStatus(DateTime startDate, DateTime currentDate, DateTime endDate)
        {
            if (startDate < currentDate && endDate > currentDate)
            {
                return "Đang diễn ra, kết thúc sau " + (endDate.Subtract(currentDate)).Days + " ngày nữa";
            }
            if (currentDate < startDate)
            {
                return "Chưa diễn ra, bắt đầu sau " + (startDate.Subtract(currentDate)).Days + " ngày nữa";
            }
            if (currentDate > endDate)
            {
                return "Đã kết thúc";
            }

            return null;
        }
        private Task<bool> CheckAlreadyExistDiscount(string productID, string discountId)
        {
            return _productEventDiscountRepo.CheckAlreadyExistProductDiscount(productID, discountId);
        }

        public async Task<ViewProductDiscountSpecific> GetProductDiscount(string discountId)
        {
            var productDiscountView = await _productDiscountRepo.GetViewModel<ViewProductDiscountSpecific>(discountId);
            if (productDiscountView == null) return null;
            productDiscountView.EventProductDiscounts = await _productEventDiscountRepo.GetAllProductDiscountId(discountId);
            productDiscountView.TotalProduct = await GetTotalProductFromDiscountId(discountId);
            productDiscountView.Status = GetStatus(productDiscountView.StartDate, CustomDateTime.Now, productDiscountView.EndDate);
            for(int i = 0; i < productDiscountView.EventProductDiscounts.Count; i++)
            {
                
                var productId = productDiscountView.EventProductDiscounts[i].ProductId;
                var productParentId = await _productDetailRepo.GetProductParentID(productId);
                var imagesModel = await _productImageRepo.GetProductImage(productParentId);
                productDiscountView.EventProductDiscounts[i].ProductImageUrl = imagesModel == null ? null : imagesModel.ImageURL;
                var productModel = await _productDetailRepo.GetProductNameAndCurrentUnit(productId);
                var unitName = GetStringUnit(await _productDetailRepo.GetProductLaterUnit(productParentId, productModel.UnitLevel));

                productDiscountView.EventProductDiscounts[i].Price = productModel.Price;
                productDiscountView.EventProductDiscounts[i].ProductName = productModel.Name + " (" + unitName + ")";

                if (productDiscountView.DiscountMoney.HasValue)
                {
                    productDiscountView.EventProductDiscounts[i].PriceAfterDiscount = productModel.Price - productDiscountView.DiscountMoney.Value;
                }

                if (productDiscountView.DiscountPercent.HasValue)
                {
                    productDiscountView.EventProductDiscounts[i].PriceAfterDiscount = productModel.Price - (productModel.Price * productDiscountView.DiscountPercent.Value / 100);
                }
            }

            return productDiscountView;
        }

        private string GetStringUnit(List<ProductUnitModel> productUnitList)
        {
            var namewithUnit = String.Empty;
            if (productUnitList.Count >= 1)
            {
                for (var j = 0; j < productUnitList.Count; j++)
                {
                    var productUnit = productUnitList[j];
                    if (j == 0)
                    {
                        namewithUnit = namewithUnit + "1 " + productUnit.UnitName;
                    }
                    else
                    {
                        namewithUnit = namewithUnit + productUnit.Quantitative + " " + productUnit.UnitName;
                    }

                    if (j != productUnitList.Count - 1) namewithUnit += " x ";
                }
            }
            return namewithUnit;
        }

        public async Task<IActionResult> CheckExistProductDiscount(string discountId, string productId)
        {
            var isExist = await CheckAlreadyExistDiscount(productId, discountId);

            if (isExist) return new BadRequestObjectResult("Sản phẩm đã tồn tại ở chương trình giảm giá khác");
            return new OkResult();
        }
    }
}
