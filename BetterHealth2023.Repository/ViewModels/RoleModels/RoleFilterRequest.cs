using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.RoleModels
{
    public class RoleFilterRequest
    {
        /// <summary>
        /// Bộ lọc load Role làm việc, nếu chỉ muốn load Role Manager và Pharmacist truyền true. Ngược lại, chỉ load Owner và Admin truyền false. Không truyền không thực hiện bộ lọc.
        /// </summary>
        public bool? LoadWorkingRoleOnly { get; set; }
    }
}
