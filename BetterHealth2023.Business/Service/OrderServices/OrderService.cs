using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderCheckOutModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly ISiteInventoryRepo _siteInventoryRepo;
        private readonly ICustomerPointRepo _customerPointRepo;
        private readonly ICustomerRepo _customerRepo;

        public OrderService(ISiteInventoryRepo siteInventoryRepo)
        {
            _siteInventoryRepo = siteInventoryRepo;
        }

        public async Task<CreateOrderCheckOutStatus> CheckOutOrder(CheckOutOrderModel checkOutOrderModel, string CustomerId)
        {
            var checkError = new CreateOrderCheckOutStatus();
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
                Id = Guid.NewGuid().ToString(),
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
                SiteId = checkOutOrderModel.OrderTypeId == 3 ? null : checkOutOrderModel.SiteId
            };

            if(orderHeaderDB.OrderTypeId == Commons.ORDER_TYPE_DELIVERY)
            {
                var orderShipmentDB = new OrderShipment()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = orderHeaderDB.Id,
                    //StartAddressId = Get Current Site AddressId
                    //DestinationId = From Order Contact,
                    ShippingFee = checkOutOrderModel.ShippingPrice,
                };
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
            }

            throw new NotImplementedException();
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
                
                for(int i = 0; i < productIdList.Count; i++)
                {
                    cartModels.Add(new CartModel()
                    {
                        ProductId = productIdList[i],
                        Quantity = productQuantityList[i]
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


    }

    public class CartModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
