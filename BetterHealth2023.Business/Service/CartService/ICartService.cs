using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CartService
{
    public interface ICartService
    {
        public Task<ViewCart> GetCart(string cartId);
        public Task<bool> UpdateCart(Cart cart);
        public Task<bool> RemoveItemFromCart(string productId, string IpAddressOrPhoneNo);
    }
}
