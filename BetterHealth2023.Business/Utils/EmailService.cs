using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public static class EmailService
    {
        private static SmtpClient _smtpClient = new SmtpClient();
        public static string body;
        private static IConfiguration _configuration;

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
                if(alternateView != null) mail.AlternateViews.Add(alternateView);
                _smtpClient.Host = "smtp.gmail.com";
                _smtpClient.Port = 587;
                _smtpClient.Credentials = new NetworkCredential(_configuration.GetSection("SendEmail:LoginAccount").Value, _configuration.GetSection("SendEmail:LoginPassword").Value);
                _smtpClient.EnableSsl = true;
                _smtpClient.UseDefaultCredentials = false;
                await _smtpClient.SendMailAsync(mail);
                return true;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<bool> SendCustomerInvoiceEmail(List<SendingEmailProductModel> sendingEmailProductModels, CheckOutOrderModel checkOutOrderModel, string address)
        {
            DateTime createdDate = DateTime.Now;
            string cartHtml = string.Empty;
            for(int i = 0; i < sendingEmailProductModels.Count; i++)
            {
                var productModel = sendingEmailProductModels[i];
                string eachRow = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\WebTemplate\CheckOutCartPartial.html");
                eachRow = eachRow.Replace("{{imageUrl}}", "cid:productImage" + (i + 1));
                eachRow = eachRow.Replace("{{productName}}", productModel.ProductName);
                eachRow = eachRow.Replace("{{productPrice}}", ConvertToVietNamCurrency(productModel.OriginalPrice));
                eachRow = eachRow.Replace("{{productDiscountPrice}}", ConvertToVietNamCurrency(productModel.OriginalPrice - productModel.DiscountPrice));
                eachRow = eachRow.Replace("{{productQuantity}}", productModel.Quantity.ToString());
                eachRow = eachRow.Replace("{{productPriceTotal}}", ConvertToVietNamCurrency(productModel.TotalPrice));

                cartHtml = cartHtml + eachRow;
            }
            body = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\WebTemplate\CheckOutCartTemplate.html");
            body = body.Replace("{{orderId}}", checkOutOrderModel.OrderId);
            body = body.Replace("{{createdDate}}", "(" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", createdDate) + ")");

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


            var webClient = new WebClient();
            AlternateView alternateView = default(AlternateView);
            alternateView = AlternateView.CreateAlternateViewFromString(body.ToString(), null, "text/html");
            for(int i = 0; i < sendingEmailProductModels.Count; i++)
            {
                var sendingEmailProductModel = sendingEmailProductModels[i];
                LinkedResource linkedResource = AddImagesToEmail(sendingEmailProductModel.imageUrl, "productImage" + (i + 1), webClient);
                alternateView.LinkedResources.Add(linkedResource);
            }

            return await EmailService.SendEmailAsync(checkOutOrderModel.ReveicerInformation.Email, "BetterHealth: Xác nhận đơn hàng #" + checkOutOrderModel.OrderId, true, alternateView);
        }

        private static string ConvertToVietNamCurrency(double price)
        {
            return String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c}", price);
        }

        private static string GetEstimateDeliveryTime(DateTime createdOrderTime)
        {
            if(createdOrderTime.Hour > 19)
            {
                createdOrderTime = createdOrderTime.AddDays(1);
                return "Giao trước 11:00 trưa " + GetVietnamDayOfWeek((int)createdOrderTime.DayOfWeek) + " (ngày mai) (" + String.Format("{0:dd/MM/yyyy}", createdOrderTime) + ")";
            }
            else if(createdOrderTime.Minute > 30)
            {
                return "Dự kiến giao từ " + createdOrderTime.Hour + 2 + ":00 " + "- " + createdOrderTime.Hour + 3 + ":00 " + "trong ngày hôm nay";
            } else
            {
                return "Dự kiến giao từ " + createdOrderTime.Hour + 1 + ":00 " + "- " + createdOrderTime.Hour + 2 + ":00 " + "trong ngày hôm nay";
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

        public static async Task<bool> SendWelcomeEmail(RegisterInternalUser registerEmployee, string randomPassword, string subject, bool isHtml)
        {
            string roleName = String.Empty;
            string imageURL = registerEmployee.ImageUrl;

            if (registerEmployee.RoleId == Commons.MANAGER)
            {
                roleName = "Quản Lý chi nhánh";
            } else if(registerEmployee.RoleId == Commons.PHARMACIST)
            {
                roleName = "Dược sĩ";
            } else if (registerEmployee.RoleId == Commons.OWNER)
            {
                roleName = "Chủ sở hữu";
            } else if (registerEmployee.RoleId == Commons.ADMIN)
            {
                roleName = "Quản lý tài khoản";
            }
            else
            {
                return false;
            }
            /*emailService._body.Append("<html>");
            emailService._body.Append($"<p1>Xin chào <b> {registerEmployee.Fullname} </b>,</p1> <br/> <br/>");
            emailService._body.Append("Chúc mừng, hôm nay là một ngày bạn đã về đội đồng hành của chúng tôi. <br/>");
            emailService._body.Append("Hiện tại ban quản lý cũng như đại diện cho toàn hệ thống BetterHealth xin phép cung cấp tài khoản đăng nhập của bạn. <br/>");
            emailService._body.Append($"<img src='cid:image1' width=\"200\" height=\"200\">");
            emailService._body.Append("<br/>");
            emailService._body.Append($"<b>Tài khoản đăng nhập: </b> {registerEmployee.Username}");
            emailService._body.Append("<br/>");
            emailService._body.Append($"<b>Mật khẩu đăng nhập: </b> {randomPassword}");
            emailService._body.Append("<br/>");
            emailService._body.Append($"<b>Chức vụ của bạn là: </b> {roleName}");
            emailService._body.Append("<br/>");
            emailService._body.Append("<br/>");
            emailService._body.Append("Đây sẽ là tài khoản đăng nhập nội bộ của bạn, đừng quên thay đổi lại mật khẩu để đảm bảo an toàn bảo mật cho tài khoản. Cảm ơn bạn đã hợp tác cùng chuỗi cửa hàng dược phẩm BetterHealth! <br/><br/>");
            emailService._body.Append("Trân trọng. BetterHealth Company!");
            emailService._body.Append("</html>");
            */
            body = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\WebTemplate\InternalUserRegisteration.html");
            body = body.Replace("{{name}}", registerEmployee.Fullname);
            body = body.Replace("{{username}}", registerEmployee.Username);
            body = body.Replace("{{password}}", randomPassword);
            body = body.Replace("{{roleName}}", roleName);
            body = body.Replace("{{support_email}}", _configuration.GetSection("SendEmail:SupportEmail").Value);
            body = body.Replace("{{login_url}}", _configuration.GetSection("SendEmail:LoginURL").Value);

            var webClient = new WebClient();
            AlternateView alternateView = default(AlternateView);
            alternateView = AlternateView.CreateAlternateViewFromString(body.ToString(), null, "text/html");
            LinkedResource linkedResource = AddImagesToEmail(imageURL, "image1", webClient);
            alternateView.LinkedResources.Add(linkedResource);

            return await EmailService.SendEmailAsync(registerEmployee.Email, subject, isHtml, alternateView);
        }

        private static LinkedResource AddImagesToEmail(string imageURL, string contentId, WebClient webClient)
        {
            MemoryStream ms = new MemoryStream(webClient.DownloadData(imageURL));
            LinkedResource linkedResource = new LinkedResource(ms, MediaTypeNames.Image.Jpeg);
            linkedResource.ContentId = contentId;
            linkedResource.TransferEncoding = TransferEncoding.Base64;
            return linkedResource;
        }
    }
}
