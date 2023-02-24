using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
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

        public OrderService(ISiteInventoryRepo siteInventoryRepo)
        {
            _siteInventoryRepo = siteInventoryRepo;
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
