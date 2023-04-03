using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ProductPagingHomePageRequest : PagingRequestBase
    {
        /// <summary>
        /// Phương pháp load sản phẩm: 1 (bán chạy), 2 (mới nhất)
        /// </summary>
        [Required]
        [Range(1, 2, ErrorMessage = "Không được vượt quá 1 và 2")]
        public int GetProductType { get; set; }
    }
}
