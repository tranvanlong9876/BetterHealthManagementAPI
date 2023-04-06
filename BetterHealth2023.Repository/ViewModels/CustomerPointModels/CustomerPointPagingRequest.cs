using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerPointModels
{
    public class CustomerPointPagingRequest : PagingRequestBase
    {
        /// <summary>
        /// Sắp xếp hiển thị: true (gần hiện tại nhất), false (trễ nhất)
        /// </summary>
        [Required]
        public bool sortDateBySoonest { get; set; }

        /// <summary>
        /// Bộ lọc sử dụng điểm: True (hiển thị điểm tích lũy được cộng), False (hiển thị điểm tích lũy bị trừ) 
        /// </summary>
        public bool? FilterIsPlus { get; set; }
    }
}
