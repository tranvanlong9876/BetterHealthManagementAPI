using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerTokenModel
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Status { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
    }
}
