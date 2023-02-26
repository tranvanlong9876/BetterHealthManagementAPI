using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
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

        public async Task<bool> CreateManufacturer(CreateNewManufacturer newManufacturer)
        {
            var manuDB = new Manufacturer();
            manuDB.Id = Guid.NewGuid().ToString();
            manuDB.ManufacturerName = newManufacturer.Name;
            manuDB.CountryId = newManufacturer.CountryID;
            return await _manufacturerRepo.Insert(manuDB);
        }

        public async Task<ViewManufacturerList> GetManufacturer(string id)
        {
            var manufactViewModel = await _manufacturerRepo.GetViewManufacturer(id);
            if (manufactViewModel == null) return null;
            manufactViewModel.ProductUsing = await _manufacturerRepo.GetProductUsing(id);
            return manufactViewModel;
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

        public async Task<bool> UpdateManufacturer(UpdateManufacturer updateManufacturer)
        {
            var manufacturerDB = await _manufacturerRepo.Get(updateManufacturer.Id);
            if (manufacturerDB == null) return false;
            manufacturerDB.ManufacturerName = updateManufacturer.Name;
            manufacturerDB.CountryId = updateManufacturer.CountryID;
            await _manufacturerRepo.Update();

            return true;
        }
    }
}
