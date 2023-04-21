using BarcodeLib;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public static class EmailService
    {
        private static SmtpClient _smtpClient = new SmtpClient();
        public static string body;
        private static IConfiguration _configuration;

        private static string CHECKOUTCARTPARTIAL_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/CheckOutCartPartial.html?alt=media&token=3f56dec3-a0a6-4ffb-8be6-4dbfd10ff6c5";
        private static string CHECKOUTCARTTEMPLATE_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/CheckOutCartTemplate.html?alt=media&token=3ebbd290-eb8d-4d37-ba4c-f1a3559d143b";
        private static string INTERNALUSER_REGISTER_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/InternalUserRegisteration.html?alt=media&token=a1114ea3-41af-4c4c-ba5a-2058d0952ff5";
        private static string BETTERHEALTH_LOGO_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/BetterHealth-Logo.png?alt=media&token=d0832ae8-eabb-4642-afdf-ebdf93b587ae";

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static async Task<bool> SendEmailAsync(string receiverEmail, string subject, bool isHtml, AlternateView alternateView)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(receiverEmail);
                mail.From = new MailAddress(_configuration.GetSection("SendEmail:LoginAccount").Value, "Cửa hàng dược phẩm BetterHealth", Encoding.UTF8);
                mail.Subject = subject;
                mail.Body = body.ToString();
                mail.IsBodyHtml = isHtml;
                mail.Priority = MailPriority.High;
                if (alternateView != null) mail.AlternateViews.Add(alternateView);
                _smtpClient.Host = "smtp.gmail.com";
                _smtpClient.Port = 587;
                _smtpClient.Credentials = new NetworkCredential(_configuration.GetSection("SendEmail:LoginAccount").Value, _configuration.GetSection("SendEmail:LoginPassword").Value);
                _smtpClient.EnableSsl = true;
                _smtpClient.UseDefaultCredentials = false;
                await _smtpClient.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task SendCustomerInvoiceEmail(List<SendingEmailProductModel> sendingEmailProductModels, CheckOutOrderModel checkOutOrderModel, string address)
        {
            DateTime createdDate = CustomDateTime.Now;
            string cartHtml = string.Empty;
            string dynamicRow = await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTPARTIAL_URL);
            for (int i = 0; i < sendingEmailProductModels.Count; i++)
            {
                var productModel = sendingEmailProductModels[i];
                string eachRow = dynamicRow;
                //string eachRow = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\WebTemplate\CheckOutCartPartial.html");
                eachRow = eachRow.Replace("{{imageUrl}}", "cid:productImage" + (i + 1));
                eachRow = eachRow.Replace("{{productName}}", productModel.ProductName);
                eachRow = eachRow.Replace("{{unitName}}", productModel.UnitName);
                eachRow = eachRow.Replace("{{productPrice}}", ConvertToVietNamCurrency(productModel.DiscountPrice));
                eachRow = eachRow.Replace("{{productQuantity}}", productModel.Quantity.ToString());
                eachRow = eachRow.Replace("{{productPriceTotal}}", ConvertToVietNamCurrency(productModel.TotalPrice));

                cartHtml = cartHtml + eachRow;
            }
            body = await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTTEMPLATE_URL);
            body = body.Replace("{{orderId}}", checkOutOrderModel.OrderId);
            body = body.Replace("{{createdDate}}", "(" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", createdDate) + ")");
            body = body.Replace("{{websiteUrl}}", _configuration.GetSection("SendEmail:HomeUrl").Value);
            //Thông tin thanh toán
            if (checkOutOrderModel.PayType.Equals(2))
            {
                body = body.Replace("{{paymentMethod}}", "VN Pay - " + (checkOutOrderModel.isPaid ? "Đã Thanh Toán" : "Chưa Thanh Toán"));
            }
            else
            {
                body = body.Replace("{{paymentMethod}}", "Thanh toán khi nhận hàng");
            }

            //Giao hàng
            if (checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
            {
                body = body.Replace("{{header_orderType}}", "Thông tin giao hàng");
                body = body.Replace("{{address}}", "Địa chỉ giao hàng: " + address);
                body = body.Replace("{{more_info}}", "Thời gian giao hàng dự kiến: " + GetEstimateDeliveryTime(createdDate));
                body = body.Replace("{{other_fee}}", "Phí vận chuyển");
                body = body.Replace("{{shipping_fee}}", ConvertToVietNamCurrency(checkOutOrderModel.ShippingPrice));
            }

            //Đến lấy
            if (checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP))
            {
                body = body.Replace("{{header_orderType}}", "Thông tin hẹn lấy");
                body = body.Replace("{{address}}", "Địa chỉ cửa hàng: " + address);
                body = body.Replace("{{more_info}}", "Thời gian hẹn: " + checkOutOrderModel.OrderPickUp.DatePickUp + " " + checkOutOrderModel.OrderPickUp.TimePickUp);
                body = body.Replace("{{other_fee}}", "Chi phí khác");
                body = body.Replace("{{shipping_fee}}", ConvertToVietNamCurrency(0));
            }

            body = body.Replace("{{cart}}", cartHtml);
            body = body.Replace("{{email}}", checkOutOrderModel.ReveicerInformation.Email);
            body = body.Replace("{{name}}", checkOutOrderModel.ReveicerInformation.Fullname);
            body = body.Replace("{{phone}}", checkOutOrderModel.ReveicerInformation.PhoneNumber);
            body = body.Replace("{{sub_total}}", ConvertToVietNamCurrency(checkOutOrderModel.SubTotalPrice));
            body = body.Replace("{{discount_total}}", ConvertToVietNamCurrency(checkOutOrderModel.DiscountPrice));
            body = body.Replace("{{total_order}}", ConvertToVietNamCurrency(checkOutOrderModel.TotalPrice));
            body = body.Replace("{{login_url}}", _configuration.GetSection("SendEmail:LoginURL").Value);
            body = body.Replace("{{hotline_phone}}", _configuration.GetSection("SendEmail:HotlineNumber").Value);
            body = body.Replace("{{support_email}}", _configuration.GetSection("SendEmail:SupportEmail").Value);


            var webClient = new WebClient();
            AlternateView alternateView = default(AlternateView);
            alternateView = AlternateView.CreateAlternateViewFromString(body.ToString(), null, "text/html");
            LinkedResource linkedResourceLogo = AddImagesToEmail(BETTERHEALTH_LOGO_URL, "betterhealth-logo", webClient);
            alternateView.LinkedResources.Add(linkedResourceLogo);
            alternateView.LinkedResources.Add(AddBarCodeToEmail(checkOutOrderModel.OrderId));
            for (int i = 0; i < sendingEmailProductModels.Count; i++)
            {
                var sendingEmailProductModel = sendingEmailProductModels[i];
                LinkedResource linkedResource = AddImagesToEmail(sendingEmailProductModel.imageUrl, "productImage" + (i + 1), webClient);
                alternateView.LinkedResources.Add(linkedResource);
            }

            await EmailService.SendEmailAsync(checkOutOrderModel.ReveicerInformation.Email, "BetterHealth: Xác nhận đơn hàng #" + checkOutOrderModel.OrderId, true, alternateView);
        }

        public static string ConvertToVietNamCurrency(double price)
        {
            return price.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + "đ";
        }

        public static string GetEstimateDeliveryTime(DateTime createdOrderTime)
        {
            if (createdOrderTime.Hour > 19)
            {
                createdOrderTime = createdOrderTime.AddDays(1);
                return "Giao trước 11:00 trưa " + GetVietnamDayOfWeek((int)createdOrderTime.DayOfWeek) + " (ngày mai) (" + String.Format("{0:dd/MM/yyyy}", createdOrderTime) + ")";
            }
            else if (createdOrderTime.Minute > 30)
            {
                return "Dự kiến giao từ " + (createdOrderTime.Hour + 2) + ":00 " + "- " + (createdOrderTime.Hour + 3) + ":00 " + "trong ngày hôm nay";
            }
            else
            {
                return "Dự kiến giao từ " + (createdOrderTime.Hour + 1) + ":00 " + "- " + (createdOrderTime.Hour + 2) + ":00 " + "trong ngày hôm nay";
            }
        }

        private static string GetVietnamDayOfWeek(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return "Thứ hai";
                case 2: return "Thứ ba";
                case 3: return "Thứ tư";
                case 4: return "Thứ năm";
                case 5: return "Thứ sáu";
                case 6: return "Thứ bảy";
                case 0: return "Chủ Nhật";
                default: return "";
            }
        }

        public static async Task SendWelcomeEmailAsync(RegisterInternalUser registerEmployee, string randomPassword, string subject, bool isHtml)
        {
            string roleName = String.Empty;
            string imageURL = registerEmployee.ImageUrl;

            if (registerEmployee.RoleId == Commons.MANAGER)
            {
                roleName = "Quản Lý Chi Nhánh";
            }
            else if (registerEmployee.RoleId == Commons.PHARMACIST)
            {
                roleName = "Nhân Viên";
            }
            else if (registerEmployee.RoleId == Commons.OWNER)
            {
                roleName = "Chủ sở hữu";
            }
            else if (registerEmployee.RoleId == Commons.ADMIN)
            {
                roleName = "Quản lý tài khoản";
            }

            string WORKING_SITE_INFORMATION_TEMPLATE = "<tr>\r\n" +
                                                       "    <td class=\"attributes_item\">\r\n" +
                                                       "        <span class=\"f-fallback\">\r\n" +
                                                       "            <strong>{{head}}</strong> {{child}}\r\n" +
                                                       "        </span>\r\n" +
                                                       "    </td>\r\n" +
                                                       "</tr>";

            body = await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(INTERNALUSER_REGISTER_URL);
            body = body.Replace("{{name}}", registerEmployee.Fullname);
            body = body.Replace("{{username}}", registerEmployee.Username);
            body = body.Replace("{{password}}", randomPassword);
            body = body.Replace("{{roleName}}", roleName);
            body = body.Replace("{{support_email}}", _configuration.GetSection("SendEmail:SupportEmail").Value);
            body = body.Replace("{{login_url}}", _configuration.GetSection("SendEmail:LoginURL").Value);
            body = body.Replace("{{websiteUrl}}", _configuration.GetSection("SendEmail:LoginURL").Value);

            if(registerEmployee.RoleId.Equals(Commons.PHARMACIST) || registerEmployee.RoleId.Equals(Commons.MANAGER))
            {
                var eachRow = WORKING_SITE_INFORMATION_TEMPLATE;
                var fullInformation = string.Empty;

                eachRow = eachRow.Replace("{{head}}", "Tên Chi Nhánh Làm Việc: ");
                eachRow = eachRow.Replace("{{child}}", registerEmployee.SiteName);
                fullInformation += eachRow;

                eachRow = WORKING_SITE_INFORMATION_TEMPLATE;
                eachRow = eachRow.Replace("{{head}}", "Địa Chỉ Chi Nhánh: ");
                eachRow = eachRow.Replace("{{child}}", registerEmployee.SiteAddress);
                fullInformation += eachRow;

                body = body.Replace("{{employee_site_information}}", fullInformation);
            } else
            {
                body = body.Replace("{{employee_site_information}}", "");
            }

            var webClient = new WebClient();
            AlternateView alternateView = default(AlternateView);
            alternateView = AlternateView.CreateAlternateViewFromString(body.ToString(), null, "text/html");
            alternateView.LinkedResources.Add(AddImagesToEmail(imageURL, "image1", webClient));
            alternateView.LinkedResources.Add(AddImagesToEmail(BETTERHEALTH_LOGO_URL, "betterhealth-logo", webClient));

            await EmailService.SendEmailAsync(registerEmployee.Email, subject, isHtml, alternateView);
        }

        public static LinkedResource AddImagesToEmail(string imageURL, string contentId, WebClient webClient)
        {
            MemoryStream ms = new MemoryStream(webClient.DownloadData(imageURL));
            LinkedResource linkedResource = new LinkedResource(ms, MediaTypeNames.Image.Jpeg);
            linkedResource.ContentId = contentId;
            linkedResource.TransferEncoding = TransferEncoding.Base64;
            return linkedResource;
        }

        public static LinkedResource AddBarCodeToEmail(string data)
        {
            Barcode barcode = new Barcode();
            var image = barcode.Encode(TYPE.CODE128, data, Color.Black, Color.Transparent, 300, 100);

            var memoryStream = new MemoryStream();

            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            var linkedResource = new LinkedResource(memoryStream, "image/png");
            linkedResource.ContentId = "orderid-barcode";
            return linkedResource;
        }
    }
}
