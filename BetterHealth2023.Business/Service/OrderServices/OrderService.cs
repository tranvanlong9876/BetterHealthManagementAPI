using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderBatchRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderContactInfoRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderPickupRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderShipmentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportBatchRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly ISiteInventoryRepo _siteInventoryRepo;
        private readonly ISiteRepo _siteRepo;
        private readonly ICustomerPointRepo _customerPointRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;
        private readonly IOrderHeaderRepo _orderHeaderRepo;
        private readonly IOrderShipmentRepo _orderShipmentRepo;
        private readonly IOrderPickUpRepo _orderPickUpRepo;
        private readonly IProductImportRepo _productImportRepo;
        private readonly IOrderBatchRepo _orderBatchRepo;
        private readonly IOrderDetailRepo _orderDetailRepo;
        private readonly IOrderContactInfoRepo _orderContactInfoRepo;
        private readonly IProductDetailRepo _productDetailRepo;

        public OrderService(ISiteInventoryRepo siteInventoryRepo, ISiteRepo siteRepo, ICustomerPointRepo customerPointRepo, ICustomerRepo customerRepo, IDynamicAddressRepo dynamicAddressRepo, IOrderHeaderRepo orderHeaderRepo, IOrderShipmentRepo orderShipmentRepo, IOrderPickUpRepo orderPickUpRepo, IProductImportRepo productImportRepo, IOrderBatchRepo orderBatchRepo, IOrderDetailRepo orderDetailRepo, IOrderContactInfoRepo orderContactInfoRepo, IProductDetailRepo productDetailRepo)
        {
            _siteInventoryRepo = siteInventoryRepo;
            _siteRepo = siteRepo;
            _customerPointRepo = customerPointRepo;
            _customerRepo = customerRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
            _orderHeaderRepo = orderHeaderRepo;
            _orderShipmentRepo = orderShipmentRepo;
            _orderPickUpRepo = orderPickUpRepo;
            _productImportRepo = productImportRepo;
            _orderBatchRepo = orderBatchRepo;
            _orderDetailRepo = orderDetailRepo;
            _orderContactInfoRepo = orderContactInfoRepo;
            _productDetailRepo = productDetailRepo;
        }

        public async Task<CreateOrderCheckOutStatus> CheckOutOrder(CheckOutOrderModel checkOutOrderModel, string CustomerId)
        {
            var checkError = new CreateOrderCheckOutStatus();

            //Validate
            if(checkOutOrderModel.OrderTypeId == Commons.ORDER_TYPE_DIRECTLY && (String.IsNullOrEmpty(checkOutOrderModel.PharmacistId) || (String.IsNullOrEmpty(checkOutOrderModel.SiteId))))
            {
                checkError.isError = true;
                checkError.missingPharmacist = "Mua hàng tại chỗ phải có Chi Nhánh và Nhân Viên Dược Sĩ đại diện.";
                checkError.missingSite = "Mua hàng tại chỗ phải có Chi Nhánh và Nhân Viên Dược Sĩ đại diện.";
                return checkError;
            }

            if (checkOutOrderModel.OrderTypeId != Commons.ORDER_TYPE_DELIVERY && String.IsNullOrEmpty(checkOutOrderModel.SiteId))
            {
                checkError.isError = true;
                checkError.missingPharmacist = "Mua hàng tại chỗ và đến lấy phải có Chi Nhánh đại diện";
                return checkError;
            }

            if(checkOutOrderModel.OrderTypeId == Commons.ORDER_TYPE_PICKUP && checkOutOrderModel.OrderPickUp == null)
            {
                checkError.isError = true;
                checkError.missingPharmacist = "Thiếu Model của đơn hàng đến lấy, vui lòng bổ sung dữ liệu.";
                return checkError;
            }
            if(checkOutOrderModel.OrderTypeId == Commons.ORDER_TYPE_DELIVERY)
            {
                if(checkOutOrderModel.ReveicerInformation.CityId == null || checkOutOrderModel.ReveicerInformation.DistrictId == null || checkOutOrderModel.ReveicerInformation.WardId == null || checkOutOrderModel.ReveicerInformation.HomeAddress == null)
                {
                    checkError.isError = true;
                    checkError.missingAddress = "Thiếu dữ liệu địa chỉ của đơn giao hàng, vui lòng bổ sung dữ liệu.";
                    return checkError;
                }
            }

            if(checkOutOrderModel.Vouchers != null)
            {
                if(checkOutOrderModel.Vouchers.Count > 0)
                {
                    //Validate Voucher khi có sử dụng Voucher.
                }
            }

            //Trừ điểm tích lũy
            var isUseSuccessfully = false;
            
            if (checkOutOrderModel.UsedPoint > 0)
            {
                var checkType = 0;
                int? customerPoint = null;
                if (!String.IsNullOrEmpty(CustomerId))
                {
                    customerPoint = await _customerPointRepo.GetCustomerPointBasedOnCustomerId(CustomerId);
                    checkType = 1;
                } else if(!String.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.PhoneNumber))
                {
                    customerPoint = await _customerPointRepo.GetCustomerPointBasedOnPhoneNumber(checkOutOrderModel.ReveicerInformation.PhoneNumber);
                    checkType = 2;
                } else
                {
                    customerPoint = null;
                }

                if (customerPoint.HasValue)
                {
                    if (customerPoint >= checkOutOrderModel.UsedPoint)
                    {
                        var customerPointModel = new CustomerPoint()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IsPlus = false,
                            CreateDate = DateTime.Now,
                            CustomerId = checkType == 1 ? CustomerId : await _customerRepo.GetCustomerIdBasedOnPhoneNo(checkOutOrderModel.ReveicerInformation.PhoneNumber),
                            Point = checkOutOrderModel.UsedPoint,
                            Description = $"Sử dụng {checkOutOrderModel.UsedPoint} điểm cho đơn hàng {checkOutOrderModel.OrderId} vào lúc {DateTime.Now}"
                        };
                        await _customerPointRepo.Insert(customerPointModel);
                        isUseSuccessfully = true;
                    }
                }
            }
            //OrderType = 1 : Bán tại chỗ, 2: Nhận tại cửa hàng, 3: Giao hàng
            var orderHeaderDB = new OrderHeader()
            {
                Id = checkOutOrderModel.OrderId,
                OrderTypeId = checkOutOrderModel.OrderTypeId,
                SubTotalPrice = checkOutOrderModel.SubTotalPrice,
                DiscountPrice = checkOutOrderModel.DiscountPrice,
                TotalPrice = checkOutOrderModel.TotalPrice,
                UsedPoint = isUseSuccessfully ? checkOutOrderModel.UsedPoint : 0,
                PayType = checkOutOrderModel.PayType,
                Note = checkOutOrderModel.Note,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatusIdCheckOut(checkOutOrderModel.OrderTypeId),
                PharmacistId = checkOutOrderModel.OrderTypeId == 1 ? checkOutOrderModel.PharmacistId : null,
                SiteId = checkOutOrderModel.OrderTypeId == 3 ? null : checkOutOrderModel.SiteId,
                IsPaid = checkOutOrderModel.isPaid,
            };

            await _orderHeaderRepo.Insert(orderHeaderDB);

            string addressId = null;

            if (checkOutOrderModel.ReveicerInformation.CityId != null && checkOutOrderModel.ReveicerInformation.DistrictId != null && checkOutOrderModel.ReveicerInformation.WardId != null && checkOutOrderModel.ReveicerInformation.HomeAddress != null)
            {
                addressId = Guid.NewGuid().ToString();
                var dynamicAddressDB = new DynamicAddress()
                {
                    Id = addressId,
                    CityId = checkOutOrderModel.ReveicerInformation.CityId,
                    DistrictId = checkOutOrderModel.ReveicerInformation.DistrictId,
                    WardId = checkOutOrderModel.ReveicerInformation.WardId,
                    HomeAddress = checkOutOrderModel.ReveicerInformation.HomeAddress
                };
                await _dynamicAddressRepo.Insert(dynamicAddressDB);
            }

            //Thêm thông tin khách hàng của đơn hàng.
            var orderContactInfo = new OrderContactInfo()
            {
                Id = Guid.NewGuid().ToString(),
                AddressId = addressId,
                CustomerId = CustomerId,
                Email = checkOutOrderModel.ReveicerInformation.Email,
                Fullname = checkOutOrderModel.ReveicerInformation.Fullname,
                Gender = checkOutOrderModel.ReveicerInformation.Gender,
                OrderId = checkOutOrderModel.OrderId,
                PhoneNo = checkOutOrderModel.ReveicerInformation.PhoneNumber
            };
            await _orderContactInfoRepo.Insert(orderContactInfo);

            if (orderHeaderDB.OrderTypeId == Commons.ORDER_TYPE_DELIVERY)
            {
                //thêm địa chỉ giao hàng đối với đơn giao hàng.
                //thêm thông tin giao hàng đối với đơn giao hàng.
                var orderShipmentDB = new OrderShipment()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = orderHeaderDB.Id,
                    //StartAddressId: Khi check out xong chưa có.
                    DestinationAddressId = addressId,
                    ShippingFee = checkOutOrderModel.ShippingPrice,
                };

                await _orderShipmentRepo.Insert(orderShipmentDB);
                
            }

            if(orderHeaderDB.OrderTypeId == Commons.ORDER_TYPE_PICKUP)
            {
                var orderPickUpDB = new Repository.DatabaseModels.OrderPickUp()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = orderHeaderDB.Id,
                    DatePickUp = checkOutOrderModel.OrderPickUp.DatePickUp,
                    TimePickUp = checkOutOrderModel.OrderPickUp.TimePickUp
                };

                await _orderPickUpRepo.Insert(orderPickUpDB);
                
            }
            //Insert Xong Header.
            List<SendingEmailProductModel> productSendingEmailModels = new List<SendingEmailProductModel>();
            for (int i = 0; i < checkOutOrderModel.Products.Count; i++)
            {
                var productModel = checkOutOrderModel.Products[i];

                //Thêm lô hàng và trừ tồn kho đối với đơn hàng không phải đơn giao hàng.
                if (!checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
                {
                    var isBatches = await _productImportRepo.checkProductManageByBatches(productModel.ProductId);

                    //Convert thành unit cuối cùng
                    var productParentId = await _productDetailRepo.GetProductParentID(productModel.ProductId);
                    var productDetailDB = await _productDetailRepo.Get(productModel.ProductId);
                    var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);
                    var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();
                    int currentQuantity = productModel.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList);
                    //Có quản lý theo lô
                    if (isBatches)
                    {

                        var listOfProductBatches = new List<OrderBatch>();
                        var availableBatches = await _siteInventoryRepo.GetAllProductBatchesAvailable(productLastUnitDetail.Id, checkOutOrderModel.SiteId);

                        int loopBatch = 0;
                        //Quantity sau khi convert thành đơn vị cuối
                        while (currentQuantity > 0)
                        {
                            if (availableBatches[loopBatch].Quantity > currentQuantity)
                            {
                                listOfProductBatches.Add(new OrderBatch()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SiteInventoryBatchId = availableBatches[loopBatch].Id,
                                    OrderId = checkOutOrderModel.OrderId,
                                    SoldQuantity = currentQuantity
                                });
                                //Đã trừ
                                var thisBatch = await _siteInventoryRepo.Get(availableBatches[loopBatch].Id);
                                thisBatch.Quantity = thisBatch.Quantity - currentQuantity;
                                await _siteInventoryRepo.Update();
                                currentQuantity = 0;

                            }
                            else
                            {
                                listOfProductBatches.Add(new OrderBatch()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SiteInventoryBatchId = availableBatches[loopBatch].Id,
                                    OrderId = checkOutOrderModel.OrderId,
                                    SoldQuantity = availableBatches[loopBatch].Quantity
                                });
                                
                                currentQuantity = currentQuantity - availableBatches[loopBatch].Quantity;
                                var batchDB = await _siteInventoryRepo.Get(availableBatches[loopBatch].Id);
                                batchDB.Quantity = 0;
                                await _siteInventoryRepo.Update();
                                loopBatch++;
                            }
                        }

                        await _orderBatchRepo.InsertRange(listOfProductBatches);
                    } else //không quản lý theo lô
                    {
                        var siteInventory = await _siteInventoryRepo.GetSiteInventory(checkOutOrderModel.SiteId, productLastUnitDetail.Id);
                        siteInventory.Quantity = siteInventory.Quantity - currentQuantity;
                        await _siteInventoryRepo.Update();
                    }
                }
                //Đưa hàng cần mua xuống Database
                //Also Generate List Of Product with A Lot of detail.

                var orderProductModelDB = new OrderDetail()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = checkOutOrderModel.OrderId,
                    ProductId = productModel.ProductId,
                    OriginalPrice = productModel.OriginalPrice,
                    DiscountPrice = productModel.DiscountPrice,
                    Quantity = productModel.Quantity,
                    PriceTotal = productModel.DiscountPrice * productModel.Quantity,
                };

                if(checkOutOrderModel.OrderTypeId != Commons.ORDER_TYPE_DIRECTLY)
                {
                    var productSendingEmailModel = _customerPointRepo.TransferBetweenTwoModels<OrderDetail, SendingEmailProductModel>(orderProductModelDB);
                    productSendingEmailModel.TotalPrice = orderProductModelDB.PriceTotal;
                    productSendingEmailModel.imageUrl = productModel.ProductImageUrl;
                    productSendingEmailModel.ProductName = productModel.ProductName;
                    productSendingEmailModels.Add(productSendingEmailModel);
                }
                await _orderDetailRepo.Insert(orderProductModelDB);
            }

            //Khi hoàn thành đơn mới cộng điểm tích lũy, checkout xong không cộng.

            //Generate List Of Product With More Information
            if(checkOutOrderModel.OrderTypeId != Commons.ORDER_TYPE_DIRECTLY && !String.IsNullOrWhiteSpace(checkOutOrderModel.ReveicerInformation.Email))
            {
                if (checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP))
                {
                    //Get Site Address
                    var SiteDB = await _siteRepo.Get(checkOutOrderModel.SiteId);
                    string address = await _dynamicAddressRepo.GetFullAddressFromAddressId(SiteDB.AddressId);
                    _ = Task.Run(() => EmailService.SendCustomerInvoiceEmail(productSendingEmailModels, checkOutOrderModel, address)).ConfigureAwait(false);
                }
                if (checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
                {
                    //Get Customer Address
                    string address = await _dynamicAddressRepo.GetFullAddressFromAddressId(addressId);
                    _ = Task.Run(() => EmailService.SendCustomerInvoiceEmail(productSendingEmailModels, checkOutOrderModel, address)).ConfigureAwait(false);
                }
            }

            //Gửi mail hóa đơn qua email khách

            checkError.isError = false;
            return checkError;
            
        }

        private int CountTotalQuantityFromFirstToLastUnit(List<ProductUnitModel> productDetailList)
        {
            int totalQuantity = 1;

            if (productDetailList.Count <= 1) return totalQuantity;

            for (int i = 0; i < productDetailList.Count - 1; i++)
            {
                totalQuantity = totalQuantity * productDetailList.Find(x => x.UnitLevel == (i + 2)).Quantitative;
            }

            return totalQuantity;
        }

        private string OrderStatusIdCheckOut(int orderTypeId)
        {
            switch (orderTypeId)
            {
                case 1:
                    return Commons.CHECKOUT_ORDER_DIRECTLY_ID;
                case 2:
                    return Commons.CHECKOUT_ORDER_PICKUP_ID;
                case 3:
                    return Commons.CHECKOUT_ORDER_DELIVERY_ID;
                default:
                    return null;
            }
                 
        }

        public async Task<ViewSiteToPickUpStatus> GetViewSiteToPickUps(CartEntrance cartEntrance)
        {
            var checkError = new ViewSiteToPickUpStatus();
            List<CartModel> cartModels;
            try
            {
                var productIdList = cartEntrance.ProductId.Split(";").ToList();
                var productQuantityList = cartEntrance.Quantity.Split(";").ToList().Select(x => int.Parse(x)).ToList();
                if (productIdList.Count != productQuantityList.Count)
                {
                    checkError.isError = true;
                    checkError.errorConvert = "Số lượng phần tử không bằng nhau, vui lòng kiểm tra lại dữ liệu đầu vào.";
                    return checkError;
                }
                cartModels = new List<CartModel>();

                
                for (int i = 0; i < productIdList.Count; i++)
                {
                    var productParentId = await _productDetailRepo.GetProductParentID(productIdList[i]);

                    var productDetailDB = await _productDetailRepo.Get(productIdList[i]);
                    //Get All Unit 
                    var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);
                    var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();
                    //Convert thành unit level cuối và quantity tổng của nó
                    cartModels.Add(new CartModel()
                    {
                        ProductId = productLastUnitDetail.Id,
                        Quantity = productQuantityList[i] * CountTotalQuantityFromFirstToLastUnit(productLaterList),
                        isBatches = await _productImportRepo.checkProductManageByBatches(productIdList[i])
                });
                }
            }
            catch
            {
                checkError.isError = true;
                checkError.errorConvert = "Lỗi chuyển đổi dữ liệu, hãy đảm bảo Quantity đã đúng kiểu Integer và ngăn cách bằng chấm phẩy (;)";
                return checkError;
            }

            SiteModelToPickUp siteModel = await _siteInventoryRepo.ViewSiteToPickUpsAsync(cartModels, cartEntrance.CityId, cartEntrance.DistrictId);

            checkError.siteListPickUp = siteModel;

            checkError.isError = false;
            return checkError;
        }

        public async Task<string> GenerateOrderId()
        {
            Random generator = new Random();
            
            var createdDate = String.Format("{0:ddMMyyyy-HHmmss}", DateTime.Now);
            bool isDuplicate = true;
            var orderId = String.Empty;
            while (isDuplicate)
            {
                var randomNumber = generator.Next(1, 1_000_000).ToString("D6");
                orderId = "BTH-" + randomNumber + "-" + createdDate;
                isDuplicate = await _orderHeaderRepo.CheckDuplicateOrderId(orderId);
            }
            return orderId;
        }
    }



    public class CartModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public bool isBatches { get; set; }
    }
}
