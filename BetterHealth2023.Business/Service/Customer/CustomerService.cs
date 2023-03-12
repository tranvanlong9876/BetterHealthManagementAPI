using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<CustomerUpdateMOdel>> GetCustomerPaging()
        {
            List<CustomerUpdateMOdel> listView = new List<CustomerUpdateMOdel>();
            
            
            //get all customer
            List<Repository.DatabaseModels.Customer> listcus = await _customerRepo.GetAllCustomerModelView();
            //get all customeraddress by customerid



            foreach (Repository.DatabaseModels.Customer item in listcus)
            {
                CustomerUpdateMOdel cusview = new()
                {
                    CustomerId = item.Id,
                    FullName = item.Fullname,
                    Email = item.Email,
                    PhoneNo = item.PhoneNo,
                    Dob = item.Dob,
                    Gender = (int)item.Gender,
                    ImageUrl = item.ImageUrl,

                };
                List<CustomerAddress> listCusAddress = await _customerAddressRepo.GetAllCustomerAddressByCustomerId(item.Id);
                List<CustomerAddressView> listcusaddview = new List<CustomerAddressView>();
                //filter in to list<Customeraddressview>
                foreach (CustomerAddress item1 in listCusAddress)
                {
                    //new customerupdateview
                    CustomerAddressView cusaddressview = new()
                    {
                        Id = item1.Id,
                        CustomerId = item1.CustomerId,
                        AddressId = item1.AddressId,
                    };
                    DynamicAddress dynamicaddress = await _dynamicAdressRepo.Get(item1.AddressId);
                    DynamicAddressCustomerView dynamicaddresscusview = new()
                    {
                        AddressId = dynamicaddress.Id,
                        CityId = dynamicaddress.CityId,
                        DistrictId = dynamicaddress.DistrictId,
                        WardId = dynamicaddress.WardId,
                        HomeAddress = dynamicaddress.HomeAddress
                    };
                    cusaddressview.DynamicAddressCustomerView = dynamicaddresscusview;
                    
                    listcusaddview.Add(cusaddressview);
                   
                }
                cusview.CustomerAddressList = listcusaddview;
                listView.Add(cusview);

            }
            return listView;
        }
                public async Task<CustomerUpdateMOdel> GetCustomerById(string id)
        {
           
            Repository.DatabaseModels.Customer customer = await _customerRepo.Get(id);
            if(customer == null)
            {
                return null;
            }
            //get list customeraddress by id
            List<CustomerAddress> customerAddress = await _customerAddressRepo.GetAllCustomerAddressByCustomerId(id);
            List<DynamicAddressCustomerView> listdynamiview = await _dynamicAdressRepo.GetAllDynamicAddressByCusId(id);
            List<CustomerAddressView> Listcustomerview = new List<CustomerAddressView>();
            //filter in to list<Customeraddressview>
            foreach (CustomerAddress item in customerAddress)
            {
                //new customerupdatemodel
                CustomerAddressView customerAddressView = new CustomerAddressView()
                {
                    Id = item.Id,
                    CustomerId = item.CustomerId,
                    AddressId = item.AddressId,
                    
                };
                foreach(DynamicAddressCustomerView item2 in listdynamiview)
                {
                    if (item2.AddressId == item.AddressId)
                    {
                        customerAddressView.DynamicAddressCustomerView = item2;
                        continue;
                    }
                }
                Listcustomerview.Add(customerAddressView);              
            }
            CustomerUpdateMOdel customerview = new()
            {
                //filter customer inton customerview
                CustomerId = customer.Id,
                FullName = customer.Fullname,
                PhoneNo = customer.PhoneNo,
                Email = customer.Email,
                Dob = customer.Dob,
                Gender = (int)customer.Gender,
                ImageUrl = customer.ImageUrl,
                CustomerAddressList = Listcustomerview        
            };
            return customerview;
            }

     
        
        public async Task<bool> UpdateCustomer(CustomerUpdateMOdel customerUpdateMOdel)
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
        public async Task<PagedResult<CustomerUpdateMOdel>> GetCustomerPaging2(string name, string email, string phoneNo, int pageindex, int pageitem)
        {
            List<CustomerUpdateMOdel> listView = new List<CustomerUpdateMOdel>();


            //get all customer
            List<Repository.DatabaseModels.Customer> listcus = await _customerRepo.GetAllCustomerModelView();
            //get all customeraddress by customerid
            if (!string.IsNullOrEmpty(name))
            {
                listcus = listcus.Where(x => x.Fullname.Contains(name)).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                listcus = listcus.Where(x => x.Email.Contains(email)).ToList();
            }
            if (!string.IsNullOrEmpty(phoneNo))
            {
                listcus = listcus.Where(x => x.PhoneNo.Contains(phoneNo)).ToList();
            }
            int totalrecord = listcus.Count();
            List<Repository.DatabaseModels.Customer> listcusget = listcus;
            List<Repository.DatabaseModels.Customer> listcus2 = listcusget.Skip((pageindex - 1) * pageitem).Take(pageitem).ToList();

            foreach (Repository.DatabaseModels.Customer item in listcus2)
            {
                CustomerUpdateMOdel cusview = new()
                {
                    CustomerId = item.Id,
                    FullName = item.Fullname,
                    Email = item.Email,
                    PhoneNo = item.PhoneNo,
                    Dob = item.Dob,
                    Gender = (int)item.Gender,
                    ImageUrl = item.ImageUrl,

                };
                List<CustomerAddress> listCusAddress = await _customerAddressRepo.GetAllCustomerAddressByCustomerId(item.Id);
                List<CustomerAddressView> listcusaddview = new List<CustomerAddressView>();
                //filter in to list<Customeraddressview>
                foreach (CustomerAddress item1 in listCusAddress)
                {
                    //new customerupdateview
                    CustomerAddressView cusaddressview = new()
                    {
                        Id = item1.Id,
                        CustomerId = item1.CustomerId,
                        AddressId = item1.AddressId,
                    };
                    DynamicAddress dynamicaddress = await _dynamicAdressRepo.Get(item1.AddressId);
                    DynamicAddressCustomerView dynamicaddresscusview = new()
                    {
                        AddressId = dynamicaddress.Id,
                        CityId = dynamicaddress.CityId,
                        DistrictId = dynamicaddress.DistrictId,
                        WardId = dynamicaddress.WardId,
                        HomeAddress = dynamicaddress.HomeAddress
                    };
                    cusaddressview.DynamicAddressCustomerView = dynamicaddresscusview;

                    listcusaddview.Add(cusaddressview);

                }
                cusview.CustomerAddressList = listcusaddview;
                listView.Add(cusview);

            }


            var pagedResult = new PagedResult<CustomerUpdateMOdel>(listView, totalrecord, pageindex, pageitem);

            return pagedResult;

        }

        
    }

   
    }

