using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.VNPay
{
    public interface IVNPayService
    {
        public string CreatePaymentUrl(VNPayInformationModel model);
        //PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
