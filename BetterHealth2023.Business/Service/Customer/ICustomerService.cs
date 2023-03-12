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
        public Task<Repository.DatabaseModels.Customer> CreateCustomer(CustomerRegisView customerRegisView);
        // update customer
        public Task<bool> UpdateCustomer(CustomerUpdateMOdel customerUpdateMOdel);

        public Task<List<CustomerUpdateMOdel>> GetCustomerPaging();

        public Task<CustomerUpdateMOdel> GetCustomerById(string id);
        public Task<PagedResult<CustomerUpdateMOdel>> GetCustomerPaging2(CustomerPagingRequest pagingRequest);

    }
}
