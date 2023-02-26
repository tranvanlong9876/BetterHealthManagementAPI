using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels
{
    public class VNPayInformationModel
    {
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }

        [Required]
        public string IpAddress { get; set; }

        [Required]
        public string UrlCallBack { get; set; }
    }
}
