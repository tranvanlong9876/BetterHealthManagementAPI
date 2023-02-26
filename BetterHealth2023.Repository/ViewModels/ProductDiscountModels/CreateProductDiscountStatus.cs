using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels
{
    public class CreateProductDiscountStatus
    {
        public bool isError { get; set; }

        public string VariablesError { get; set; }
        public string alreadyExistDiscount { get; set; }

        public string productError { get; set; }

    }
}
