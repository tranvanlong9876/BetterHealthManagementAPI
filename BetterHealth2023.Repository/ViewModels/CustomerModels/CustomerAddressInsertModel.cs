using System.ComponentModel.DataAnnotations;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CustomerModels
{
    public class CustomerAddressInsertModel
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string CityId { get; set; }
        [Required]
        public string DistrictId { get; set; }
        [Required]
        public string WardId { get; set; }
        [Required]
        public string HomeAddress { get; set; }
        [Required]
        public bool IsMainAddress { get; set; }
    }
}
