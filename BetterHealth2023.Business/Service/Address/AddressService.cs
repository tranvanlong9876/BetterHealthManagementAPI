using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.AddressModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Address
{
    public class AddressService : IAddressService
    {
        private readonly IDynamicAddressRepo _dynamicAddressRepo;

        public AddressService(IDynamicAddressRepo dynamicAddressRepo)
        {
            _dynamicAddressRepo = dynamicAddressRepo;
        }

        public async Task<AddressModel> GetAddressById(string addressId)
        {
            var addressModel = await _dynamicAddressRepo.GetAddressFromId(addressId);
            return addressModel;
        }

        public async Task<List<CityModel>> GetAllCitys()
        {
            var cities = await _dynamicAddressRepo.GetAllCitys();
            return cities;
        }

        public async Task<List<DistrictModel>> GetAllDistricts(string cityID)
        {
            var districts = await _dynamicAddressRepo.GetAllDistricts(cityID);
            return districts;
        }

        public async Task<List<WardModel>> GetAllWards(string districtID)
        {
            var wards = await _dynamicAddressRepo.GetAllWards(districtID);
            return wards;
        }

        public async Task InsertAddressSite(DynamicAddModel dynamicAddModel)
        {
            DynamicAddress dynamicAddress = new DynamicAddress()
            {
                Id = dynamicAddModel.Id,
                CityId = dynamicAddModel.CityId,
                DistrictId = dynamicAddModel.DistrictId,
                WardId = dynamicAddModel.WardId,
                HomeAddress = dynamicAddModel.HomeAddress,

            };
            //insert to database
            await _dynamicAddressRepo.Insert(dynamicAddress);
        }
    }
}
