using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerAddressSer
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly ICustomerAddressRepo _customerAddressRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;

        public CustomerAddressService(ICustomerAddressRepo customerAddressRepo, IDynamicAddressRepo dynamicAddressRepo)
        {
            _customerAddressRepo = customerAddressRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
        }

        public async Task<IActionResult> InsertCustomerAddress(CustomerAddressInsertModel CustomerAddressInsertModel)
        {
            var totalMainAddress = await _customerAddressRepo.GetTotalCurrentCustomerAddress(CustomerAddressInsertModel.CustomerId);

            DynamicAddress dynamicAddressnew = new()
            {
                Id = Guid.NewGuid().ToString(),
                CityId = CustomerAddressInsertModel.CityId,
                DistrictId = CustomerAddressInsertModel.DistrictId,
                WardId = CustomerAddressInsertModel.WardId,
                HomeAddress = CustomerAddressInsertModel.HomeAddress,
            };

            //insert customeraddress
            CustomerAddress customerAddress = new()
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = CustomerAddressInsertModel.CustomerId,
                AddressId = dynamicAddressnew.Id,
                MainAddress = totalMainAddress > 0 ? CustomerAddressInsertModel.IsMainAddress : true
            };
            //update database
            await _dynamicAddressRepo.Insert(dynamicAddressnew);
            await _customerAddressRepo.Insert(customerAddress);

            return new OkObjectResult("Insert địa chỉ khách hàng thành công");
        }

        public async Task<IActionResult> RemoveCustomerAddressById(string id)
        {
            var customerAddress = await _customerAddressRepo.Get(id);
            if (customerAddress == null)
            {
                //return null
                return new NotFoundObjectResult(new { message = "Customer Address Not Found" });
            }
            else
            {
                await _dynamicAddressRepo.Remove(await _dynamicAddressRepo.Get(customerAddress.AddressId));
                await _customerAddressRepo.Remove(customerAddress);

                //return action result success
                return new OkObjectResult("Xóa địa chỉ khách hàng thành công!");

            }
        }

        public async Task<IActionResult> UpdateCustomerAddress(AddressUpdateModel addressUpdateModel)
        {
            var customerCurrentAddress = await _customerAddressRepo.Get(addressUpdateModel.CustomerAddressId);

            if (customerCurrentAddress == null) return new NotFoundObjectResult("Không tìm thấy địa chỉ khách hàng");
            var dynamicAddress = await _dynamicAddressRepo.Get(customerCurrentAddress.AddressId);
            dynamicAddress.CityId = addressUpdateModel.CityId;
            dynamicAddress.DistrictId = addressUpdateModel.DistrictId;
            dynamicAddress.WardId = addressUpdateModel.WardId;
            dynamicAddress.HomeAddress = addressUpdateModel.HomeAddress;

            await _dynamicAddressRepo.Update();

            return new OkObjectResult("Cập nhật địa chỉ khách hàng thành công!");
        }
    }
}
