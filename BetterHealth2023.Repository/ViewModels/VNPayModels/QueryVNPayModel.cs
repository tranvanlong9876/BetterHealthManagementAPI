using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.VNPayModels
{
    public class QueryVNPayModel
    {

        [Required]
        public string OrderId { get; set; }

        [BindNever]
        [JsonIgnore]
        public string TransactionDate { get; set; }

        [Required]
        public string IpAddress { get; set; }
    }
}
