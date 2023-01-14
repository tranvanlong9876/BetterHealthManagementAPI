using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels
{
    public class PagingRequestBase
    {
        //trang hiện tại.
        [Required]
        public int pageIndex { get; set; }

        [Required]
        //số lượng mỗi trang.
        public int pageItems { get; set; }
    }
}
