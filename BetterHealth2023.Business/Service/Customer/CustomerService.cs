using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
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

        public CustomerService(ICustomerRepo customerRepo, ICustomerPointRepo customerpointRepo)
        {
            _customerRepo = customerRepo;
            _customerpointRepo = customerpointRepo;
        }

        public async Task<Repository.DatabaseModels.Customer> CreateCustomer(CustomerRegisView customerRegisView)
        {
            //customer model
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
            //sinsert customerpoint
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
    }
}
