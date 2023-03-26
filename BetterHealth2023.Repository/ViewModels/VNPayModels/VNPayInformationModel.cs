﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels
{
    public class VNPayInformationModel
    {
        [Required]
        public double Amount { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public string IpAddress { get; set; }

        [Required]
        public string UrlCallBack { get; set; }
    }
}
