using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerPointServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductParentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IProductParentRepo _productParentRepo;
        private readonly IProductService _productService;
        private readonly IProductEventDiscountRepo _productEventDiscountRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly ICustomerPointService _customerPointService;


        public CartService(IProductDetailRepo productDetailRepo, IProductParentRepo productParentRepo, IProductEventDiscountRepo productEventDiscountRepo, IProductService productService, ICustomerRepo customerRepo, ICustomerPointService customerPointService)
        {
            _productDetailRepo = productDetailRepo;
            _productParentRepo = productParentRepo;
            _productEventDiscountRepo = productEventDiscountRepo;
            _productService = productService;
            _customerRepo = customerRepo;
            _customerPointService = customerPointService;
        }

        public async Task<bool> UpdateCart(Cart cart, string CustomerId)
        {
            var product = await _productDetailRepo.Get(cart.Item.ProductId);
            if (product == null) throw new ArgumentException("Không tìm thấy sản phẩm trong hệ thống");
            if (!product.IsSell) throw new ArgumentException("Sản phẩm này không được bày bán");
            var productParent = await _productParentRepo.Get(product.ProductIdParent);
            if (productParent.IsPrescription) throw new ArgumentException("Khách hàng không được phép tự ý đặt mua thuốc kê đơn.");
            cart.Point = 0;
            cart.LastUpdated = Timestamp.GetCurrentTimestamp();
            var collectionReference = firestore.Collection("carts");

            var cartId = await GetCartID(CustomerId, cart.deviceId, collectionReference);
            var documentReference = collectionReference.Document(cartId);
            var documentSnapshot = await documentReference.GetSnapshotAsync();
            if (!documentSnapshot.Exists)
            {
                cart.Items = new List<AddToCart>();
                cart.Items.Add(cart.Item);
                cart.DeviceIds = new List<string>();
                cart.DeviceIds.Add(cart.deviceId);
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    cart.customerId = CustomerId;
                }
                await documentReference.SetAsync(cart);
            }
            else
            {
                Cart existingCart = documentSnapshot.ConvertTo<Cart>();
                if (existingCart.Items == null)
                {
                    existingCart.Items = new List<AddToCart>();
                }
                AddToCart cartItems = existingCart.Items.FirstOrDefault(x => x.ProductId == cart.Item.ProductId);
                bool isUpdated = false;
                if (cartItems != null)
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

        private async Task<string> GetCartID(string CustomerId, string DeviceId, CollectionReference collectionReference)
        {
            var existCustomerCart = false;
            var existDeviceCart = false;
            var cartCustomerId = String.Empty;
            var cartDeviceId = String.Empty;
            if (!string.IsNullOrEmpty(CustomerId))
            {
                Query query = collectionReference.WhereEqualTo("customerid", CustomerId);
                QuerySnapshot documentSnapshots = await query.GetSnapshotAsync();
                if (documentSnapshots.Documents.Count > 0)
                {
                    existCustomerCart = true;
                    foreach (var documentFound in documentSnapshots.Documents)
                    {
                        cartCustomerId = documentFound.Id;
                    }
                }
            }

            //Tiếp tục tìm cart theo DeviceID
            Query queryDeviceId = collectionReference.WhereArrayContains("deviceids", DeviceId);
            QuerySnapshot snapshotsDeviceId = await queryDeviceId.GetSnapshotAsync();
            if (snapshotsDeviceId.Documents.Count > 0)
            {

                existDeviceCart = true;
                foreach (var documentFound in snapshotsDeviceId.Documents)
                {
                    cartDeviceId = documentFound.Id;
                }
            }

            if (existCustomerCart && existDeviceCart)
            {
                //Merge Cart
                //Ưu tiên cart Last Update gần hơn
                return await MergeCart(cartCustomerId, cartDeviceId, collectionReference);
            }
            if (existCustomerCart)
            {
                var documentCustomer = collectionReference.Document(cartCustomerId);
                var snapShotCustomer = await documentCustomer.GetSnapshotAsync();
                var CustomerCart = snapShotCustomer.ConvertTo<ViewCart>();
                CustomerCart.DeviceIds.Add(DeviceId);
                await documentCustomer.UpdateAsync("deviceids", CustomerCart.DeviceIds);
                return cartCustomerId;
            }
            if (existDeviceCart)
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    var documentDevice = collectionReference.Document(cartDeviceId);
                    await documentDevice.UpdateAsync("customerid", CustomerId);
                }
                return cartDeviceId;
            }

            //Không exist cả 2, tạo cart mới
            return Guid.NewGuid().ToString();
        }

        private async Task<string> MergeCart(string CustomerCartId, string DeviceCartId, CollectionReference collectionReference)
        {
            var documentReferenceCustomer = collectionReference.Document(CustomerCartId);
            var snapshotCustomer = await documentReferenceCustomer.GetSnapshotAsync();

            var documentReferenceDevice = collectionReference.Document(DeviceCartId);
            var snapshotDevice = await documentReferenceDevice.GetSnapshotAsync();
            ViewCart CustomerCart = null;
            ViewCart DeviceCart = null;
            if (snapshotCustomer.Exists)
            {
                CustomerCart = snapshotCustomer.ConvertTo<ViewCart>();
            }
            if (snapshotDevice.Exists)
            {
                DeviceCart = snapshotDevice.ConvertTo<ViewCart>();
            }
            if (!CustomerCartId.Equals(DeviceCartId))
            {
                for (int i = 0; i < CustomerCart.DeviceIds.Count; i++)
                {
                    if (!DeviceCart.DeviceIds.Contains(CustomerCart.DeviceIds[i]))
                    {
                        DeviceCart.DeviceIds.Add(CustomerCart.DeviceIds[i]);
                    }
                }
                for (int i = 0; i < CustomerCart.Items.Count; i++)
                {
                    if(DeviceCart.Items == null)
                    {
                        DeviceCart.Items = new List<CartItem>();
                    }
                    if (DeviceCart.Items.Where(x => x.ProductId.Equals(CustomerCart.Items[i].ProductId)).Count() == 0)
                    {
                        DeviceCart.Items.Add(CustomerCart.Items[i]);
                    }
                }

                await documentReferenceDevice.UpdateAsync("customerid", CustomerCart.customerId);
                await documentReferenceDevice.UpdateAsync("deviceids", DeviceCart.DeviceIds);
                await documentReferenceDevice.UpdateAsync("items", DeviceCart.Items);
                await documentReferenceDevice.UpdateAsync("last-update", Timestamp.GetCurrentTimestamp());

                if (!string.IsNullOrEmpty(CustomerCart.customerId) && string.IsNullOrEmpty(DeviceCart.customerId))
                {
                    await documentReferenceCustomer.DeleteAsync();
                }
                else if (!string.IsNullOrEmpty(CustomerCart.customerId) && !string.IsNullOrEmpty(DeviceCart.customerId))
                {
                    if (CustomerCart.customerId.Equals(DeviceCart.customerId))
                    {
                        await documentReferenceCustomer.DeleteAsync();
                    }
                }
            }

            return DeviceCartId;
        }

        public async Task<ViewCart> GetCart(string deviceId, string CustomerId)
        {
            var collectionReference = firestore.Collection("carts");
            var cartId = await GetCartID(CustomerId, deviceId, collectionReference);
            var documentReference = collectionReference.Document(cartId);
            var snapshot = await documentReference.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                var cart = snapshot.ConvertTo<ViewCart>();
                cart.CartId = cartId;
                return cart;
            }
            else
            {
                Cart cart = new Cart();
                cart.Point = 0;
                cart.LastUpdated = Timestamp.GetCurrentTimestamp();
                cart.DeviceIds = new List<string>();
                cart.DeviceIds.Add(deviceId);
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    cart.customerId = CustomerId;
                }
                await documentReference.SetAsync(cart);
                return await GetCart(deviceId, CustomerId);
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

            if (item != null)
            {
                existingCart.Items.Remove(item);
                await documentReference.UpdateAsync("items", existingCart.Items);
                await documentReference.UpdateAsync("last-update", Timestamp.GetCurrentTimestamp());
            }

            return true;

        }

        public async Task<IActionResult> UpdateCustomerCartPoint(CustomerCartPoint cartPoint)
        {
            var collectionReference = firestore.Collection("carts");
            var documentReference = collectionReference.Document(cartPoint.CartId);
            var snapShot = await documentReference.GetSnapshotAsync();

            if (snapShot.Exists)
            {
                var viewCart = snapShot.ConvertTo<ViewCart>();
                viewCart = await AddFullInformationToCart(viewCart);

                var customerDB = await _customerRepo.Get(viewCart.customerId);

                if (customerDB == null) return new NotFoundObjectResult("Giỏ hàng này không có khách hàng đại diện, không thể dùng điểm!");

                int maxPointCanUse = 0;

                var objectResult = await _customerPointService.GetCustomerAvailablePoint(customerDB.PhoneNo);

                if(objectResult is OkObjectResult customerPoint)
                {
                    maxPointCanUse = Convert.ToInt32(customerPoint.Value);
                }

                if (maxPointCanUse < cartPoint.UsingPoint) return new BadRequestObjectResult($"Không thể áp dụng điểm do số điểm dùng ({cartPoint.UsingPoint}) lớn hơn số điểm khách hàng đang có ({maxPointCanUse}).");

                maxPointCanUse = (int) (viewCart.SubTotalPrice / 1000);

                if (maxPointCanUse < cartPoint.UsingPoint) return new BadRequestObjectResult(new { message = $"Không thể sử dụng điểm vào giỏ hàng, số điểm tối đa có thể sử dụng là {maxPointCanUse}",
                                                                                                   maxPoint = maxPointCanUse});

                var existingCart = snapShot.ConvertTo<CustomerCartPoint>();
                existingCart.UsingPoint = cartPoint.UsingPoint;
                await documentReference.UpdateAsync("point", existingCart.UsingPoint);
                await documentReference.UpdateAsync("last-update", Timestamp.GetCurrentTimestamp());
                return new OkObjectResult("Apply điểm vào giỏ hàng thành công!");
            }
            return new NotFoundObjectResult("Không tìm thấy giỏ hàng của khách hàng");
        }

        private async Task<ViewCart> AddFullInformationToCart(ViewCart listCart)
        {
            var SubTotalPrice = 0D;
            if (listCart.Items != null)
            {
                for (int i = 0; i < listCart.Items.Count; i++)
                {
                    var cartInformation = await _productService.AddMoreProductInformationToCart(listCart.Items[i].ProductId);
                    listCart.Items[i].Price = cartInformation.Price;
                    listCart.Items[i].ProductImageUrl = cartInformation.ProductImageUrl;
                    listCart.Items[i].ProductName = cartInformation.ProductName;
                    listCart.Items[i].UnitId = cartInformation.UnitId;
                    listCart.Items[i].UnitName = cartInformation.UnitName;
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

            return listCart;
        }

        public async Task<IActionResult> RemoveCart(string cartId)
        {
            var document = firestore.Collection("carts").Document(cartId);

            var snapShot = await document.GetSnapshotAsync();
            if (snapShot.Exists)
            {
                await document.DeleteAsync();
            }

            return new OkObjectResult("Giỏ hàng đã được xóa thành công");
        }

        public async Task<IActionResult> AddToCartPharmacist(AddToCartPharmacistEntrance cartEntrance)
        {
            if (cartEntrance.items.Where(x => x.quantity <= 0).Any()) return new BadRequestObjectResult("Số lượng sản phẩm trong giỏ hàng phải lớn hơn 0");

            for (int i = 0; i < cartEntrance.items.Count; i++)
            {
                var item = cartEntrance.items[i];
                var product = await _productDetailRepo.Get(item.productId);
                if (product == null) return new BadRequestObjectResult(new { message = $"Không tìm thấy sản phẩm {item.productId} trong hệ thống", productId = item.productId });
                if (!product.IsSell) return new BadRequestObjectResult(new { message = $"Sản phẩm {item.productId} không được cho phép bày bán", productId = item.productId });
            }
            var document = firestore.Collection("carts").Document(cartEntrance.CartId);

            var snapShot = await document.GetSnapshotAsync();

            if (!snapShot.Exists) return new BadRequestObjectResult("Không tìm thấy cart khách hàng");

            Cart existingCart = snapShot.ConvertTo<Cart>();
            if (existingCart.Items == null)
            {
                existingCart.Items = new List<AddToCart>();
            }

            for (int i = 0; i < cartEntrance.items.Count; i++)
            {
                var item = cartEntrance.items[i];
                AddToCart cartItems = existingCart.Items.FirstOrDefault(x => x.ProductId == item.productId);
                bool isUpdated = false;
                if (cartItems != null)
                {

                    if ((cartItems.Quantity <= Math.Abs(item.quantity)) && item.quantity < 0) return new BadRequestObjectResult("Không thể trừ thêm số lượng nữa");
                    cartItems.Quantity = item.quantity;
                    isUpdated = true;
                }
                else
                {
                    if (item.quantity <= 0) return new BadRequestObjectResult("Số lượng phải lớn hơn 0");
                    var addCart = new AddToCart()
                    {
                        ProductId = item.productId,
                        Quantity = item.quantity
                    };
                    existingCart.Items.Add(addCart);
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    await document.UpdateAsync("items", existingCart.Items);
                    await document.UpdateAsync("last-update", Timestamp.GetCurrentTimestamp());
                }
            }

            return new OkObjectResult("Toàn bộ sản phẩm đã được thêm vào giỏ hàng khách hàng thành công!");
        }

        public async Task RunRemoveCartHourly()
        {
            try
            {
                var currentTime7DaysAgo = DateTime.SpecifyKind(CustomDateTime.Now.AddDays(-7), DateTimeKind.Utc);
                var dateTime7DaysAgo = Timestamp.FromDateTime(currentTime7DaysAgo);
                var collectionReference = firestore.Collection("carts");

                var query = collectionReference.WhereLessThan("last-update", dateTime7DaysAgo);

                var querySnapshot = await query.GetSnapshotAsync();

                if (querySnapshot.Count > 0)
                {
                    foreach (var documentSnapshot in querySnapshot.Documents)
                    {
                        await collectionReference.Document(documentSnapshot.Id).DeleteAsync();
                    }
                }
                await Task.CompletedTask;
            }
            catch
            {

            }
        }
    }
}
