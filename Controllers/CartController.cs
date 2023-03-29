using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CartService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        private readonly IProductService _productService;
        private readonly IProductEventDiscountRepo _productEventDiscountRepo;

        public CartController(ICartService cartService, IProductService productService, IProductEventDiscountRepo productEventDiscountRepo)
        {
            _cartService = cartService;
            _productService = productService;
            _productEventDiscountRepo = productEventDiscountRepo;
        }

        [HttpPost("ApplyCustomerPoint")]
        [SwaggerOperation(Summary = "Sử dụng điểm vào Giỏ Hàng của khách hàng, lưu ý chỉ dùng cho Role Khách hàng")]
        [AllowAnonymous]
        public async Task<IActionResult> ApplyCustomerPoint(CustomerCartPoint cartPoint)
        {
            try
            {
                var check = await _cartService.UpdateCustomerCartPoint(cartPoint);
                if (check)
                {
                    return Ok("Thêm điểm sử dụng của khách vào giỏ hàng thành công");
                }
                else
                {
                    return BadRequest("Lỗi thêm điểm");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{deviceId}")]
        [SwaggerOperation(Summary = "Lấy ra toàn bộ thông tin trong giỏ hàng của khách hàng. Luôn luôn truyền mã thiết bị, không truyền SĐT Khách hàng (API tự nhận diện trong Token)")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartFromFirebase(string deviceId)
        {
            try
            {
                //Kết nối firebase
                var listCart = await _cartService.GetCart(deviceId, GetCustomerId());

                //Kết nối database
                if (listCart == null) return NotFound("Không tìm thấy giỏ hàng");
                if (!listCart.Point.HasValue) listCart.Point = 0;
                var SubTotalPrice = 0D;
                if (listCart.Items != null)
                {
                    for (int i = 0; i < listCart.Items.Count; i++)
                    {
                        var cartInformation = await _productService.AddMoreProductInformationToCart(listCart.Items[i].ProductId);
                        listCart.Items[i].Price = cartInformation.Price;
                        listCart.Items[i].ProductImageUrl = cartInformation.ProductImageUrl;
                        listCart.Items[i].ProductName = cartInformation.ProductName;
                        var productDiscount = await _productEventDiscountRepo.GetProductDiscount(listCart.Items[i].ProductId);
                        if (productDiscount != null)
                        {
                            if (productDiscount.DiscountMoney.HasValue)
                            {
                                listCart.Items[i].PriceAfterDiscount = listCart.Items[i].Price - productDiscount.DiscountMoney.Value;
                            }

                            if (productDiscount.DiscountPercent.HasValue)
                            {
                                listCart.Items[i].PriceAfterDiscount = listCart.Items[i].Price - (listCart.Items[i].Price * productDiscount.DiscountPercent.Value / 100);
                            }
                        }
                        else
                        {
                            listCart.Items[i].PriceAfterDiscount = listCart.Items[i].Price;
                        }
                        listCart.Items[i].PriceTotal = listCart.Items[i].PriceAfterDiscount * listCart.Items[i].Quantity;
                        SubTotalPrice += listCart.Items[i].PriceTotal;
                    }
                }
                listCart.SubTotalPrice = SubTotalPrice;
                listCart.DiscountPrice = listCart.Point.Value * 1000;
                listCart.TotalCartPrice = SubTotalPrice - listCart.DiscountPrice;

                return Ok(listCart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Thêm một sản phẩm vào giỏ hàng của khách hàng. Nếu giỏ hàng đã có sản phẩm thì sẽ update theo quantity vừa truyền vô. Lưu ý, truyền mã thiết bị DeviceID.")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCart(Cart cart)
        {
            try
            {
                var check = await _cartService.UpdateCart(cart, GetCustomerId());
                if (check)
                {
                    return Ok("Thêm sản phẩm vào giỏ hàng thành công");
                }
                else
                {
                    return BadRequest("Lỗi thêm sản phẩm");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Xóa một sản phẩm khỏi giỏ hàng của khách hàng. Lưu ý không truyền mã thiết bị, vui lòng truyền mã CartID (dữ liệu trong Json của API GetCart)")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveItemFromCart(RemoveItemFromCart removeItemFromCart)
        {
            try
            {
                var check = await _cartService.RemoveItemFromCart(removeItemFromCart.productId, removeItemFromCart.cartId);
                if (check) return Ok("Đã xóa vật phẩm khỏi giỏ hàng thành công");

                return BadRequest("Lỗi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GetCustomerId()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                var token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                if (JwtUserToken.DecodeAPITokenToRole(token).Equals(Commons.CUSTOMER_NAME))
                {
                    return JwtUserToken.GetUserID(token);
                }
                else
                {
                    throw new ArgumentException("Đây không phải là khách hàng, không được phép thêm giỏ hàng");
                }
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
