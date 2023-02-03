using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
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

        public async Task<ViewProductModel> GetAllProductsPaging(ProductPagingRequest pagingRequest)
        {

            /*var query = from details in context.ProductDetails
                        from parent in context.ProductParents.Where(parents => parents.Id == details.ProductIdParent).DefaultIfEmpty()
                        from description in context.ProductDescriptions.Where(desc => desc.Id == parent.ProductDescriptionId).DefaultIfEmpty()
                        from subcategory in context.SubCategories.Where(sub_cate => sub_cate.Id == .AddressId).DefaultIfEmpty()
                        from site in context.SiteInformations.Where(s => s.Id == working.SiteId).DefaultIfEmpty()
                        where user.Id.Trim().Equals(guid.Trim())
                        select new { user, role, working, address, site };*/
            throw new NotImplementedException();
        }
    }
}
