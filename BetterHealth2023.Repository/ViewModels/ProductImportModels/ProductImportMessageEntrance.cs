using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductImportModels
{
    public class ProductImportMessageEntrance
    {
        /// <summary>
        /// Mã Sản Phẩm, lấy từ Id của trong Dropdown List chọn đơn vị nhập hàng.
        /// </summary>
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// Số lượng Nhập hàng, lấy từ số lượng tổng của Manager, chỉ nên truyền/gọi api khi lớn hơn 0.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Đừng gọi API vội, tổng Quantity lớn hơn 1 hẳn gọi.")]
        public int Quantity { get; set; }
    }
}
