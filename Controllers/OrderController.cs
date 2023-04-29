using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DateTimeModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCancelModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderValidateModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Danh Sách Đơn Hàng")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetOrderListPagingRequest pagingRequest)
        {
            var userInformation = new UserInformation();
            userInformation.UserAccessToken = GetWholeToken();
            if (string.IsNullOrEmpty(userInformation.UserAccessToken))
            {
                if (string.IsNullOrEmpty(pagingRequest.OrderIdOrPhoneNo))
                {
                    return BadRequest("Khách hàng không đăng nhập phải nhập SĐT mới có thể tra cứu đơn hàng");
                }
            }
            else
            {
                if (!JwtUserToken.ValidateToken(userInformation.UserAccessToken)) return BadRequest("Token không hợp lệ");
            }
            var orderList = await _orderService.GetAllOrders(pagingRequest, userInformation);

            return Ok(orderList);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrder(string id)
        {
            var userInformation = new UserInformation();
            userInformation.UserAccessToken = GetWholeToken();
            if (!string.IsNullOrEmpty(userInformation.UserAccessToken) && !JwtUserToken.ValidateToken(userInformation.UserAccessToken)) return BadRequest("Token không hợp lệ");
            var order = await _orderService.GetSpecificOrder(id, userInformation);
            if (order == null) return NotFound("Không tìm thấy đơn hàng, có thể là sai ID");
            return Ok(order);
        }

        [HttpGet("PickUp/Site")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSiteListPickUp([FromQuery] CartEntrance productEntrance)
        {
            var check = await _orderService.GetViewSiteToPickUps(productEntrance);
            if (check.isError)
            {
                return BadRequest(check);
            }

            return Ok(check.siteListPickUp);
        }

        [HttpGet("GenerateOrderId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRandomOrderId()
        {
            var orderId = await _orderService.GenerateOrderId();
            if (orderId == null) return BadRequest("Service Not Available");
            return Ok(orderId);
        }

        [HttpGet("PickUp/DateAvailable")]
        [AllowAnonymous]
        public IActionResult GetDatePickUpAvailable()
        {
            var currentHour = CustomDateTime.Now.Hour;
            var dateTime = CustomDateTime.Now.Date;
            int load = currentHour >= 18 ? 1 : 0;
            List<PickUpDateModel> dateTimes = new List<PickUpDateModel>();
            for (var i = load; i < 10; i++)
            {
                var nextday = dateTime.AddDays(i);
                var dayofWeek = nextday.DayOfWeek;
                string strDate = String.Format("{0:dd/MM/yyyy}", nextday);
                dateTimes.Add(new PickUpDateModel()
                {
                    dateTime = nextday.ToString("MM-dd-yyyy"),
                    dayofWeekAndDate = GetVietnamDayOfWeek((int)dayofWeek) + " " + strDate
                });
            }

            return Ok(dateTimes);
        }

        [HttpGet("PickUp/{dateTime}/TimeAvailable/")]
        [AllowAnonymous]
        public IActionResult GetTimeAvailable(DateTime dateTime)
        {
            var currentDateTime = CustomDateTime.Now;
            List<string> timeAvailables = new List<string>();
            if (currentDateTime.ToShortDateString().Equals(dateTime.ToShortDateString()))
            {
                for (int i = (currentDateTime.Hour + 2); i < 20; i++)
                {
                    timeAvailables.Add($"{i}:00 - {i + 1}:00");
                }

                return Ok(timeAvailables);
            }
            else
            {
                for (int i = 8; i < 20; i++)
                {
                    timeAvailables.Add($"{i}:00 - {i + 1}:00");
                }
                return Ok(timeAvailables);
            }

            //return Ok(currentTime);
        }

        [HttpPost("Checkout")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "API dùng để đặt hàng. OrderTypeId: 1 (tại chỗ), 2 (đến lấy tại cửa hàng), 3 (giao hàng)", Description = "Đối với đơn hàng đặt tại chỗ, phải có đầy đủ PharmacistId và SiteId. Đơn hàng đến lấy phải có SiteId.")]
        public async Task<IActionResult> CheckOutOrder(CheckOutOrderModel checkOutOrderModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(GetWholeToken()))
                {
                    if (!JwtUserToken.ValidateToken(GetWholeToken()))
                    {
                        return BadRequest("Token không hợp lệ");
                    }
                }
                return await _orderService.CheckOutOrder(checkOutOrderModel, GetWholeToken());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.StackTrace);
            }
        }

        [HttpPut("ValidateOrder")]
        [Authorize(Roles = Commons.PHARMACIST_NAME)]
        [SwaggerOperation(Summary = "API tiếp nhận đơn hàng, nếu từ chối đơn hàng (IsAccept = false), bắt buộc phải nêu lý do cụ thể.")]
        public async Task<IActionResult> ValidateOrder(ValidateOrderModel validateOrderModel)
        {
            var token = GetWholeToken();
            return await _orderService.ValidateOrder(validateOrderModel, token);
        }

        [HttpPut("ExecuteOrder")]
        [Authorize(Roles = Commons.PHARMACIST_NAME)]
        [SwaggerOperation(Summary = "API xử lý quá trình đơn hàng")]
        public async Task<IActionResult> ExecuteOrder(OrderExecutionModel orderExecutionModel)
        {
            var token = GetWholeToken();
            return await _orderService.ExecuteOrder(orderExecutionModel, token);
        }

        [HttpPut("CancelOrder")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "API hủy đơn hàng. Khách hàng hoặc Nhân Viên có thể dùng", Description = "Khách hàng chỉ có thể yêu cầu hủy khi nhân viên chưa duyệt đơn. Nhân viên chỉ có thể yêu cầu hủy khi đang là người chịu trách nhiệm đơn hàng và đồng thời đơn hàng chưa hoàn thành.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Hủy Đơn Thành Công")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Không đủ điều kiện hủy đơn hàng")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Không tìm thấy đơn hàng")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Role nội bộ nhưng lại không phải Nhân Viên")]
        public async Task<IActionResult> CancelOrder(OrderCancelModel orderCancelModel)
        {
            var token = GetWholeToken();
            if (!string.IsNullOrEmpty(token))
            {
                if (!JwtUserToken.ValidateToken(token))
                {
                    return BadRequest("Token không hợp lệ");
                }
            }
            var user = new UserInformation()
            {
                UserAccessToken = token
            };
            return await _orderService.CancelOrder(orderCancelModel, user);
        }

        [HttpPut("UpdateOrderProductNote")]
        [Authorize(Roles = Commons.PHARMACIST_NAME)]
        [SwaggerOperation(Summary = "Cho phép Pharmacist sửa ghi chú sản phẩm trong đơn hàng của khách hàng.", Description = "Sử dụng Mã Id trong mảng OrderProduct trả về từ trang View Detail, không sử dụng ProductId hoặc OrderId. Role Duy nhất được phép sử dụng: Pharmacist.")]
        public async Task<IActionResult> UpdateOrderProductNote(List<UpdateOrderProductNoteModel> productModels)
        {
            return await _orderService.UpdateOrderProductNoteModel(productModels);
        }

        [HttpGet("OrderExecutionHistory/{orderId}")]
        [SwaggerOperation(Summary = "Cho phép xem lịch sử hành trình đơn hàng.", Description = "Sử dụng OrderId để truyền vào. Trả về NotFound nếu chưa có gì cả.")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewOrderExecutionHistory(string orderId)
        {
            return await _orderService.GetOrderExecutionHistory(orderId);
        }
        private string GetWholeToken()
        {
            try
            {
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private string GetVietnamDayOfWeek(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return "Thứ hai";
                case 2: return "Thứ ba";
                case 3: return "Thứ tư";
                case 4: return "Thứ năm";
                case 5: return "Thứ sáu";
                case 6: return "Thứ bảy";
                case 0: return "Chủ Nhật";
                default: return "";
            }
        }
    }
}
