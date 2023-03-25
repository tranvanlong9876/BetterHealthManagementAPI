using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.OrderModels.OrderExecutionModels
{
    public class OrderExecutionInformation
    {
        public List<UserExecution> users { get; set; }
        public List<StatusExecution> statuses { get; set; }
    }

    public class UserExecution { 
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class StatusExecution
    {
        public string StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
