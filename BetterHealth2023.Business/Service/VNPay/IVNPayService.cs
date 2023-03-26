using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.VNPay
{
    public interface IVNPayService
    {
        public string CreatePaymentUrl(VNPayInformationModel model);
        public Task<IActionResult> QueryExistingPaymentAsync(QueryVNPayModel queryVNPay);
        //PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
