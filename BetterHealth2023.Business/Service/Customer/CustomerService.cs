using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly ICustomerPointRepo _customerpointRepo;
        private readonly IDynamicAddressRepo _dynamicAdressRepo;
        private readonly ICustomerAddressRepo _customerAddressRepo;
        public CustomerService(ICustomerRepo customerRepo, ICustomerPointRepo customerpointRepo, IDynamicAddressRepo dynamicaddressAdressRepo, ICustomerAddressRepo customerAddressRepo)
        {
            _customerRepo = customerRepo;
            _customerpointRepo = customerpointRepo;
            _dynamicAdressRepo = dynamicaddressAdressRepo;
            _customerAddressRepo = customerAddressRepo;
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
            await _customerRepo.Insert(customer);
            //insert customer poitn
            CustomerPoint customerPoint = new()
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = customer.Id,
                Point = 0,
                IsPlus = true,
                Description = "Tạo tài khoản thành công",
                CreateDate = DateTime.Now,
            };
            await _customerpointRepo.Insert(customerPoint);
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
                Token = customerToken,
                PhoneNo = phoneNo
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
    }
}
