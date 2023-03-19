using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos
{
    public class ProductDetailRepo : Repository<ProductDetail>, IProductDetailRepo
    {
        public ProductDetailRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> CheckDuplicateBarCode(string BarCode)
        {
            var product_detail = await context.ProductDetails.Where(x => x.BarCode.Trim().Equals(BarCode.Trim())).FirstOrDefaultAsync();
            if (product_detail != null) return true;
            return false;
        }

        public async Task<bool> CheckDuplicateBarCodeUpdate(string BarCode, string productID)
        {
            var product_detail = await context.ProductDetails.Where(x => (x.BarCode.Trim().Equals(BarCode.Trim())) && (x.Id.Trim() != productID.Trim())).FirstOrDefaultAsync();
            if (product_detail != null) return true;
            return false;
        }

        public async Task<PagedResult<ViewProductListModel>> GetAllProductsPagingForCustomer(ProductPagingRequest pagingRequest)
        {
            var query = from details in context.ProductDetails.Where(x => x.UnitLevel == 1)
                        from parent in context.ProductParents.Where(parents => parents.Id == details.ProductIdParent).DefaultIfEmpty().Where(parents => parents.IsDelete.Equals(false))
                        from subcategory in context.SubCategories.Where(sub_cate => sub_cate.Id == parent.SubCategoryId).DefaultIfEmpty()
                        from maincategory in context.CategoryMains.Where(x => x.Id == subcategory.MainCategoryId).DefaultIfEmpty()
                        select new { details, parent, subcategory, maincategory };

            if (!string.IsNullOrEmpty(pagingRequest.mainCategoryID))
            {
                pagingRequest.subCategoryID = null;
            }

            if (!string.IsNullOrEmpty(pagingRequest.productName))
            {
                query = query.Where(x => (x.parent.Name.Contains(pagingRequest.productName.Trim())) || (x.details.BarCode.Contains(pagingRequest.productName.Trim())));
            }

            if (!string.IsNullOrEmpty(pagingRequest.subCategoryID))
            {
                query = query.Where(x => x.parent.SubCategoryId.Equals(pagingRequest.subCategoryID.Trim()));
            }

            if (!string.IsNullOrEmpty(pagingRequest.mainCategoryID))
            {
                query = query.Where(x => x.maincategory.Id.Equals(pagingRequest.mainCategoryID.Trim()));
            }

            if (pagingRequest.isPrescription.HasValue)
            {
                query = query.Where(x => x.parent.IsPrescription.Equals(pagingRequest.isPrescription));
            }

            if (!string.IsNullOrEmpty(pagingRequest.manufacturerID))
            {
                query = query.Where(x => x.parent.ManufacturerId.Equals(pagingRequest.manufacturerID.Trim()));
            }

            int totalRow = await query.CountAsync();

            var productList = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                .Take(pagingRequest.pageItems)
                .Select(selector => new ViewProductListModel()
                {
                    Id = selector.details.Id,
                    Name = selector.parent.Name,
                    SubCategoryId = selector.parent.SubCategoryId,
                    ManufacturerId = selector.parent.ManufacturerId,
                    IsPrescription = selector.parent.IsPrescription,
                    IsBatches = selector.parent.IsBatches,
                    UnitId = selector.details.UnitId,
                    UnitLevel = selector.details.UnitLevel,
                    Quantitative = selector.details.Quantitative,
                    SellQuantity = selector.details.SellQuantity,
                    Price = selector.details.Price,
                    IsSell = selector.details.IsSell,
                    BarCode = selector.details.BarCode
                }).ToListAsync();

            var pageResult = new PagedResult<ViewProductListModel>(productList, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pageResult;
        }

        public async Task<List<UpdateProductDetailModel>> GetProductDetailLists(string productParentID)
        {
            var results = context.ProductDetails.Where(x => x.ProductIdParent.Trim().Equals(productParentID.Trim())).OrderBy(x => x.UnitLevel);
            return await results.Select(model => mapper.Map<UpdateProductDetailModel>(model)).ToListAsync();

        }

        public async Task<List<ProductUnitModel>> GetProductLaterUnit(string productID, int unitLevel)
        {
            var query = from details in context.ProductDetails.Where(x => x.UnitLevel >= unitLevel).Where(x => x.ProductIdParent.Equals(productID))
                        from units in context.Units.Where(unit => unit.Id == details.UnitId).DefaultIfEmpty()
                        orderby details.UnitLevel ascending
                        select new { details, units };

            var productLists = await query.Select(selector => new ProductUnitModel()
            {
                Id = selector.details.Id,
                UnitId = selector.details.UnitId,
                UnitName = selector.units.UnitName,
                Quantitative = selector.details.Quantitative,
                UnitLevel = selector.details.UnitLevel
            }).ToListAsync();

            return productLists;
        }

        public async Task<string> GetProductParentID(string productID)
        {
            return await (from detail in context.ProductDetails.Where(x => x.Id.Equals(productID))
                          select detail.ProductIdParent).SingleOrDefaultAsync();
        }

        public async Task<List<ProductUnitModel>> GetProductUnitButThis(string productID, int unitLevel)
        {
            var query = from details in context.ProductDetails.Where(x => x.UnitLevel != unitLevel).Where(x => x.ProductIdParent.Equals(productID))
                        from units in context.Units.Where(unit => unit.Id == details.UnitId).DefaultIfEmpty()
                        orderby details.UnitLevel ascending
                        select new { details, units };

            query = query.Where(x => x.details.IsSell);

            var productLists = await query.Select(selector => new ProductUnitModel()
            {
                Id = selector.details.Id,
                UnitId = selector.details.UnitId,
                UnitName = selector.units.UnitName,
                Quantitative = selector.details.Quantitative,
                UnitLevel = selector.details.UnitLevel
            }).ToListAsync();

            return productLists;
        }

        public async Task<ViewSpecificProductModel> GetSpecificProduct(string productID, bool isInternal)
        {
            var query = from details in context.ProductDetails.Where(x => x.Id.Equals(productID))
                        from parent in context.ProductParents.Where(parents => parents.Id == details.ProductIdParent).DefaultIfEmpty().Where(parents => parents.IsDelete.Equals(false))
                        from description in context.ProductDescriptions.Where(desc => desc.Id == parent.ProductDescriptionId).DefaultIfEmpty()
                        from unitOfProduct in context.Units.Where(unitPro => unitPro.Id == details.UnitId).DefaultIfEmpty()
                        select new { details, parent, description, unitOfProduct };

            if (!isInternal)
            {
                query = query.Where(x => x.details.IsSell.Equals(true));
            }

            var productDesc = await query.Select(selector => new ProductDescriptionModel()
            {
                Id = selector.description.Id,
                Contraindications = selector.description.Contraindications,
                Effect = selector.description.Effect,
                Instruction = selector.description.Instruction,
                Preserve = selector.description.Preserve,
                SideEffect = selector.description.SideEffect,
            }).FirstOrDefaultAsync();

            var product = await query.Select(selector => new ViewSpecificProductModel()
            {
                Id = selector.details.Id,
                ProductIdParent = selector.details.ProductIdParent,
                Name = selector.parent.Name,
                IsPrescription = selector.parent.IsPrescription,
                IsBatches = selector.parent.IsBatches,
                ManufacturerId = selector.parent.ManufacturerId,
                Price = selector.details.Price,
                SubCategoryId = selector.parent.SubCategoryId,
                UnitId = selector.details.UnitId,
                UnitName = selector.unitOfProduct.UnitName,
                UnitLevel = selector.details.UnitLevel,
                descriptionModels = productDesc
            }).FirstOrDefaultAsync();

            return product;
        }

        public async Task<bool> UpdateProductDetailRange(List<UpdateProductDetailModel> updateProductDetailModels)
        {
            context.UpdateRange(updateProductDetailModels);
            await Update();
            return true;
        }

        public async Task<List<ViewProductListModel>> GetAllProductWithParent(string productParentID, int loadSellProduct)
        {

            if (loadSellProduct == 0) return null;

            var queryUnit = from parent in context.ProductParents
                            from detail in context.ProductDetails.Where(details => details.ProductIdParent == parent.Id)
                            where parent.Id.Equals(productParentID.Trim()) && detail.IsSell
                            orderby detail.UnitLevel ascending
                            select detail;

            var unitLevel = await queryUnit.Skip(loadSellProduct - 1).Take(1).Select(x => x.UnitLevel).SingleOrDefaultAsync();


            var query = from parent in context.ProductParents
                        from detail in context.ProductDetails.Where(details => details.ProductIdParent == parent.Id)
                        from subcategory in context.SubCategories.Where(sub_cate => sub_cate.Id == parent.SubCategoryId).DefaultIfEmpty()
                        select new { parent, detail };

            query = query.Where(x => (x.parent.Id.Equals(productParentID.Trim())) && (x.detail.IsSell) && (x.detail.UnitLevel <= unitLevel));

            var data = await query.Select(selector => new ViewProductListModel()
            {
                Id = selector.detail.Id,
                Name = selector.parent.Name,
                SubCategoryId = selector.parent.SubCategoryId,
                ManufacturerId = selector.parent.ManufacturerId,
                IsPrescription = selector.parent.IsPrescription,
                UnitId = selector.detail.UnitId,
                UnitLevel = selector.detail.UnitLevel,
                Quantitative = selector.detail.Quantitative,
                SellQuantity = selector.detail.SellQuantity,
                Price = selector.detail.Price,
                IsSell = selector.detail.IsSell,
                BarCode = selector.detail.BarCode
            }).ToListAsync();

            return data;
        }

        public async Task<List<ViewProductListModel>> GetAllProductForInternal(string productParentID, bool? isSell)
        {
            var queryUnit = from parent in context.ProductParents
                            from detail in context.ProductDetails.Where(details => details.ProductIdParent == parent.Id)
                            where parent.Id.Equals(productParentID.Trim())
                            orderby detail.UnitLevel ascending
                            select detail;

            if (isSell.HasValue)
            {
                queryUnit = queryUnit.Where(x => x.IsSell);
            }

            var unitLevel = await queryUnit.Take(1).Select(x => x.UnitLevel).SingleOrDefaultAsync();

            var query = from parent in context.ProductParents
                        from detail in context.ProductDetails.Where(details => details.ProductIdParent == parent.Id)
                        from subcategory in context.SubCategories.Where(sub_cate => sub_cate.Id == parent.SubCategoryId).DefaultIfEmpty()
                        select new { parent, detail };

            query = query.Where(x => (x.parent.Id.Equals(productParentID.Trim())) && (x.detail.UnitLevel <= unitLevel));

            if (isSell.HasValue)
            {
                query = query.Where(x => x.detail.IsSell);
            }

            var data = await query.Select(selector => new ViewProductListModel()
            {
                Id = selector.detail.Id,
                Name = selector.parent.Name,
                SubCategoryId = selector.parent.SubCategoryId,
                ManufacturerId = selector.parent.ManufacturerId,
                IsPrescription = selector.parent.IsPrescription,
                IsBatches = selector.parent.IsBatches,
                UnitId = selector.detail.UnitId,
                UnitLevel = selector.detail.UnitLevel,
                Quantitative = selector.detail.Quantitative,
                SellQuantity = selector.detail.SellQuantity,
                Price = selector.detail.Price,
                IsSell = selector.detail.IsSell,
                BarCode = selector.detail.BarCode
            }).ToListAsync();

            return data;
        }

        public async Task<ProductUnitModelForDiscount> GetProductNameAndCurrentUnit(string productId)
        {
            var query = from detail in context.ProductDetails
                        from parent in context.ProductParents.Where(x => x.Id == detail.ProductIdParent)
                        select new { detail, parent };

            query = query.Where(x => x.detail.Id.Equals(productId));

            return await query.Select(x => new ProductUnitModelForDiscount()
            {
                Name = x.parent.Name,
                UnitLevel = x.detail.UnitLevel,
                Price = x.detail.Price
            }).FirstOrDefaultAsync();
        }

        public async Task<PagedResult<ViewProductListModelForInternal>> GetAllProductsPagingForInternalUser(ProductPagingRequest pagingRequest)
        {
            var query = from details in context.ProductDetails.Where(x => x.UnitLevel == 1)
                        from parent in context.ProductParents.Where(parents => parents.Id == details.ProductIdParent).DefaultIfEmpty().Where(parents => parents.IsDelete.Equals(false))
                        from subcategory in context.SubCategories.Where(sub_cate => sub_cate.Id == parent.SubCategoryId).DefaultIfEmpty()
                        from maincategory in context.CategoryMains.Where(x => x.Id == subcategory.MainCategoryId).DefaultIfEmpty()
                        select new { details, parent, subcategory, maincategory };

            if (!string.IsNullOrEmpty(pagingRequest.mainCategoryID))
            {
                pagingRequest.subCategoryID = null;
            }

            if (!string.IsNullOrEmpty(pagingRequest.productName))
            {
                query = query.Where(x => (x.parent.Name.Contains(pagingRequest.productName.Trim())) || (x.details.BarCode.Contains(pagingRequest.productName.Trim())));
            }

            if (!string.IsNullOrEmpty(pagingRequest.subCategoryID))
            {
                query = query.Where(x => x.parent.SubCategoryId.Equals(pagingRequest.subCategoryID.Trim()));
            }

            if (!string.IsNullOrEmpty(pagingRequest.mainCategoryID))
            {
                query = query.Where(x => x.maincategory.Id.Equals(pagingRequest.mainCategoryID.Trim()));
            }

            if (pagingRequest.isPrescription.HasValue)
            {
                query = query.Where(x => x.parent.IsPrescription.Equals(pagingRequest.isPrescription));
            }

            if (!string.IsNullOrEmpty(pagingRequest.manufacturerID))
            {
                query = query.Where(x => x.parent.ManufacturerId.Equals(pagingRequest.manufacturerID.Trim()));
            }

            int totalRow = await query.CountAsync();

            var productList = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                .Take(pagingRequest.pageItems)
                .Select(selector => new ViewProductListModelForInternal()
                {
                    Id = selector.details.Id,
                    Name = selector.parent.Name,
                    SubCategoryId = selector.parent.SubCategoryId,
                    ManufacturerId = selector.parent.ManufacturerId,
                    IsPrescription = selector.parent.IsPrescription,
                    IsBatches = selector.parent.IsBatches,
                    UnitId = selector.details.UnitId,
                    UnitLevel = selector.details.UnitLevel,
                    Quantitative = selector.details.Quantitative,
                    SellQuantity = selector.details.SellQuantity,
                    Price = selector.details.Price,
                    IsSell = selector.details.IsSell,
                    BarCode = selector.details.BarCode
                }).ToListAsync();

            var pageResult = new PagedResult<ViewProductListModelForInternal>(productList, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pageResult;
        }

        public async Task<InformationToSendEmail> GetImageAndProductName(string productId)
        {
            var imageUrl = string.Empty;
            var productName = string.Empty;

            var query = from detail in context.ProductDetails
                        from parent in context.ProductParents.Where(x => x.Id == detail.ProductIdParent).DefaultIfEmpty()
                        from image in context.ProductImages.Where(x => x.ProductId == parent.Id && x.IsFirstImage).DefaultIfEmpty()
                        where detail.Id == productId
                        select new { parent.Name, image.ImageUrl };

            return await query.Select(selector => new InformationToSendEmail()
            {
                ImageUrl = selector.ImageUrl,
                Name = selector.Name
            }).FirstOrDefaultAsync();
        }

        public async Task<CartItem> AddMoreProductInformationToCart(string productId)
        {
            var query = (from detail in context.ProductDetails
                         from parent in context.ProductParents.Where(x => x.Id == detail.ProductIdParent).DefaultIfEmpty()
                         from image in context.ProductImages.Where(x => x.ProductId == parent.Id && x.IsFirstImage).DefaultIfEmpty()
                         select new { detail.Price, parent.Name, image.ImageUrl, detail.Id });

            var result = await query.Where(x => x.Id.Equals(productId)).Select(selector => new CartItem()
            {
                Price = selector.Price,
                ProductId = selector.Id,
                ProductImageUrl = selector.ImageUrl,
                ProductName = selector.Name
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
