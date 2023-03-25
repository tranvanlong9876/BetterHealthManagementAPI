using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderStatusModels
{
    public class OrderStatusFilterRequest
    {
        /// <summary>
        /// OrderTypeId: 1 là Tại Chỗ, 2 là Đến lấy, 3 là Giao hàng.
        /// </summary>
        [Required]
        [Range(1, 3, ErrorMessage = "Mã của loại đơn hàng không hợp lệ.")]
        public int OrderTypeId { get; set; }
    }
}
