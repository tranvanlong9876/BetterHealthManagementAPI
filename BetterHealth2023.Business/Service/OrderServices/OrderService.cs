using BetterHealthManagementAPI.BetterHealth2023.Business.Service.VNPay;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderExecutionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderBatchRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderContactInfoRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderPickupRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderShipmentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderStatusRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderVNPayRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderValidateModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewOrderListModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.ViewSpecificOrderModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IOrderExecutionRepo _orderExecutionRepo;
        private readonly IOrderStatusRepo _orderStatusRepo;
        private readonly IOrderVNPayRepo _orderVNPayRepo;
        private readonly IVNPayService _vnPayService;

        public OrderService(ISiteInventoryRepo siteInventoryRepo, ISiteRepo siteRepo, ICustomerPointRepo customerPointRepo, ICustomerRepo customerRepo, IDynamicAddressRepo dynamicAddressRepo, IOrderHeaderRepo orderHeaderRepo, IOrderShipmentRepo orderShipmentRepo, IOrderPickUpRepo orderPickUpRepo, IProductImportRepo productImportRepo, IOrderBatchRepo orderBatchRepo, IOrderDetailRepo orderDetailRepo, IOrderContactInfoRepo orderContactInfoRepo, IProductDetailRepo productDetailRepo, IOrderExecutionRepo orderExecutionRepo, IOrderStatusRepo orderStatusRepo, IOrderVNPayRepo orderVNPayRepo, IVNPayService vnPayService)
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
            _orderExecutionRepo = orderExecutionRepo;
            _orderStatusRepo = orderStatusRepo;
            _orderVNPayRepo = orderVNPayRepo;
            _vnPayService = vnPayService;
        }

        public async Task<IActionResult> CheckOutOrder(CheckOutOrderModel checkOutOrderModel, string token)
        {
            var userRole = JwtUserToken.DecodeAPITokenToRole(token);
            var userId = JwtUserToken.GetUserID(token);
            var customerId = userRole.Equals(Commons.CUSTOMER_NAME) ? userId : null;
            var pharmacistId = userRole.Equals(Commons.PHARMACIST_NAME) ? userId : null;
            //Validate
            if (checkOutOrderModel.Products.Count == 0)
            {
                return new BadRequestObjectResult("Không thể checkout khi không có sản phẩm");
            }

            if (checkOutOrderModel.OrderTypeId == Commons.ORDER_TYPE_DIRECTLY && (String.IsNullOrEmpty(checkOutOrderModel.PharmacistId) || (String.IsNullOrEmpty(checkOutOrderModel.SiteId))))
            {
                return new BadRequestObjectResult("Đơn hàng tại chỗ phải có Nhân Viên và Chi nhánh đại diện");
            }

            if (checkOutOrderModel.OrderTypeId != Commons.ORDER_TYPE_DELIVERY && String.IsNullOrEmpty(checkOutOrderModel.SiteId))
            {
                return new BadRequestObjectResult("Mua hàng tại chỗ và đến lấy phải có Chi Nhánh đại diện");
            }

            if (checkOutOrderModel.OrderTypeId == Commons.ORDER_TYPE_PICKUP && checkOutOrderModel.OrderPickUp == null)
            {
                return new BadRequestObjectResult("Thiếu model của đơn hàng đến lấy, vui lòng bổ sung dữ liệu.");
            }

            if (checkOutOrderModel.OrderTypeId == Commons.ORDER_TYPE_DIRECTLY && !userRole.Equals(Commons.PHARMACIST_NAME))
            {
                return new BadRequestObjectResult("Không phải Pharmacist không thể tạo được đơn hàng bán tại chỗ");
            }

            if (checkOutOrderModel.OrderTypeId == Commons.ORDER_TYPE_DELIVERY)
            {
                if (string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.CityId) || string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.DistrictId) || string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.WardId) || string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.HomeAddress))
                {
                    return new BadRequestObjectResult("Thiếu dữ liệu địa chỉ của đơn giao hàng, vui lòng bổ sung dữ liệu.");
                }
            }

            if (checkOutOrderModel.PayType == 2 && checkOutOrderModel.VnpayInformation == null)
            {
                return new BadRequestObjectResult("Thanh toán VN Pay bắt buộc phải có dữ liệu trả về từ VN Pay, cần thêm model của VnpayInformation.");
            }

            if (checkOutOrderModel.Vouchers != null)
            {
                if (checkOutOrderModel.Vouchers.Count > 0)
                {
                    //Validate Voucher khi có sử dụng Voucher.
                }
            }

            //check tồn kho đối với đơn bán tại chỗ + bán đến lấy

            if (!checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
            {
                for (int i = 0; i < checkOutOrderModel.Products.Count; i++)
                {
                    List<OrderProductLastUnitLevel> orderProductLastUnitLevels = new List<OrderProductLastUnitLevel>();
                    var productsInOrder = checkOutOrderModel.Products[i];

                    var productParentId = await _productDetailRepo.GetProductParentID(productsInOrder.ProductId);
                    var productDetailDB = await _productDetailRepo.Get(productsInOrder.ProductId);
                    var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);
                    var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();
                    var manageByBatch = await _productImportRepo.checkProductManageByBatches(productLastUnitDetail.Id);

                    //int QuantityAfterConvert = CountTotalQuantityFromFirstToLastUnit(productLaterList);

                    orderProductLastUnitLevels.Add(new OrderProductLastUnitLevel()
                    {
                        productId = productLastUnitDetail.Id,
                        productQuantity = productsInOrder.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList),
                        UnitName = productLastUnitDetail.UnitName,
                        productQuantityButOnlyOne = CountTotalQuantityFromFirstToLastUnit(productLaterList),
                        isBatches = manageByBatch
                    });

                    var missingProduct = await _siteInventoryRepo.CheckMissingProductOfSiteId(checkOutOrderModel.SiteId, orderProductLastUnitLevels);

                    if (missingProduct.Count > 0)
                    {
                        return new BadRequestObjectResult(missingProduct);
                    }
                }
            }

            //Nếu Token không có CustomerId (Guest, kiểm tra thêm một lần nữa bằng cách lôi số điện thoại ra)
            //Line 107'
            if (string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.PhoneNumber))
            {
                //Get thêm lần nữa bằng SĐT
                customerId = await _customerRepo.GetCustomerIdBasedOnPhoneNo(checkOutOrderModel.ReveicerInformation.PhoneNumber);
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                userId = customerId;
            }

            //Trừ điểm tích lũy
            var isUseSuccessfully = false;


            if (checkOutOrderModel.UsedPoint > 0)
            {
                int customerPoint = 0;
                if (!String.IsNullOrEmpty(customerId))
                {
                    customerPoint = await _customerPointRepo.GetCustomerPointBasedOnCustomerId(customerId);
                }
                else
                {
                    customerPoint = 0;
                }

                if (customerPoint > 0)
                {
                    if (customerPoint >= checkOutOrderModel.UsedPoint)
                    {
                        var customerPointModel = new CustomerPoint()
                        {
                            Id = Guid.NewGuid().ToString(),
                            IsPlus = false,
                            CreateDate = CustomDateTime.Now,
                            CustomerId = customerId,
                            Point = checkOutOrderModel.UsedPoint,
                            Description = $"Sử dụng {checkOutOrderModel.UsedPoint} điểm cho đơn hàng {checkOutOrderModel.OrderId} vào lúc {Commons.ConvertToVietNamDatetime(CustomDateTime.Now)}"
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
                CreatedDate = CustomDateTime.Now,
                OrderStatus = OrderStatusIdCheckOut(checkOutOrderModel.OrderTypeId),
                PharmacistId = checkOutOrderModel.OrderTypeId == 1 ? checkOutOrderModel.PharmacistId : null,
                SiteId = checkOutOrderModel.OrderTypeId == 3 ? null : checkOutOrderModel.SiteId,
                IsPaid = checkOutOrderModel.isPaid,
                ApprovedDate = checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DIRECTLY) ? CustomDateTime.Now : null,
                IsApproved = checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DIRECTLY) ? true : null
            };

            await _orderHeaderRepo.Insert(orderHeaderDB);

            //Insert VN Pay

            if (checkOutOrderModel.PayType == 2)
            {
                var orderVNPay = new OrderVnpay()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = checkOutOrderModel.OrderId,
                    VnpPayDate = checkOutOrderModel.VnpayInformation.vnp_PayDate,
                    VnpTransactionNo = checkOutOrderModel.VnpayInformation.Vnp_TransactionNo
                };

                await _orderVNPayRepo.Insert(orderVNPay);
            }

            string addressId = null;

            if (!string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.CityId) && !string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.DistrictId) && !string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.WardId) && !string.IsNullOrEmpty(checkOutOrderModel.ReveicerInformation.HomeAddress))
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
                CustomerId = string.IsNullOrEmpty(customerId) ? null : customerId,
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
                    EstimateDeliveryTime = EmailService.GetEstimateDeliveryTime(CustomDateTime.Now)
                };

                await _orderShipmentRepo.Insert(orderShipmentDB);

            }

            if (orderHeaderDB.OrderTypeId == Commons.ORDER_TYPE_PICKUP)
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
            for(int i = 0; i < checkOutOrderModel.Products.Count; i++)
            {
                var productModel = checkOutOrderModel.Products[i];
                var productParentId = await _productDetailRepo.GetProductParentID(productModel.ProductId);
                var productDetailDB = await _productDetailRepo.Get(productModel.ProductId);
                var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);
                var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();
                checkOutOrderModel.Products[i].QuantityAfterConvert = CountTotalQuantityFromFirstToLastUnit(productLaterList);
                checkOutOrderModel.Products[i].ParentId = productParentId;
                checkOutOrderModel.Products[i].productDetail = productDetailDB;
                checkOutOrderModel.Products[i].listUnit = productLaterList;
                checkOutOrderModel.Products[i].lastUnit = productLastUnitDetail;
            }

            checkOutOrderModel.Products = checkOutOrderModel.Products.OrderByDescending(x => x.QuantityAfterConvert).ToList();

            for (int i = 0; i < checkOutOrderModel.Products.Count; i++)
            {
                var productModel = checkOutOrderModel.Products[i];

                //Thêm lô hàng và trừ tồn kho đối với đơn hàng không phải đơn giao hàng.
                if (!checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
                {
                    var isBatches = await _productImportRepo.checkProductManageByBatches(productModel.ProductId);

                    //Convert thành unit cuối cùng
                    var productParentId = checkOutOrderModel.Products[i].ParentId;
                    var productDetailDB = checkOutOrderModel.Products[i].productDetail;
                    var productLaterList = checkOutOrderModel.Products[i].listUnit;
                    var productLastUnitDetail = checkOutOrderModel.Products[i].lastUnit;
                    int currentQuantity = productModel.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList);

                    bool differentUnitAfterConvert = productLaterList.Count >= 2;
                    int QuantityAfterConvert = CountTotalQuantityFromFirstToLastUnit(productLaterList);

                    //Có quản lý theo lô
                    if (isBatches)
                    {
                        var listOfProductBatches = new List<OrderBatch>();
                        var availableBatches = await _siteInventoryRepo.GetAllProductBatchesAvailable(productLastUnitDetail.Id, checkOutOrderModel.SiteId);

                        int loopBatch = 0;
                        //Quantity sau khi convert thành đơn vị cuối
                        while (currentQuantity > 0)
                        {
                            if (differentUnitAfterConvert) //nếu không làm được, sửa thành False.
                            {
                                if ((availableBatches[loopBatch].Quantity / QuantityAfterConvert) < 1)
                                {
                                    //bỏ qua lô này
                                    loopBatch++;
                                    continue;
                                }
                                else
                                {
                                    int NumberEnoughForHighestUnit = availableBatches[loopBatch].Quantity / QuantityAfterConvert;
                                    int ConvertToLatestUnitQuantity = NumberEnoughForHighestUnit * QuantityAfterConvert;

                                    if (ConvertToLatestUnitQuantity >= currentQuantity) //chỉ cần sử dụng lô này
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
                                        thisBatch.UpdatedDate = CustomDateTime.Now;
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
                                            SoldQuantity = ConvertToLatestUnitQuantity
                                        });

                                        currentQuantity = currentQuantity - ConvertToLatestUnitQuantity;
                                        var batchDB = await _siteInventoryRepo.Get(availableBatches[loopBatch].Id);
                                        batchDB.Quantity -= ConvertToLatestUnitQuantity;
                                        batchDB.UpdatedDate = CustomDateTime.Now;
                                        await _siteInventoryRepo.Update();
                                        loopBatch++;
                                    }
                                }
                            } //here is same unit
                            else
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
                                    thisBatch.UpdatedDate = CustomDateTime.Now;
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
                                    batchDB.UpdatedDate = CustomDateTime.Now;
                                    await _siteInventoryRepo.Update();
                                    loopBatch++;
                                }
                            }
                        }

                        await _orderBatchRepo.InsertRange(listOfProductBatches);
                    }
                    else //không quản lý theo lô
                    {
                        var siteInventory = await _siteInventoryRepo.GetSiteInventory(checkOutOrderModel.SiteId, productLastUnitDetail.Id);
                        siteInventory.Quantity = siteInventory.Quantity - currentQuantity;
                        siteInventory.UpdatedDate = CustomDateTime.Now;
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
                    Quantity = productModel.Quantity
                };

                if (checkOutOrderModel.OrderTypeId != Commons.ORDER_TYPE_DIRECTLY)
                {
                    var productSendingEmailModel = _customerPointRepo.TransferBetweenTwoModels<OrderDetail, SendingEmailProductModel>(orderProductModelDB);
                    productSendingEmailModel.TotalPrice = orderProductModelDB.DiscountPrice * orderProductModelDB.Quantity;
                    var informationToSendEmail = await _productDetailRepo.GetImageAndProductName(orderProductModelDB.ProductId);
                    productSendingEmailModel.imageUrl = informationToSendEmail.ImageUrl;
                    productSendingEmailModel.ProductName = informationToSendEmail.Name;
                    productSendingEmailModel.UnitName = informationToSendEmail.UnitName;
                    productSendingEmailModels.Add(productSendingEmailModel);
                }
                await _orderDetailRepo.Insert(orderProductModelDB);
            }

            //Khi hoàn thành đơn mới cộng điểm tích lũy, checkout xong không cộng

            //Insert Execution
            var orderExecution = new OrderExecution()
            {
                Id = Guid.NewGuid().ToString(),
                DateOfCreate = CustomDateTime.Now,
                Description = checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DIRECTLY) ? "Đơn tại chỗ đã bán xong, cảm ơn bạn đã mua hàng!" : "Đơn hàng đã được đặt thành công và sẽ được nhân viên xử lý sớm.",
                IsInternalUser = checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DIRECTLY) ? true : false,
                OrderId = checkOutOrderModel.OrderId,
                StatusChangeFrom = OrderStatusIdCheckOut(checkOutOrderModel.OrderTypeId),
                StatusChangeTo = OrderStatusIdCheckOut(checkOutOrderModel.OrderTypeId),
                UserId = string.IsNullOrEmpty(userId) ? null : userId
            };

            await _orderExecutionRepo.Insert(orderExecution);

            //Generate List Of Product With More Information
            if (checkOutOrderModel.OrderTypeId != Commons.ORDER_TYPE_DIRECTLY && !string.IsNullOrWhiteSpace(checkOutOrderModel.ReveicerInformation.Email))
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

            //Cộng điểm đối với đơn hàng bán tại chỗ.
            if (!string.IsNullOrEmpty(customerId) && checkOutOrderModel.TotalPrice >= 15000 && checkOutOrderModel.OrderTypeId.Equals(Commons.ORDER_TYPE_DIRECTLY))
            {
                //cộng điểm
                var customerPoint = new CustomerPoint()
                {
                    CustomerId = customerId,
                    CreateDate = CustomDateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    IsPlus = true,
                    Point = (int)(checkOutOrderModel.TotalPrice / 15000) + (checkOutOrderModel.TotalPrice % 15000 == 0 ? 0 : 1),
                    Description = $"Điểm tích lũy từ đơn hàng #{checkOutOrderModel.OrderId}"
                };
                await _customerPointRepo.Insert(customerPoint);
            }

            return new OkObjectResult("Checkout đơn hàng thành công!");

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
                        isBatches = await _productImportRepo.checkProductManageByBatches(productIdList[i]),
                        QuantityConvert = CountTotalQuantityFromFirstToLastUnit(productLaterList)
                    });
                }
            }
            catch
            {
                checkError.isError = true;
                checkError.errorConvert = "Lỗi chuyển đổi dữ liệu, hãy đảm bảo Quantity đã đúng kiểu Integer và ngăn cách bằng chấm phẩy (;). Đồng thời, vui lòng truyền ProductId có thật.";
                return checkError;
            }

            SiteModelToPickUp siteModel = await _siteInventoryRepo.ViewSiteToPickUpsAsync(cartModels, cartEntrance.CityId, cartEntrance.DistrictId);

            checkError.siteListPickUp = siteModel;
            for (var i = 0; i < siteModel.siteListToPickUps.Count; i++)
            {
                siteModel.siteListToPickUps[i].FullyAddress = await _dynamicAddressRepo.GetFullAddressFromAddressId(siteModel.siteListToPickUps[i].AddressId);
            }

            checkError.isError = false;
            return checkError;
        }

        public async Task<string> GenerateOrderId()
        {
            Random generator = new Random();

            var createdDate = String.Format("{0:ddMMyyyy-HHmmss}", CustomDateTime.Now);
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

        public async Task<PagedResult<ViewOrderList>> GetAllOrders(GetOrderListPagingRequest pagingRequest, UserInformation userInformation)
        {
            //Filter theo Role User
            var userRole = JwtUserToken.DecodeAPITokenToRole(userInformation.UserAccessToken);
            dynamic payLoadToken = JwtUserToken.GetPayLoadFromToken(userInformation.UserAccessToken);
            userInformation.UserId = JwtUserToken.GetUserID(userInformation.UserAccessToken);
            userInformation.RoleName = userRole;
            switch (userRole)
            {
                case Commons.CUSTOMER_NAME:
                    userInformation.RoleName = Commons.CUSTOMER_NAME;
                    break;
                case Commons.PHARMACIST_NAME:
                case Commons.MANAGER_NAME:
                    userInformation.SiteId = payLoadToken.SiteID;
                    var site = await _siteRepo.Get(userInformation.SiteId);
                    var dynamicAddress = await _dynamicAddressRepo.GetAddressFromId(site.AddressId);
                    userInformation.SiteCityId = dynamicAddress.CityId;
                    userInformation.SiteDistrictId = dynamicAddress.DistrictId;
                    userInformation.SiteWardId = dynamicAddress.DistrictId;
                    break;
                default:
                    break;

            }
            return await _orderHeaderRepo.GetAllOrders(pagingRequest, userInformation);
        }

        public async Task<ViewOrderSpecific> GetSpecificOrder(string orderId, UserInformation userInformation)
        {
            try
            {
                var userRole = JwtUserToken.DecodeAPITokenToRole(userInformation.UserAccessToken);
                dynamic payLoadToken = JwtUserToken.GetPayLoadFromToken(userInformation.UserAccessToken);
                userInformation.UserId = JwtUserToken.GetUserID(userInformation.UserAccessToken);
                userInformation.RoleName = userRole;
                switch (userRole)
                {
                    case Commons.CUSTOMER_NAME:
                        userInformation.RoleName = Commons.CUSTOMER_NAME;
                        break;
                    case Commons.PHARMACIST_NAME:
                    case Commons.MANAGER_NAME:
                        userInformation.SiteId = payLoadToken.SiteID;
                        var site = await _siteRepo.Get(userInformation.SiteId);
                        var dynamicAddress = await _dynamicAddressRepo.GetAddressFromId(site.AddressId);
                        userInformation.SiteCityId = dynamicAddress.CityId;
                        userInformation.SiteDistrictId = dynamicAddress.DistrictId;
                        userInformation.SiteWardId = dynamicAddress.DistrictId;
                        break;
                    default:
                        break;

                }
            }
            catch
            {
                userInformation.RoleName = String.Empty;
            }
            var order = await _orderHeaderRepo.GetSpecificOrder(orderId);
            order.orderProducts = await _orderDetailRepo.GetViewSpecificOrderProducts(orderId);
            //chỉ khác null khi nó là giao hàng và chưa tiếp nhận
            List<OrderProductLastUnitLevel> orderProductLastUnitLevels = null;

            for (int i = 0; i < order.orderProducts.Count; i++)
            {
                var productsInOrder = order.orderProducts[i];
                //load ra order batch đối với sản phẩm có quản lý theo lô
                var productParentId = await _productDetailRepo.GetProductParentID(productsInOrder.ProductId);
                var productDetailDB = await _productDetailRepo.Get(productsInOrder.ProductId);
                var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);
                var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();
                var isBatches = await _productImportRepo.checkProductManageByBatches(productLastUnitDetail.Id);

                //Nếu đơn hàng là giao hàng và chưa được chấp nhận
                if (order.OrderTypeId == Commons.ORDER_TYPE_DELIVERY && order.NeedAcceptance && userInformation.RoleName.Equals(Commons.PHARMACIST_NAME))
                {
                    //Khởi tạo list mà chỉ khả dụng (khác null) nếu như đúng điều kiện
                    if (orderProductLastUnitLevels == null)
                    {
                        orderProductLastUnitLevels = new List<OrderProductLastUnitLevel>();
                    }
                    orderProductLastUnitLevels.Add(new OrderProductLastUnitLevel()
                    {
                        productId = productLastUnitDetail.Id,
                        productQuantity = productsInOrder.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList),
                        UnitName = productLastUnitDetail.UnitName,
                        productQuantityButOnlyOne = CountTotalQuantityFromFirstToLastUnit(productLaterList),
                        isBatches = isBatches
                    });
                }
                else if (productsInOrder.IsBatches)
                {
                    order.orderProducts[i].orderBatches = await _orderBatchRepo.GetViewSpecificOrderBatches(orderId, productLastUnitDetail.Id);
                }
            }

            //Xử lý action nhận đơn
            if (orderProductLastUnitLevels != null)
            {
                var siteDB = await _siteRepo.Get(userInformation.SiteId);
                if (siteDB.IsDelivery)
                {
                    var missingProductList = await _siteInventoryRepo.CheckMissingProductOfSiteId(userInformation.SiteId, orderProductLastUnitLevels);
                    if (missingProductList.Count > 0)
                    {
                        order.actionStatus = new ViewSpecificActionStatus();
                        order.actionStatus.CanAccept = false;
                        order.actionStatus.missingProducts = missingProductList;
                        order.actionStatus.StatusMessage = "Chi nhánh đang bị thiếu hàng, không thể tiếp nhận đơn giao hàng này!";
                    }
                    else
                    {
                        order.actionStatus = new ViewSpecificActionStatus();
                        order.actionStatus.CanAccept = true;
                        order.actionStatus.missingProducts = missingProductList;
                        order.actionStatus.StatusMessage = "Bạn có thể tiếp nhận đơn giao hàng này";
                    }
                }
                else
                {
                    order.actionStatus = new ViewSpecificActionStatus();
                    order.actionStatus.CanAccept = false;
                    order.actionStatus.missingProducts = null;
                    order.actionStatus.StatusMessage = "Chi nhánh của bạn chưa được Owner cho phép hỗ trợ giao hàng";
                }
            }
            else if (order.NeedAcceptance && userInformation.RoleName.Equals(Commons.PHARMACIST_NAME))
            {
                order.actionStatus = new ViewSpecificActionStatus();
                order.actionStatus.CanAccept = true;
                order.actionStatus.missingProducts = Enumerable.Empty<ViewSpecificMissingProduct>().ToList();
                order.actionStatus.StatusMessage = "Bạn có thể tiếp nhận đơn hàng này";
            }
            else if (!order.NeedAcceptance && userInformation.RoleName.Equals(Commons.PHARMACIST_NAME))
            {
                order.actionStatus = new ViewSpecificActionStatus();
                order.actionStatus.CanAccept = false;
                order.actionStatus.missingProducts = Enumerable.Empty<ViewSpecificMissingProduct>().ToList();
                order.actionStatus.StatusMessage = "Đơn hàng này đã có người đại diện xử lý.";
            }
            else
            {
                order.actionStatus = null;
            }

            //Lấy ra thông tin giao hàng nếu như là Đơn Giao Hàng
            if (order.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
            {
                order.orderDelivery = await _orderShipmentRepo.GetOrderDeliveryInformation(orderId);
                order.orderDelivery.FullyAddress = await _dynamicAddressRepo.GetFullAddressFromAddressId(order.orderDelivery.AddressId);
            }

            //Lấy ra thông tin đến lấy nếu như là Đơn Đến Lấy 
            if (order.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP))
            {
                order.orderPickUp = await _orderPickUpRepo.GetOrderPickUpInformation(orderId);
            }

            return order;
        }

        public async Task<IActionResult> ValidateOrder(ValidateOrderModel validateOrderModel, string pharmacistToken)
        {
            var pharmacistId = JwtUserToken.GetUserID(pharmacistToken);
            var siteId = JwtUserToken.GetWorkingSiteFromManagerAndPharmacist(pharmacistToken);

            var orderHeader = await _orderHeaderRepo.Get(validateOrderModel.OrderId);

            if (orderHeader == null) return new BadRequestObjectResult("Đơn hàng không tồn tại trong hệ thống!");

            if (orderHeader.IsApproved.HasValue) return new BadRequestObjectResult("Đơn hàng đã được duyệt rồi, không thể thao tác được nữa");

            if (!orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
            {
                if (!orderHeader.SiteId.Equals(siteId))
                {
                    return new BadRequestObjectResult("Đơn hàng này không thuộc về chi nhánh của bạn, không thể xử lý");
                }
            }
            else
            {
                var siteDB = await _siteRepo.Get(siteId);
                if (!siteDB.IsDelivery) return new BadRequestObjectResult("Chi nhánh của bạn chưa được Owner cho phép hỗ trợ giao hàng, không thể duyệt đơn hàng này. Mọi thông tin liên hệ Owner/Admin của bạn để được cấp quyền.");
            }

            if (!validateOrderModel.IsAccept && string.IsNullOrEmpty(validateOrderModel.Description))
            {
                return new BadRequestObjectResult("Từ chối đơn hàng cần lý do chính đáng");
            }

            if (!validateOrderModel.IsAccept && validateOrderModel.Description.Length < 10)
            {
                return new BadRequestObjectResult("Lý do từ chối đơn hàng phải có trên 10 kí tự.");
            }

            if (!validateOrderModel.IsAccept && orderHeader.PayType.Equals(2))
            {
                var refundModel = await _orderVNPayRepo.GetRefundVNPayModel(validateOrderModel.OrderId);
                var result = await _vnPayService.RequestARefundVNPay(pharmacistToken, validateOrderModel.IpAddress, refundModel);
                if (result is BadRequestObjectResult badRequest)
                {
                    return new BadRequestObjectResult($"Không thể yêu cầu hoàn tiền được, tin nhắn lỗi: {badRequest.Value}");
                }

                if (result is OkResult || result is OkObjectResult)
                {
                    var updateExecution = new OrderExecution()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DateOfCreate = CustomDateTime.Now.AddSeconds(3),
                        Description = "Đơn hàng đã được yêu cầu hoàn tiền thành công và sẽ được hoàn tiền trong vòng 48 giờ làm việc.",
                        IsInternalUser = true,
                        OrderId = orderHeader.Id,
                        StatusChangeFrom = validateOrderModel.IsAccept ? (orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP) ? Commons.ORDER_PICKUP_AFTERVALIDATE_ACCEPT : Commons.ORDER_DELIVERY_AFTERVALIDATE_ACCEPT) : (orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP) ? Commons.ORDER_PICKUP_AFTERVALIDATE_DENY : Commons.ORDER_DELIVERY_AFTERVALIDATE_DENY),
                        StatusChangeTo = validateOrderModel.IsAccept ? (orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP) ? Commons.ORDER_PICKUP_AFTERVALIDATE_ACCEPT : Commons.ORDER_DELIVERY_AFTERVALIDATE_ACCEPT) : (orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP) ? Commons.ORDER_PICKUP_AFTERVALIDATE_DENY : Commons.ORDER_DELIVERY_AFTERVALIDATE_DENY),
                        UserId = pharmacistId
                    };
                    await _orderExecutionRepo.Insert(updateExecution);
                }
            }

            if (orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP))
            {
                orderHeader.IsApproved = validateOrderModel.IsAccept;
                orderHeader.OrderStatus = validateOrderModel.IsAccept ? Commons.ORDER_PICKUP_AFTERVALIDATE_ACCEPT : Commons.ORDER_PICKUP_AFTERVALIDATE_DENY;
                orderHeader.PharmacistId = pharmacistId;
                orderHeader.ApprovedDate = CustomDateTime.Now;
                await _orderHeaderRepo.Update();

                var message = validateOrderModel.IsAccept ? "Đơn hàng đã được nhân viên phê duyệt và đang tiến hành chuẩn bị sản phẩm cho quý khách. Ghi chú của nhân viên: " : "Đơn hàng đã bị từ chối bởi nhân viên với lý do: ";
                var updateExecution = new OrderExecution()
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfCreate = CustomDateTime.Now,
                    Description = string.IsNullOrEmpty(validateOrderModel.Description) ? "Trống." : validateOrderModel.Description,
                    IsInternalUser = true,
                    OrderId = orderHeader.Id,
                    StatusChangeFrom = Commons.CHECKOUT_ORDER_PICKUP_ID,
                    StatusChangeTo = validateOrderModel.IsAccept ? Commons.ORDER_PICKUP_AFTERVALIDATE_ACCEPT : Commons.ORDER_PICKUP_AFTERVALIDATE_DENY,
                    UserId = pharmacistId
                };

                await _orderExecutionRepo.Insert(updateExecution);

                if (!validateOrderModel.IsAccept)
                {
                    await AddSiteInventory(orderHeader.Id, siteId);
                }

                return new OkObjectResult(validateOrderModel.IsAccept ? "Đơn hàng đã được duyệt thành công" : "Đơn hàng đã từ chối thành công");
            }

            if (orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY))
            {
                orderHeader.IsApproved = validateOrderModel.IsAccept;
                orderHeader.OrderStatus = validateOrderModel.IsAccept ? Commons.ORDER_DELIVERY_AFTERVALIDATE_ACCEPT : Commons.ORDER_DELIVERY_AFTERVALIDATE_DENY;
                orderHeader.PharmacistId = pharmacistId;
                orderHeader.ApprovedDate = CustomDateTime.Now;
                orderHeader.SiteId = siteId;
                await _orderHeaderRepo.Update();

                //Trừ tồn kho đối với đơn giao hàng được đồng ý.
                if (validateOrderModel.IsAccept)
                {
                    var productList = await _orderDetailRepo.GetListOfProductInsideOrderId(validateOrderModel.OrderId);

                    for (int i = 0; i < productList.Count; i++) 
                    { 
                        var productModel = productList[i];
                        var productParentId = await _productDetailRepo.GetProductParentID(productModel.ProductId);
                        var productDetailDB = await _productDetailRepo.Get(productModel.ProductId);
                        var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);
                        var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();
                        productList[i].QuantityAfterConvert = CountTotalQuantityFromFirstToLastUnit(productLaterList);
                        productList[i].ParentId = productParentId;
                        productList[i].productDetail = productDetailDB;
                        productList[i].listUnit = productLaterList;
                        productList[i].lastUnit = productLastUnitDetail;
                    }

                    productList = productList.OrderByDescending(x => x.QuantityAfterConvert).ToList();
                    //
                    foreach (var productModel in productList)
                    {
                        var isBatches = await _productImportRepo.checkProductManageByBatches(productModel.ProductId);

                        //Convert thành unit cuối cùng
                        var productParentId = productModel.ParentId;
                        var productDetailDB = productModel.productDetail;
                        var productLaterList = productModel.listUnit;
                        var productLastUnitDetail = productModel.lastUnit;
                        int currentQuantity = productModel.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList);

                        bool differentUnitAfterConvert = productLaterList.Count >= 2;
                        int QuantityAfterConvert = CountTotalQuantityFromFirstToLastUnit(productLaterList);
                        //Có quản lý theo lô
                        if (isBatches)
                        {

                            var listOfProductBatches = new List<OrderBatch>();
                            var availableBatches = await _siteInventoryRepo.GetAllProductBatchesAvailable(productLastUnitDetail.Id, siteId);

                            int loopBatch = 0;
                            //Quantity sau khi convert thành đơn vị cuối
                            while (currentQuantity > 0)
                            {
                                if (differentUnitAfterConvert)
                                {
                                    if ((availableBatches[loopBatch].Quantity / QuantityAfterConvert) < 1)
                                    {
                                        //bỏ qua lô này
                                        loopBatch++;
                                        continue;
                                    }
                                    else
                                    {
                                        int NumberEnoughForHighestUnit = availableBatches[loopBatch].Quantity / QuantityAfterConvert;
                                        int ConvertToLatestUnitQuantity = NumberEnoughForHighestUnit * QuantityAfterConvert;

                                        if (ConvertToLatestUnitQuantity >= currentQuantity) //chỉ cần sử dụng lô này
                                        {
                                            listOfProductBatches.Add(new OrderBatch()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                SiteInventoryBatchId = availableBatches[loopBatch].Id,
                                                OrderId = validateOrderModel.OrderId,
                                                SoldQuantity = currentQuantity
                                            });
                                            //Đã trừ
                                            var thisBatch = await _siteInventoryRepo.Get(availableBatches[loopBatch].Id);
                                            thisBatch.Quantity = thisBatch.Quantity - currentQuantity;
                                            thisBatch.UpdatedDate = CustomDateTime.Now;
                                            await _siteInventoryRepo.Update();
                                            currentQuantity = 0;
                                        }
                                        else
                                        {
                                            listOfProductBatches.Add(new OrderBatch()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                SiteInventoryBatchId = availableBatches[loopBatch].Id,
                                                OrderId = validateOrderModel.OrderId,
                                                SoldQuantity = ConvertToLatestUnitQuantity
                                            });

                                            currentQuantity = currentQuantity - ConvertToLatestUnitQuantity;
                                            var batchDB = await _siteInventoryRepo.Get(availableBatches[loopBatch].Id);
                                            batchDB.Quantity -= ConvertToLatestUnitQuantity;
                                            batchDB.UpdatedDate = CustomDateTime.Now;
                                            await _siteInventoryRepo.Update();
                                            loopBatch++;
                                        }
                                    }
                                }
                                else
                                {
                                    if (availableBatches[loopBatch].Quantity > currentQuantity)
                                    {
                                        listOfProductBatches.Add(new OrderBatch()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            SiteInventoryBatchId = availableBatches[loopBatch].Id,
                                            OrderId = validateOrderModel.OrderId,
                                            SoldQuantity = currentQuantity
                                        });
                                        //Đã trừ
                                        var thisBatch = await _siteInventoryRepo.Get(availableBatches[loopBatch].Id);
                                        thisBatch.Quantity = thisBatch.Quantity - currentQuantity;
                                        thisBatch.UpdatedDate = CustomDateTime.Now;
                                        await _siteInventoryRepo.Update();
                                        currentQuantity = 0;
                                    }
                                    else
                                    {
                                        listOfProductBatches.Add(new OrderBatch()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            SiteInventoryBatchId = availableBatches[loopBatch].Id,
                                            OrderId = validateOrderModel.OrderId,
                                            SoldQuantity = availableBatches[loopBatch].Quantity
                                        });

                                        currentQuantity = currentQuantity - availableBatches[loopBatch].Quantity;
                                        var batchDB = await _siteInventoryRepo.Get(availableBatches[loopBatch].Id);
                                        batchDB.Quantity = 0;
                                        batchDB.UpdatedDate = CustomDateTime.Now;
                                        await _siteInventoryRepo.Update();
                                        loopBatch++;
                                    }
                                }
                            }
                            await _orderBatchRepo.InsertRange(listOfProductBatches);
                        }
                        else //không quản lý theo lô
                        {
                            var siteInventory = await _siteInventoryRepo.GetSiteInventory(siteId, productLastUnitDetail.Id);
                            siteInventory.Quantity = siteInventory.Quantity - currentQuantity;
                            siteInventory.UpdatedDate = CustomDateTime.Now;
                            await _siteInventoryRepo.Update();
                        }
                    }
                }
                var message = validateOrderModel.IsAccept ? "Đơn hàng đã được nhân viên phê duyệt và đang tiến hành chuẩn bị sản phẩm cho quý khách. Ghi chú của nhân viên: " : "Đơn hàng đã bị từ chối bởi nhân viên với lý do: ";

                var updateExecution = new OrderExecution()
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfCreate = CustomDateTime.Now,
                    Description = string.IsNullOrEmpty(validateOrderModel.Description) ? "Trống." : validateOrderModel.Description,
                    IsInternalUser = true,
                    OrderId = orderHeader.Id,
                    StatusChangeFrom = Commons.CHECKOUT_ORDER_DELIVERY_ID,
                    StatusChangeTo = validateOrderModel.IsAccept ? Commons.ORDER_DELIVERY_AFTERVALIDATE_ACCEPT : Commons.ORDER_DELIVERY_AFTERVALIDATE_DENY,
                    UserId = pharmacistId
                };

                await _orderExecutionRepo.Insert(updateExecution);
                return new OkObjectResult(validateOrderModel.IsAccept ? "Đơn hàng đã được duyệt thành công" : "Đơn hàng đã từ chối thành công");
            }


            //Xử lý hoàn tiền nếu thanh toán VN Pay

            return new BadRequestObjectResult("Đơn hàng tại chỗ không thể được duyệt!");
        }

        public async Task<IActionResult> ExecuteOrder(OrderExecutionModel orderExecutionModel, string pharmacistToken)
        {
            var pharmacistId = JwtUserToken.GetUserID(pharmacistToken);
            var orderHeader = await _orderHeaderRepo.Get(orderExecutionModel.OrderId);

            if (orderHeader == null) return new BadRequestObjectResult("Đơn hàng không tồn tại trong hệ thống");

            if (!pharmacistId.Equals(orderHeader.PharmacistId) && !orderHeader.OrderTypeId.Equals(Commons.ORDER_TYPE_PICKUP)) return new BadRequestObjectResult("Pharmacist không đại diện xử lý đơn hàng này, không thể yêu cầu chuyển trạng thái.");

            var orderStatusDB = await _orderStatusRepo.Get(orderExecutionModel.OrderStatusId);

            if (orderStatusDB == null) return new BadRequestObjectResult("Trạng thái đơn hàng không tồn tại trong hệ thống");

            if (!orderStatusDB.ApplyForType.Equals(orderHeader.OrderTypeId)) return new BadRequestObjectResult("Trạng thái đơn hàng không hợp lệ, không dành cho loại đơn hàng này!");

            if (orderExecutionModel.OrderStatusId.Equals(Commons.ORDER_DELIVERY_STATUS_DONE) || orderExecutionModel.OrderStatusId.Equals(Commons.ORDER_PICKUP_STATUS_DONE))
            {
                var doneExecution = new OrderExecution()
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfCreate = CustomDateTime.Now,
                    Description = orderExecutionModel.OrderStatusId.Equals(Commons.ORDER_DELIVERY_STATUS_DONE) ? "Đã giao hàng thành công cho khách hàng và thu hộ số tiền." : "Khách hàng đã nhận hàng thành công và đã thu hộ số tiền.",
                    IsInternalUser = true,
                    OrderId = orderHeader.Id,
                    StatusChangeFrom = orderExecutionModel.OrderStatusId,
                    StatusChangeTo = orderExecutionModel.OrderStatusId,
                    UserId = pharmacistId
                };
                await _orderExecutionRepo.Insert(doneExecution);

                orderHeader.IsPaid = true;
                await _orderHeaderRepo.Update();

                //cộng điểm khách hàng
                var orderContactInfoDB = await _orderContactInfoRepo.GetCustomerInfoBasedOnOrderId(orderHeader.Id);
                if (orderContactInfoDB != null)
                {
                    if (!string.IsNullOrEmpty(orderContactInfoDB.CustomerId) && orderHeader.TotalPrice >= 15000)
                    {
                        var customerPoint = new CustomerPoint()
                        {
                            CustomerId = orderContactInfoDB.CustomerId,
                            CreateDate = CustomDateTime.Now,
                            Id = Guid.NewGuid().ToString(),
                            IsPlus = true,
                            Point = (int)(orderHeader.TotalPrice / 15000) + (orderHeader.TotalPrice % 15000 == 0 ? 0 : 1),
                            Description = $"Điểm tích lũy từ đơn hàng #{orderHeader.Id}"
                        };
                        await _customerPointRepo.Insert(customerPoint);
                    }
                }
            }
            else
            {
                string orderDescription = Commons.RecommendDescription(orderExecutionModel.OrderStatusId);
                var updateExecution = new OrderExecution()
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfCreate = CustomDateTime.Now,
                    Description = orderDescription + (string.IsNullOrEmpty(orderExecutionModel.Description) ? "Trống" : orderExecutionModel.Description),
                    IsInternalUser = true,
                    OrderId = orderHeader.Id,
                    StatusChangeFrom = orderHeader.OrderStatus,
                    StatusChangeTo = orderExecutionModel.OrderStatusId,
                    UserId = pharmacistId
                };

                await _orderExecutionRepo.Insert(updateExecution);

            }

            orderHeader.OrderStatus = orderExecutionModel.OrderStatusId;
            await _orderHeaderRepo.Update();

            return new OkObjectResult("Cập nhật trạng thái đơn hàng thành công");
        }

        public async Task<IActionResult> UpdateOrderProductNoteModel(List<UpdateOrderProductNoteModel> ListNoteModel)
        {
            foreach (var NoteModel in ListNoteModel)
            {
                var OrderDetailDB = await _orderDetailRepo.Get(NoteModel.OrderDetailId);
                OrderDetailDB.Note = string.IsNullOrEmpty(NoteModel.Note) ? null : NoteModel.Note;
                await _orderDetailRepo.Update();
            }

            return new OkObjectResult("Ghi chú từng món hàng đã được cập nhật thành công!");
        }

        public async Task<IActionResult> GetOrderExecutionHistory(string orderId)
        {
            var orderHistoryModel = new List<ViewOrderHistoryModel>();
            var information = new OrderExecutionInformation()
            {
                users = new(),
                statuses = new()
            };
            var orderHistoryDBList = await _orderExecutionRepo.ViewOrderHistory(orderId);

            if (!orderHistoryDBList.Any()) return new NotFoundObjectResult("Không tìm thấy đơn hàng hoặc đơn hàng chưa được tiến hành.");
            for (int i = 0; i < orderHistoryDBList.Count; i++)
            {
                var orderHistoryDB = orderHistoryDBList[i];
                var matchingModel = orderHistoryModel.Where(x => x.StatusId.Equals(orderHistoryDB.StatusId)).FirstOrDefault();
                if (matchingModel != null)
                {
                    matchingModel.statusDescriptions.Add(new StatusDescription()
                    {
                        Description = orderHistoryDB.Description,
                        Time = orderHistoryDB.Time
                    });
                }

                if (matchingModel == null)
                {
                    var findingUserModel = information.users.Where(x => x.UserId.Equals(orderHistoryDB.UserId)).FirstOrDefault();
                    var findingStatusModel = information.statuses.Where(x => x.StatusId.Equals(orderHistoryDB.StatusId)).FirstOrDefault();
                    var statusName = String.Empty;
                    var userName = String.Empty;
                    if (findingUserModel != null)
                    {
                        userName = findingUserModel.UserName;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(orderHistoryDB.UserId))
                        {
                            var userModelDB = await _orderExecutionRepo.GetUserOrderExedution(orderHistoryDB.UserId, orderHistoryDB.IsInternal);
                            information.users.Add(userModelDB);
                            userName = userModelDB.UserName;
                        }
                        else
                        {
                            userName = "Khách vãng lai";
                        }

                    }

                    if (findingStatusModel != null)
                    {
                        statusName = findingStatusModel.StatusName;
                    }
                    else
                    {
                        var statusModelDB = await _orderExecutionRepo.GetStatusOrderExecution(orderHistoryDB.StatusId);
                        information.statuses.Add(statusModelDB);
                        statusName = statusModelDB.StatusName;
                    }
                    orderHistoryModel.Add(new ViewOrderHistoryModel()
                    {
                        StatusId = orderHistoryDB.StatusId,
                        UserId = orderHistoryDB.UserId,
                        IsInternal = orderHistoryDB.IsInternal,
                        FullName = userName,
                        StatusName = statusName,
                        statusDescriptions = new List<StatusDescription>()
                        {
                            new StatusDescription()
                            {
                                Description = orderHistoryDB.Description,
                                Time = orderHistoryDB.Time
                            }
                        }
                    });
                }
            }

            return new OkObjectResult(orderHistoryModel);
        }

        private async Task AddSiteInventory(string orderId, string siteId)
        {
            var listOrderProduct = await _orderDetailRepo.GetViewSpecificOrderProducts(orderId);

            for (int i = 0; i < listOrderProduct.Count; i++)
            {

                var orderProduct = listOrderProduct[i];

                var productParentId = await _productDetailRepo.GetProductParentID(orderProduct.ProductId);
                var productDetailDB = await _productDetailRepo.Get(orderProduct.ProductId);
                var productLaterList = await _productDetailRepo.GetProductLaterUnit(productParentId, productDetailDB.UnitLevel);
                var productLastUnitDetail = productLaterList.OrderByDescending(x => x.UnitLevel).FirstOrDefault();

                if (orderProduct.IsBatches)
                {
                    var orderProductBatchList = await _siteInventoryRepo.GetAllSiteInventoryBatchFromOrderProductBatch(productLastUnitDetail.Id, siteId, orderId);

                    for (int j = 0; j < orderProductBatchList.Count; j++)
                    {
                        var orderProductBatch = orderProductBatchList[j];
                        var siteInventory = await _siteInventoryRepo.Get(orderProductBatch.SiteInventoryBatchId);
                        siteInventory.Quantity += orderProductBatch.SoldQuantity;
                        siteInventory.UpdatedDate = CustomDateTime.Now;
                        await _siteInventoryRepo.Update();
                    }
                }
                else
                {
                    int currentQuantity = orderProduct.Quantity * CountTotalQuantityFromFirstToLastUnit(productLaterList);
                    var siteInventory = await _siteInventoryRepo.GetSiteInventory(siteId, productLastUnitDetail.Id);
                    siteInventory.Quantity += currentQuantity;
                    siteInventory.UpdatedDate = CustomDateTime.Now;
                    await _siteInventoryRepo.Update();
                }
            }
        }

        private int CountTotalQuantityFromFirstToLastUnit(List<ProductUnitModel> productDetailList)
        {
            int totalQuantity = 1;

            if (productDetailList.Count <= 1) return totalQuantity;

            for (int i = 0; i < productDetailList.Count - 1; i++)
            {
                totalQuantity = totalQuantity * productDetailList[(i + 1)].Quantitative;
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

        private bool RequestARefundVNPay()
        {
            return true;
        }
    }



    public class CartModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public bool isBatches { get; set; }

        public int QuantityConvert { get; set; }
    }
}
