using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels
{
    public class ViewOrderHistoryModel
    {
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string UserId { get; set; }
        public bool IsInternal { get; set; }
        public string FullName { get; set; }
        public List<StatusDescription> statusDescriptions { get; set; }
    }

    public class StatusDescription
    {
        public string Description { get; set; }
        public DateTime Time { get; set; }

    }
}
