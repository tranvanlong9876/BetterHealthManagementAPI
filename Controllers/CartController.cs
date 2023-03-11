using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CartService;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
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
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartFromFirebase(string phoneNo)
        {
            try
            {
                var listCart = await _cartService.GetCart(phoneNo);
                return Ok(listCart);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCart(Cart cart)
        {
            try
            {
                var check = await _cartService.UpdateCart(cart);
                if (check)
                {
                    return Ok("Thêm sản phẩm vào giỏ hàng thành công");
                }
                else
                {
                    return BadRequest("Lỗi thêm sản phẩm");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveItemFromCart(RemoveItemFromCart removeItemFromCart)
        {
            try
            {
                var check = await _cartService.RemoveItemFromCart(removeItemFromCart.productId, removeItemFromCart.customerIdOrIpAddress);
                if (check) return Ok("Đã xóa vật phẩm khỏi giỏ hàng thành công");

                return BadRequest("Lỗi");
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
