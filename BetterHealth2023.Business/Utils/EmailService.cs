using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public class EmailService : IDisposable
    {
        private SmtpClient _smtpClient;
        public StringBuilder _body;

        public EmailService()
        {
            _body = new StringBuilder();
            _smtpClient = new SmtpClient();
        }
        public void Dispose()
        {
            _body.Clear();
            _smtpClient.Dispose();
        }

        public async Task<bool> SendEmailAsync(string receiverEmail, string subject, bool isHtml, AlternateView alternateView)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(receiverEmail);
                mail.From = new MailAddress("traanvanlongoooooo@gmail.com", "Cửa hàng dược phẩm BetterHealth", Encoding.UTF8);
                mail.Subject = subject;
                mail.Body = _body.ToString();
                mail.IsBodyHtml = isHtml;
                mail.Priority = MailPriority.High;
                if(alternateView != null) mail.AlternateViews.Add(alternateView);
                _smtpClient.Host = "smtp.gmail.com";
                _smtpClient.Port = 587;
                _smtpClient.Credentials = new NetworkCredential("traanvanlongoooooo@gmail.com", "lqlyyqtshvzmaecm");
                _smtpClient.EnableSsl = true;
                _smtpClient.UseDefaultCredentials = false;
                await _smtpClient.SendMailAsync(mail);
                return true;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<bool> SendWelcomeEmail(RegisterInternalUser registerEmployee, string randomPassword, string subject, bool isHtml)
        {
            string roleName = String.Empty;

            string imageURL = registerEmployee.ImageUrl;
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(imageURL);
            MemoryStream ms = new MemoryStream(imageBytes);

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
            using EmailService emailService = new EmailService();
            emailService._body.Append("<html>");
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

            AlternateView alternateView = default(AlternateView);
            alternateView = AlternateView.CreateAlternateViewFromString(emailService._body.ToString(), null, "text/html");
            LinkedResource linkedResource = new LinkedResource(ms, MediaTypeNames.Image.Jpeg);
            linkedResource.ContentId = "image1";
            linkedResource.TransferEncoding = TransferEncoding.Base64;
            alternateView.LinkedResources.Add(linkedResource);

            return await emailService.SendEmailAsync(registerEmployee.Email, subject, isHtml, alternateView);
        }

    }
}
