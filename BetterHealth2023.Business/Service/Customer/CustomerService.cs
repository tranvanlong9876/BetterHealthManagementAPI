using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using FirebaseAdmin.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly ICustomerPointRepo _customerpointRepo;
        private readonly IDynamicAddressRepo _dynamicAdressRepo;

        public CustomerService(ICustomerRepo customerRepo, ICustomerPointRepo customerpointRepo, IDynamicAddressRepo dynamicaddressAdressRepo)
        {
            _customerRepo = customerRepo;
            _customerpointRepo = customerpointRepo;
            _dynamicAdressRepo = dynamicaddressAdressRepo;
        }

        public async Task<Repository.DatabaseModels.Customer> CreateCustomer(CustomerRegisView customerRegisView)
        {
           

            //find customer by phoneno
            var customercheck = await _customerRepo.getCustomerBasedOnPhoneNo(customerRegisView.PhoneNo);
            if (customercheck != null)
            {
                return null;
            }
            //find customer by email
            var customercheckemail = await _customerRepo.getCustomerBasedOnEmail(customerRegisView.Email);
            if (customercheckemail != null)
            {
                return null;
            }
            Repository.DatabaseModels.Customer customer = new() {
                Id = Guid.NewGuid().ToString(),
                Fullname = customerRegisView.Fullname,
                PhoneNo = customerRegisView.PhoneNo,
                Email = customerRegisView.Email,
                Gender = customerRegisView.Gender,
                Status = 1,
                ImageUrl = customerRegisView.ImageUrl,
                Dob = customerRegisView.Dob,

        };
            //insert customerpoint
            CustomerPoint customerPoint = new()
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = customer.Id,
                Point = 0,
                IsPlus =true,
                Description = "Tạo tài khoản thành công",
                CreateDate = DateTime.Now,
                

            };
            //add customer to database
            await _customerRepo.Insert(customer);
            await _customerpointRepo.Insert(customerPoint);
            return await Task.FromResult(customer);
        }

        public async Task<CustomerLoginStatus> customerLoginPhoneOTP(LoginCustomerModel loginPhoneOTPModel)
        {
            CustomerLoginStatus checkError = new CustomerLoginStatus();
            //check Valid
            //bool check = await _phoneOTPRepos.VerifyPhoneOTP(loginPhoneOTPModel);
            //FirebaseToken decodeToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(loginPhoneOTPModel.firebaseToken);
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

            if(customerToken == null)
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
                Token = customerToken
            };
            checkError.customerToken = customerTokenModel;
            checkError.isError = false;
            return checkError;

        }

        public Task<PagedResult<CustomerViewListModel>> GetAllProduct(ProductPagingRequest pagingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Repository.DatabaseModels.Customer> GetCustomerById(string id)
        {
            return await _customerRepo.Get(id);
        }

        public async  Task<bool> UpdateCustomer(CustomerUpdateMOdel customerUpdateMOdel)
        {
            Repository.DatabaseModels.Customer customer = await _customerRepo.Get(customerUpdateMOdel.CustomerId);
            if (customer == null)
            {
                return false;
            }
            customer.Fullname = customerUpdateMOdel.CustomerFullName;

            customer.Email = customerUpdateMOdel.CustomerEmail;
            customer.Gender = customerUpdateMOdel.Gender;
            customer.Dob = customerUpdateMOdel.Dob;
            customer.ImageUrl = customerUpdateMOdel.ImageUrl;
            Repository.DatabaseModels.CustomerAddress cusaddress = await _customerRepo.GetAddressCustomer(customerUpdateMOdel.CustomerId);
            if (cusaddress == null)
            {
                //create new dynamicadress from customeraadress
                Repository.DatabaseModels.CustomerAddress customerAddress = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = customerUpdateMOdel.CustomerId,
                    AddressId = Guid.NewGuid().ToString(),
                    MainAddress = true,

                };
                Repository.DatabaseModels.DynamicAddress dynamicAddress = new()
                {
                    Id = customerAddress.AddressId,
                    CityId = customerUpdateMOdel.CityId,
                    DistrictId = customerUpdateMOdel.DistrictId,
                    WardId = customerUpdateMOdel.WardId,
                };
            }
            else
            {
                //find customeraddress by customerid
                Repository.DatabaseModels.CustomerAddress customerAddress = await _customerRepo.GetAddressCustomer(customerUpdateMOdel.CustomerId);
                //update dynamicaddress
                Repository.DatabaseModels.DynamicAddress dynamicAddress = await _dynamicAdressRepo.Get(customerAddress.AddressId);
                dynamicAddress.CityId = customerUpdateMOdel.CityId;
                dynamicAddress.DistrictId = customerUpdateMOdel.DistrictId;
                dynamicAddress.WardId = customerUpdateMOdel.WardId;

            }
            await _customerRepo.Update();
            await _dynamicAdressRepo.Update();

            return true;

        }
    }
}
