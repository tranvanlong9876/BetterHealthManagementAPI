using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.CartService
{
    public class CartService : ICartService
    {
        private static FirestoreDb firestore = FirestoreDb.Create("better-health-3e75a");
        private readonly IProductDetailRepo _productDetailRepo;

        public CartService(IProductDetailRepo productDetailRepo)
        {
            _productDetailRepo = productDetailRepo;
        }

        public async Task<bool> UpdateCart(Cart cart)
        {
            var product = await _productDetailRepo.Get(cart.Item.ProductId);
            if (product == null) throw new ArgumentException("Không tìm thấy sản phẩm trong hệ thống");
            if (!product.IsSell) throw new ArgumentException("Sản phẩm này không được bày bán");

            cart.LastUpdated = Timestamp.GetCurrentTimestamp();
            var collectionReference = firestore.Collection("carts");
            var documentReference = collectionReference.Document(cart.customerIpAddress);
            var documentSnapshot = await documentReference.GetSnapshotAsync();
            if (!documentSnapshot.Exists)
            {
                cart.Items = new List<AddToCart>();
                cart.Items.Add(cart.Item);
                await documentReference.SetAsync(cart);
            }
            else
            {
                Cart existingCart = documentSnapshot.ConvertTo<Cart>();
                AddToCart cartItems = existingCart.Items.FirstOrDefault(x => x.ProductId == cart.Item.ProductId);
                bool isUpdated = false;
                if(cartItems != null)
                {
                    
                    if ((cartItems.Quantity <= Math.Abs(cart.Item.Quantity)) && cart.Item.Quantity < 0) throw new ArgumentException("Không thể trừ thêm số lượng nữa");
                    cartItems.Quantity = cart.Item.Quantity;
                    isUpdated = true;
                }
                else
                {
                    if (cart.Item.Quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
                    existingCart.Items.Add(cart.Item);
                    isUpdated = true;
                }
                if (isUpdated)
                {
                    await documentReference.UpdateAsync("items", existingCart.Items);
                    await documentReference.UpdateAsync("last-update", Timestamp.GetCurrentTimestamp());
                }
            }

            return true;
        }

        public async Task<ViewCart> GetCart(string phoneNo)
        {
            var collectionReference = firestore.Collection("carts");
            var documentReference = collectionReference.Document(phoneNo);
            var snapshot = await documentReference.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<ViewCart>();
            } else
            {
                return null;
            }
        }

        public async Task<bool> RemoveItemFromCart(string productId, string IpAddressOrPhoneNo)
        {
            var collectionReference = firestore.Collection("carts");
            var documentReference = collectionReference.Document(IpAddressOrPhoneNo);
            var documentSnapshot = await documentReference.GetSnapshotAsync();
            if (!documentSnapshot.Exists) throw new ArgumentException("Giỏ hàng không tìm thấy hoặc đã quá session!");

            var existingCart = documentSnapshot.ConvertTo<ViewCart>();
            var item = existingCart.Items.FirstOrDefault(x => x.ProductId.Equals(productId));

            if(item != null)
            {
                existingCart.Items.Remove(item);
                await documentReference.UpdateAsync("items", existingCart.Items);
                await documentReference.UpdateAsync("last-update", Timestamp.GetCurrentTimestamp());
            }

            return true;

        }
    }
}
