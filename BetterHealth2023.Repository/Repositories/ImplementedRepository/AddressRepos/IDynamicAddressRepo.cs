using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.AddressModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos
{
    public interface IDynamicAddressRepo : IRepository<DynamicAddress>
    {
        public Task<bool> InsertNewAddress(DynamicAddress dynamicAddress);

        public Task<List<CityModel>> GetAllCitys();

        public Task<List<DistrictModel>> GetAllDistricts(string cityID);

        public Task<List<WardModel>> GetAllWards(string districtID);
        public Task<AddressModel> GetAddressFromId(string addressID);
        public Task<CityModel> GetSpecificCity(string cityID);
        public Task<DistrictModel> GetSpecificDistrict(string districtID);
        public Task<WardModel> GetSpecificWard(string wardID);
    }
}
