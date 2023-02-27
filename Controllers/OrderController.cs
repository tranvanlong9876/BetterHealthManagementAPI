using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DateTimeModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok("Test");
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

        [HttpGet("PickUp/DateAvailable")]
        [AllowAnonymous]
        public IActionResult GetDatePickUpAvailable()
        {
            var dateTime = DateTime.Today;
            List<PickUpDateModel> dateTimes = new List<PickUpDateModel>();
            for (int i = 0; i < 10; i++)
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
            var currentDateTime = DateTime.Now;
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CheckOutOrder(CheckOutOrderModel checkOutOrderModel)
        {
            var isGuest = CheckGuestUser(); //true is guest

            return Ok();
        }

        private bool CheckGuestUser()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                if (JwtUserToken.DecodeAPITokenToRole(token) == "Customer")
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

            return true;
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
