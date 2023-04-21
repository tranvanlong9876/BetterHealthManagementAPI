using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCancelModels
{
    public class OrderCancelModel
    {
        /// <summary>
        /// Mã đơn hàng để hủy đơn hàng
        /// </summary>
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        /// Lý do từ chối đơn hàng, yêu cầu tối thiểu 10 kí tự
        /// </summary>
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Lý do hủy đơn hàng cần tối thiểu 10-500 kí tự")]
        public string Reason { get; set; }

        /// <summary>
        /// Địa chỉ Ip khách hàng hoặc nhân viên để làm việc với VNPay khi cần thiết.
        /// </summary>
        [Required]
        public string IpAddress { get; set; }
    }
}
