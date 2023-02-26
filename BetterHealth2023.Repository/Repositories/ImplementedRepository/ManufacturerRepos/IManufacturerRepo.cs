using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ManufacturerRepos
{
    public interface IManufacturerRepo : IRepository<Manufacturer>
    {
        public Task<PagedResult<ViewManufacturerList>> GetViewManufacturers(ManufacturerPagingRequest pagingRequest);

        public Task<int> GetProductUsing(string manufacturerID);

        public Task<ViewManufacturerList> GetViewManufacturer(string id);
    }
}
