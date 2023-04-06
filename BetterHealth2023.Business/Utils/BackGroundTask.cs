using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CartService;
using System;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public class BackGroundTask
    {
        private readonly ICartService _cartService;

        public BackGroundTask(ICartService cartService)
        {
            _cartService = cartService;
        }

        public void Start()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromHours(1));
                    await _cartService.RunRemoveCartHourly();
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}
