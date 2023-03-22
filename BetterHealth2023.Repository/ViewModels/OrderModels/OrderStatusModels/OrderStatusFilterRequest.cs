using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderStatusModels
{
    public class OrderStatusFilterRequest
    {
        [Required]
        [Range(1, 3, ErrorMessage = "Mã của loại đơn hàng không hợp lệ.")]
        public int OrderTypeId { get; set; }
    }
}
