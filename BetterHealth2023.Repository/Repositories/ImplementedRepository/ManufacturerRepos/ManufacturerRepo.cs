using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ManufacturerRepos
{
    public class ManufacturerRepo : Repository<Manufacturer>, IManufacturerRepo
    {
        public ManufacturerRepo(BetterHealthManagementContext context, IMapper mapper) : base(context, mapper)
        {
            
        }

        public async Task<int> GetProductUsing(string manufacturerID)
        {
            var query = from manufact in context.Manufacturers
                        from parent in context.ProductParents.Where(x => x.ManufacturerId == manufact.Id).DefaultIfEmpty()
                        from detail in context.ProductDetails.Where(x => x.ProductIdParent == parent.Id)
                        select new { manufact };

            query = query.Where(x => x.manufact.Id.Equals(manufacturerID.Trim()));

            return await query.CountAsync();
        }

        public async Task<ViewManufacturerList> GetViewManufacturer(string id)
        {
            var query = from manufact in context.Manufacturers
                        from country in context.Countries.Where(x => x.Id == manufact.CountryId)
                        select new { manufact, country };

            var manufactModel = await query.Where(x => x.manufact.Id.Equals(id.Trim())).Select(selector => new ViewManufacturerList()
            {
                Id = selector.manufact.Id,
                ManufacturerName = selector.manufact.ManufacturerName,
                CountryId = selector.manufact.CountryId,
                CountryName = selector.country.Name
            }).FirstOrDefaultAsync();

            return manufactModel;
        }

        public async Task<PagedResult<ViewManufacturerList>> GetViewManufacturers(ManufacturerPagingRequest pagingRequest)
        {
            var query = from manufact in context.Manufacturers
                        from country in context.Countries.Where(x => x.Id == manufact.CountryId)
                        select new { manufact, country };

            if(!String.IsNullOrWhiteSpace(pagingRequest.manufacturerName))
            {
                query = query.Where(x => x.manufact.ManufacturerName.Contains(pagingRequest.manufacturerName.Trim()));
            }

            var totalRecord = await query.CountAsync();

            var manufactList = await query.Skip((pagingRequest.pageIndex - 1) * pagingRequest.pageItems)
                                  .Take(pagingRequest.pageItems).Select(selector => new ViewManufacturerList()
            {
                Id = selector.manufact.Id,
                ManufacturerName = selector.manufact.ManufacturerName,
                CountryId = selector.manufact.CountryId,
                CountryName = selector.country.Name
            }).ToListAsync();

            var pagedResult = new PagedResult<ViewManufacturerList>(manufactList, totalRecord, pagingRequest.pageIndex, pagingRequest.pageItems);

            return pagedResult;
        }

        
    }
}
