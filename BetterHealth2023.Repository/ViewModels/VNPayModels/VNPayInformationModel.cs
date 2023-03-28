using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels
{
    public class VNPayInformationModel
    {
        /// <summary>
        /// Giá trị đơn hàng: Lấy thành tiền của đơn hàng truyền vào đây (kiểu double, không ngăn cách giữa dấu phẩy hoặc chấm).
        /// </summary>
        [Required]
        public double Amount { get; set; }

        /// <summary>
        /// Mã đơn hàng thanh toán.
        /// </summary>
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        /// Địa chỉ Ip Khách hàng.
        /// </summary>
        [Required]
        public string IpAddress { get; set; }

        /// <summary>
        /// Url trả về sau khi khách hàng thanh toán xong, trả về url sau đó thực hiện checkout xuống BE/Database.
        /// </summary>
        [Required]
        public string UrlCallBack { get; set; }
    }
}
