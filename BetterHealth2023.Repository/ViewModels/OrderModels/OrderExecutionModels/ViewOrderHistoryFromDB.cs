using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels
{
    public class ViewOrderHistoryFromDB
    {
        public string StatusId { get; set; }
        public string UserId { get; set; }
        public bool IsInternal { get; set; }
        public string Description { get; set; }
        
        public DateTime Time { get; set; }
    }
}
