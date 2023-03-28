using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerPointServices
{
    public class CustomerPointService : ICustomerPointService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly ICustomerPointRepo _customerPointRepo;

        public CustomerPointService(ICustomerRepo customerRepo, ICustomerPointRepo customerPointRepo)
        {
            _customerRepo = customerRepo;
            _customerPointRepo = customerPointRepo;
        }

        public async Task<IActionResult> GetCustomerAvailablePoint(string phoneNo)
        {
            var customer = await _customerRepo.getCustomerBasedOnPhoneNo(phoneNo);

            if (customer == null) return new NotFoundObjectResult("Không tìm thấy khách hàng dựa trên SĐT này!");

            var currentPoint = await _customerPointRepo.GetCustomerPointBasedOnCustomerId(customer.Id);

            return new OkObjectResult(currentPoint);

        }
    }
}
