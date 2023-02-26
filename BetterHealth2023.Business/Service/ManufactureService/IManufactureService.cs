using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ManufactureService
{
    public interface IManufactureService
    {
        public Task<PagedResult<ViewManufacturerList>> GetManuFacturers(ManufacturerPagingRequest pagingRequest);

        public Task<ViewManufacturerList> GetManufacturer(string id);

        public Task<bool> CreateManufacturer(CreateNewManufacturer newManufacturer);

        public Task<bool> UpdateManufacturer(UpdateManufacturer updateManufacturer);
    }
}
