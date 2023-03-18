using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.CustomerErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer
{
    public interface ICustomerService
    {
        public Task<CustomerLoginStatus> customerLoginPhoneOTP(LoginCustomerModel loginPhoneOTPModel);
        // insert customer
        public Task<Repository.DatabaseModels.Customer> CreateCustomer(RegisterCustomerModel customerRegisView);
        // update customer
        public Task<bool> UpdateCustomer(CustomerUpdateModel customerUpdateMOdel);
        public Task<CustomerViewSpecificModel> GetCustomerById(string id);
        public Task<PagedResult<CustomerViewListModel>> GetCustomerPagingRequest(CustomerPagingRequest pagingRequest);

    }
}
