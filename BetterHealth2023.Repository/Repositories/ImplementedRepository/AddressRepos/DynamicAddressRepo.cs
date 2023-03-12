using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.AddressModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.Queryable;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos
{
    public class DynamicAddressRepo : Repository<DynamicAddress>, IDynamicAddressRepo
    {
        public DynamicAddressRepo(BetterHealthManagementContext context) : base(context)
        {

        }

        public async Task<bool> CheckAddressChangeById(AddressUpdateModel addressUpdateModel)
        {
            //find dynamic address by id
            var dynamicAddress =  context.DynamicAddresses.Find(addressUpdateModel.AddressId);
            //compare what were change
            if (dynamicAddress.CityId.Trim().Equals(addressUpdateModel.CityId.Trim()) &&
                dynamicAddress.DistrictId.Trim().Equals(addressUpdateModel.DistrictId.Trim()) &&
                dynamicAddress.WardId.Trim().Equals(addressUpdateModel.WardId.Trim()) &&
                dynamicAddress.HomeAddress.Trim().Equals(addressUpdateModel.HomeAddress.Trim()))
            {

                return false;
            }
            //remove customeraddress by dynamic address
            var customerAddressremove = context.CustomerAddresses.Where(customerAddress => customerAddress.AddressId.Trim()
            .Equals(addressUpdateModel.AddressId.Trim())).FirstOrDefault();
            context.CustomerAddresses.Remove(customerAddressremove);      
            //create new customer address with new information
            DynamicAddress dynamicAddressnew = new()
            {
                Id = Guid.NewGuid().ToString(),
                CityId = addressUpdateModel.CityId,
                DistrictId = addressUpdateModel.DistrictId,
                WardId = addressUpdateModel.WardId,
                HomeAddress = addressUpdateModel.HomeAddress,

            };

            //insert customeraddress
            CustomerAddress customerAddress = new()
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = addressUpdateModel.CustomerId,
                AddressId = dynamicAddressnew.Id,
                MainAddress = true,
            };
            //update database
            context.DynamicAddresses.Add(dynamicAddressnew);
            context.CustomerAddresses.Add(customerAddress);
            await context.SaveChangesAsync();
            return true;
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

        public async Task<List<DynamicAddressCustomerView>> GetAllDynamicAddressByCusId(string id)
        {
            //get all DynamicAddress by customer id
            var dynamicAddress = context.DynamicAddresses.Where(dynamicAddress => dynamicAddress.CustomerAddresses
            .Any(customerAddress => customerAddress.CustomerId.Trim().Equals(id.Trim()))).ToListAsync();
            List<DynamicAddressCustomerView> listAddCus = new List<DynamicAddressCustomerView>();
            //filter in to list<DynamicAddressCustomerView>
            foreach (var item in dynamicAddress.Result)
            {
                DynamicAddressCustomerView dynamicAddCustomerView = new()
                {
                    AddressId = item.Id,
                    CityId = item.CityId,
                    DistrictId = item.DistrictId,
                    WardId = item.WardId,
                    HomeAddress = item.HomeAddress,
                };
                //add inton listAddCus
                listAddCus.Add(dynamicAddCustomerView);
                
            }
            return listAddCus;
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

        public async Task<string> GetFullAddressFromAddressId(string addressId)
        {
            var query = from dynamic in context.DynamicAddresses
                        from city in context.Cities.Where(x => x.Id == dynamic.CityId).DefaultIfEmpty()
                        from district in context.Districts.Where(x => x.Id == dynamic.DistrictId).DefaultIfEmpty()
                        from ward in context.Wards.Where(x => x.Id == dynamic.WardId).DefaultIfEmpty()
                        select new { dynamic, city, district, ward };

            return await query.Where(x => x.dynamic.Id.Equals(addressId)).Select(x => new string(x.dynamic.HomeAddress + ", Phường " + x.ward.WardName + ", " + x.district.DistrictName + ", " + x.city.CityName)).FirstOrDefaultAsync();
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
