using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ManufacturerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.ManufactureService
{
    public class ManufactureService : IManufactureService
    {
        private readonly IManufacturerRepo _manufacturerRepo;
        public ManufactureService(IManufacturerRepo manufacturerRepo)
        {
            _manufacturerRepo = manufacturerRepo;
        }
        public async Task<PagedResult<ViewManufacturerList>> GetManuFacturers(ManufacturerPagingRequest pagingRequest)
        {
            var pageResult = await _manufacturerRepo.GetViewManufacturers(pagingRequest);

            for (var i = 0; i < pageResult.Items.Count; i++)
            {
                var manufacturerModel = pageResult.Items[i];
                pageResult.Items[i].ProductUsing = await _manufacturerRepo.GetProductUsing(manufacturerModel.Id);
            }

            return pageResult;
        }
    }
}
