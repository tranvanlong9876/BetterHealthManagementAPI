using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.VNPay
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(VNPayInformationModel model)
        {
            var currentTime = CustomDateTime.Now;
            //var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            var pay = new VNPayLibrary();

            pay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((double)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", currentTime.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", model.IpAddress);
            pay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Thanh toan cho don hang {model.OrderId}. So tien la: {model.Amount} VND.");
            pay.AddRequestData("vnp_OrderType", _configuration["VnPay:OrderType"]);
            pay.AddRequestData("vnp_ReturnUrl", model.UrlCallBack);
            pay.AddRequestData("vnp_TxnRef", model.OrderId);
            pay.AddRequestData("vnp_ExpireDate", currentTime.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            var paymentUrl = pay.CreateRequestUrl(_configuration["VnPay:BaseUrl"], _configuration["VnPay:HashSecret"]);
            return paymentUrl;
        }

        public async Task<IActionResult> QueryExistingPaymentAsync(QueryVNPayModel queryVNPay)
        {
            VNPayLibrary payLibrary = new VNPayLibrary();
            object jsonResponse = null;
            var requestId = Guid.NewGuid().ToString();
            var createDate = CustomDateTime.Now.ToString("yyyyMMddHHmmss");
            var orderInfo = $"Truy van thong tin thanh toan don hang {queryVNPay.OrderId}.";

            payLibrary.AddRequestData("vnp_RequestId", requestId);
            payLibrary.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            payLibrary.AddRequestData("vnp_Command", "querydr");
            payLibrary.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            payLibrary.AddRequestData("vnp_TxnRef", queryVNPay.OrderId); //Load tu 
            payLibrary.AddRequestData("vnp_OrderInfo", orderInfo);
            payLibrary.AddRequestData("vnp_TransactionDate", queryVNPay.TransactionDate); //Load tu database
            payLibrary.AddRequestData("vnp_CreateDate", createDate);
            payLibrary.AddRequestData("vnp_IpAddr", queryVNPay.IpAddress);

            var dataHash = "";
            dataHash += requestId + "|";
            dataHash += _configuration["VnPay:Version"] + "|";
            dataHash += "querydr" + "|";
            dataHash += _configuration["VnPay:TmnCode"] + "|";
            dataHash += queryVNPay.OrderId + "|";
            dataHash += queryVNPay.TransactionDate + "|";
            dataHash += createDate + "|";
            dataHash += queryVNPay.IpAddress + "|";
            dataHash += orderInfo;
            payLibrary.AddRequestData("vnp_SecureHash", VNPayLibrary.HmacSHA512(_configuration["VnPay:HashSecret"], dataHash));
            using (var client = new HttpClient())
            {
                var jsonRequest = JsonConvert.SerializeObject(payLibrary.GetRequestData());

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://sandbox.vnpayment.vn/merchant_webapi/api/transaction", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                jsonResponse = JsonConvert.DeserializeObject(responseContent);
            }

            return new OkObjectResult(jsonResponse);
        }
    }
}
