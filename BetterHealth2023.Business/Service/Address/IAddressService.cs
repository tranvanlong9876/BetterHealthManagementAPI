using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.AddressModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Address
{
    public interface IAddressService
    {
        Task<List<CityModel>> GetAllCitys();

        Task<List<DistrictModel>> GetAllDistricts(string cityID);

        Task<List<WardModel>> GetAllWards(string districtID);

        Task InsertAddressSite(DynamicAddModel dynamicAddModel);

        Task<AddressModel> GetAddressById(string addressId);

        Task<CityModel> GetSpecificCity(string cityID);
        Task<DistrictModel> GetSpecificDistrict(string districtID);
        Task<WardModel> GetSpecificWard(string wardID);
    }
}
