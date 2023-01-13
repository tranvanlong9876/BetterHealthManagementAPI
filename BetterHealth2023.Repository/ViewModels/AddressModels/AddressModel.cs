using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.AddressModels
{
    public class AddressModel
    {
        public string AddressId { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string HomeAddress { get; set; }
    }
}
