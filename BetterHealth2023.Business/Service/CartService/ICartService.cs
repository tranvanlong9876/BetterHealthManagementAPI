using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CartService
{
    public interface ICartService
    {
        public Task<bool> UpdateCustomerCartPoint(CustomerCartPoint cartPoint);
        public Task<ViewCart> GetCart(string deviceId, string CustomerId);
        public Task<bool> UpdateCart(Cart cart, string CustomerId);

        public Task<IActionResult> AddToCartPharmacist(AddToCartPharmacistEntrance cartEntrance);
        public Task<bool> RemoveItemFromCart(string productId, string IpAddressOrPhoneNo);

        public Task<IActionResult> RemoveCart(string cartId);
    }
}
