using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels
{
    public class UserInfoModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }
        public string Fullname { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string CityID { get; set; }
        public string DistrictID { get; set; }
        public string WardID { get; set; }
        public string HomeNumber { get; set; }
        public string AddressID { get; set; }
        public string FullyAddress { get; set; }
        public string ImageUrl { get; set; }

        public int Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DOB { get; set; }
        public int Gender { get; set; }
    }
}
