using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.PhoneOTPRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly IPhoneOTPRepo _phoneOTPRepos;
        private readonly ICustomerRepo _customerRepo;

        public CustomerService(IPhoneOTPRepo phoneOTPRepos, ICustomerRepo customerRepo)
        {
            _phoneOTPRepos = phoneOTPRepos;
            _customerRepo = customerRepo;
        }

        public async Task<CustomerLoginStatus> customerLoginPhoneOTP(LoginCustomerModel loginPhoneOTPModel)
        {
            CustomerLoginStatus checkError = new CustomerLoginStatus();
            //check Valid
            bool check = await _phoneOTPRepos.VerifyPhoneOTP(loginPhoneOTPModel);
            if (!check)
            {
                checkError.isError = true;
                checkError.InvalidPhoneOTP = "Mã xác thực không đúng hoặc đã quá hạn, vui lòng thử lại";
                return checkError;
            }

            //get customer's information
            var customer = await _customerRepo.getCustomerBasedOnPhoneNo(loginPhoneOTPModel.phoneNo);
            
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
