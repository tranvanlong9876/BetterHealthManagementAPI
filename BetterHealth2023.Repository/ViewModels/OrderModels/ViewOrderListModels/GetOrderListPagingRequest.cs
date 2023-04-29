using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels
{
    public class GetOrderListPagingRequest : PagingRequestBase
    {
        /// <summary>
        /// Tra cứu mã đơn hàng, sđt khách hàng
        /// </summary>
        public string OrderIdOrPhoneNo { get; set; }
        /// <summary>
        /// Filter theo loại đơn hàng
        /// </summary>
        public int? OrderTypeId { get; set; }

        /// <summary>
        /// Filter các đơn hàng chưa hoàn thành (false), đã hoàn thành (true)
        /// </summary>
        public bool? isCompleted { get; set; }
        /// <summary>
        /// Truyền true (đối với đơn chưa nhận, false đối với đơn đã nhận)
        /// </summary>
        public bool? NotAcceptable { get; set; }

        /// <summary>
        /// Hiển thị đơn duy nhất của Pharmacist trong token. Chỉ nên sử dụng khi NotAcceptable = false.
        /// Không thực hiện bộ lọc khi truyền False hoặc không truyền (null).
        /// </summary>
        public bool? ShowOnlyPharmacist { get; set; }
    }
}
