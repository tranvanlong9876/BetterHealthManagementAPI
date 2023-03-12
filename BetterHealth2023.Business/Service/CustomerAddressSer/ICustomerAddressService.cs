using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerAddressSer
{
    public interface ICustomerAddressService
    {
        public  Task<ActionResult> RemoveCustomerAddressById(string id);
        public Task<ActionResult> InseartCustomerAddress(CustomerAddressInsertModel CustomerAddressInsertModel);
    }
}
