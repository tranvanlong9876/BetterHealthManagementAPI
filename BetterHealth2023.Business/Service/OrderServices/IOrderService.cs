using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderPickUpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices
{
    public interface IOrderService
    {
        public Task<ViewSiteToPickUpStatus> GetViewSiteToPickUps(CartEntrance cartEntrance);
    }
}
