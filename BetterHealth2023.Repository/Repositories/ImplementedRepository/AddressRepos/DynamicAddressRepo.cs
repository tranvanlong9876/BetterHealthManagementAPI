using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.AddressModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos
{
    public class DynamicAddressRepo : Repository<DynamicAddress>, IDynamicAddressRepo
    {
        public DynamicAddressRepo(BetterHealthManagementContext context) : base(context)
        {

        }

        public async Task<AddressModel> GetAddressFromId(string addressID)
        {
            var query = from address in context.DynamicAddresses
                        from city in context.Cities.Where(city => city.Id == address.CityId).DefaultIfEmpty()
                        from district in context.Districts.Where(district => district.Id == address.DistrictId).DefaultIfEmpty()
                        from ward in context.Wards.Where(ward => ward.Id == address.WardId).DefaultIfEmpty()
                        where address.Id.Trim().Equals(addressID.Trim())
                        select new { address, city, district, ward};
            var addressModel = await query.Select(selector => new AddressModel()
            {
                AddressId = selector.address.Id,
                CityId = selector.address.CityId,
                CityName = selector.city.CityName,
                DistrictId = selector.address.DistrictId,
                DistrictName = selector.district.DistrictName,
                WardId = selector.address.WardId,
                WardName = selector.ward.WardName,
                HomeAddress = selector.address.HomeAddress
            }).FirstOrDefaultAsync();

            return addressModel;
        }

        public async Task<List<CityModel>> GetAllCitys()
        {
            var query = from city in context.Cities
                        select new { city };

            var cities = await query.Select(selector => new CityModel()
            {
                Id = selector.city.Id,
                CityName = selector.city.CityName
            }).ToListAsync();
            return cities;
        }

        public async Task<List<DistrictModel>> GetAllDistricts(string cityID)
        {
            var query = from district in context.Districts
                        where district.CityId.Trim().Equals(cityID.Trim())
                        select new { district };
            var districts = await query.Select(selector => new DistrictModel()
            {
                Id = selector.district.Id,
                DistrictName = selector.district.DistrictName
            }).ToListAsync();
            return districts;
        }

        public async Task<List<WardModel>> GetAllWards(string districtID)
        {
            var query = from ward in context.Wards
                        where ward.DistrictId.Trim().Equals(districtID.Trim())
                        select new { ward };
            var wards = await query.Select(selector => new WardModel()
            {
                Id = selector.ward.Id,
                WardName = selector.ward.WardName
            }).ToListAsync();
            return wards;
        }

        public async Task<CityModel> GetSpecificCity(string cityID)
        {
            var city = await context.Cities.FindAsync(cityID);
            var cityModel = new CityModel()
            {
                Id = city.Id,
                CityName = city.CityName
            };
            return cityModel;
        }

        public async Task<DistrictModel> GetSpecificDistrict(string districtID)
        {
            var district = await context.Districts.FindAsync(districtID);
            var districtModel = new DistrictModel()
            {
                Id = district.Id,
                DistrictName = district.DistrictName
            };
            return districtModel;
        }

        public async Task<WardModel> GetSpecificWard(string wardID)
        {
            var ward = await context.Wards.FindAsync(wardID);
            var wardModel = new WardModel()
            {
                Id = ward.Id,
                WardName = ward.WardName
            };
            return wardModel;
        }

        public async Task<bool> InsertNewAddress(DynamicAddress dynamicAddress)
        {
            await context.AddAsync(dynamicAddress);
            await Update();
            return true;
        }

    }
}
