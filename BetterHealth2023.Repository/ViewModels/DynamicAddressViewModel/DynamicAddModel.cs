using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel
{
    public class DynamicAddModel
    {

     
        public string Id { get; set; }
       
        public string CityId { get; set; }
       
        public string DistrictId { get; set; }
     
        public string WardId { get; set; }
     
        public string HomeAddress { get; set; }
    }
}
