using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<PagedResult<ViewProductListModel>> GetAllProductsPaging(ProductPagingRequest pagingRequest)
        {

            var query = from details in context.ProductDetails.Where(x => x.UnitLevel == (from details in context.ProductDetails where details.IsSell.Equals(pagingRequest.isSellFirstLevel) select details.UnitLevel).Min())
                        from parent in context.ProductParents.Where(parents => parents.Id == details.ProductIdParent).DefaultIfEmpty().Where(parents => parents.IsDelete.Equals(false))
                        from subcategory in context.SubCategories.Where(sub_cate => sub_cate.Id == parent.SubCategoryId).DefaultIfEmpty()
                        select new { details, parent, subcategory };

            if (!string.IsNullOrEmpty(pagingRequest.productName))
            {
                query = query.Where(x => x.parent.Name.Contains(pagingRequest.productName.Trim()));
            }

            if (!string.IsNullOrEmpty(pagingRequest.subCategoryID))
            {
                query = query.Where(x => x.parent.SubCategoryId.Equals(pagingRequest.subCategoryID.Trim()));
            }

            if (pagingRequest.isPrescription.HasValue)
            {
                query = query.Where(x => x.parent.IsPrescription.Equals(pagingRequest.isPrescription));
            }

            if (pagingRequest.isSell.HasValue)
            {
                query = query.Where(x => x.details.IsSell.Equals(pagingRequest.isSell));
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
                    UnitId = selector.details.UnitId,
                    UnitLevel = selector.details.UnitLevel,
                    Quantitative = selector.details.Quantitative,
                    SellQuantity = selector.details.SellQuantity,
                    Price = selector.details.Price,
                    IsSell = selector.details.IsSell
                }).ToListAsync();

            var pageResult = new PagedResult<ViewProductListModel>(productList, totalRow, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pageResult;
        }
    }
}
