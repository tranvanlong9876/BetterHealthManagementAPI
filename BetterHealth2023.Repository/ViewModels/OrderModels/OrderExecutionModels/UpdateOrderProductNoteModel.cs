using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels
{
    public class UpdateOrderProductNoteModel
    {
        [SwaggerSchema(Description = "Mã OrderDetail lấy từ Id trong mảng OrderProducts")]
        [Required]
        public string OrderDetailId { get; set; }
        public string Note { get; set; }
    }
}
