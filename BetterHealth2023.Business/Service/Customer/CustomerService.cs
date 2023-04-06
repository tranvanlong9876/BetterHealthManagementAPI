using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using FirebaseAdmin.Auth;
using System;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IDynamicAddressRepo _dynamicAdressRepo;
        private readonly ICustomerAddressRepo _customerAddressRepo;

        public CustomerService(ICustomerRepo customerRepo, IDynamicAddressRepo dynamicAdressRepo, ICustomerAddressRepo customerAddressRepo)
        {
            _customerRepo = customerRepo;
            _dynamicAdressRepo = dynamicAdressRepo;
            _customerAddressRepo = customerAddressRepo;
        }

        public async Task<Repository.DatabaseModels.Customer> CreateCustomer(RegisterCustomerModel customerRegisView)
        {
            //find customer by phoneno
            var customercheck = await _customerRepo.getCustomerBasedOnPhoneNo(customerRegisView.PhoneNo);
            if (customercheck != null)
            {
                throw new ArgumentException("Số điện thoại của khách hàng đã có sẵn trong hệ thống.");
            }

            Repository.DatabaseModels.Customer customer = new()
            {
                Id = Guid.NewGuid().ToString() + "-c",
                Fullname = customerRegisView.Fullname,
                PhoneNo = customerRegisView.PhoneNo,
                Email = customerRegisView.Email,
                Gender = customerRegisView.Gender,
                Status = 1,
                ImageUrl = customerRegisView.ImageUrl,
                Dob = customerRegisView.Dob,
            };
            await _customerRepo.Insert(customer);
            //insert customeraddress and dynamicaddress

            //insert dynamicaddress
            DynamicAddress dynamicAddress = new()
            {
                Id = Guid.NewGuid().ToString(),
                CityId = customerRegisView.CityId,
                DistrictId = customerRegisView.DistrictId,
                WardId = customerRegisView.WardId,
                HomeAddress = customerRegisView.HomeAddress,

            };
            await _dynamicAdressRepo.Insert(dynamicAddress);

            //insert customeraddress
            CustomerAddress customerAddress = new()
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = customer.Id,
                AddressId = dynamicAddress.Id,
                MainAddress = true,
            };
            await _customerAddressRepo.Insert(customerAddress);
            //add customer to database

            return await Task.FromResult(customer);
        }

        public async Task<CustomerLoginStatus> customerLoginPhoneOTP(LoginCustomerModel loginPhoneOTPModel)
        {
            CustomerLoginStatus checkError = new CustomerLoginStatus();
            //check Valid
            //bool check = await _phoneOTPRepos.VerifyPhoneOTP(loginPhoneOTPModel);
            loginPhoneOTPModel.firebaseToken = loginPhoneOTPModel.firebaseToken.Trim();
            try
            {
                FirebaseToken decodeToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(loginPhoneOTPModel.firebaseToken);
            }
            catch
            {
                throw new ArgumentException("Firebase Token đã hết hạn hoặc không hợp lệ.");
            }
            var phoneNo = JwtUserToken.DecodeFireBaseTokenToPhoneNo(loginPhoneOTPModel.firebaseToken);
            if (phoneNo == null)
            {
                checkError.isError = true;
                checkError.InvalidPhoneOTP = "Mã xác thực không đúng hoặc đã quá hạn, vui lòng thử lại";
                return checkError;
            }

            //get customer's information
            var customer = await _customerRepo.getCustomerBasedOnPhoneNo(phoneNo);

            if (customer == null)
            {
                checkError.isError = true;
                checkError.CustomerNotFound = "Thông tin khách hàng không trong hệ thống.";
                return checkError;
            }

            if (customer.Status == 0)
            {
                checkError.isError = true;
                checkError.CustomerInactive = "Khách hàng đã bị xóa hoặc ngưng hoạt động";
                return checkError;
            }

            var customerToken = JwtUserToken.CreateCustomerToken(customer);

            if (customerToken == null)
            {
                checkError.isError = true;
                checkError.OtherError = "Lỗi tạo token, vui lòng đăng nhập lại.";
                return checkError;
            }

            var customerTokenModel = new CustomerTokenModel()
            {
                Id = customer.Id,
                Email = customer.Email,
                ImageURL = customer.ImageUrl,
                Name = customer.Fullname,
                RoleName = "Customer",
                Status = customer.Status,
                Token = customerToken,
                PhoneNo = phoneNo
            };
            checkError.customerToken = customerTokenModel;
            checkError.isError = false;
            return checkError;
        }
        public async Task<CustomerViewSpecificModel> GetCustomerById(string id)
        {

            var customer = await _customerRepo.Get(id);
            if (customer == null)
            {
                return null;
            }
            //get list customeraddress by id
            var customerAddress = await _customerAddressRepo.GetAllCustomerAddressByCustomerId(id);

            for(int i = 0; i < customerAddress.Count; i++)
            {
                customerAddress[i].FullyAddress = await _dynamicAdressRepo.GetFullAddressFromAddressId(customerAddress[i].AddressId);
            }

            CustomerViewSpecificModel customerview = new()
            {
                //filter customer inton customerview
                CustomerId = customer.Id,
                FullName = customer.Fullname,
                PhoneNo = customer.PhoneNo,
                Email = customer.Email,
                Dob = customer.Dob,
                Gender = (int)customer.Gender,
                ImageUrl = customer.ImageUrl,
                CustomerAddressList = customerAddress
            };
            return customerview;
        }

        public async Task<bool> UpdateCustomer(CustomerUpdateModel customerUpdateMOdel)
        {
            var customer = await _customerRepo.Get(customerUpdateMOdel.CustomerId);
            if (customer == null)
            {
                return false;
            }

            customer.Fullname = customerUpdateMOdel.FullName;
            customer.Email = customerUpdateMOdel.Email;
            customer.Dob = customerUpdateMOdel.Dob;
            customer.ImageUrl = customerUpdateMOdel.ImageUrl;
            customer.Gender = customerUpdateMOdel.Gender;

            await _customerRepo.Update();
            return true;
        }
        public async Task<PagedResult<CustomerViewListModel>> GetCustomerPagingRequest(CustomerPagingRequest pagingRequest)
        {
            //get all customer
            var listCustomer = await _customerRepo.GetAllCustomerModelViewPaging(pagingRequest);

            return listCustomer;

        }
    }


}

