using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels
{
    public class RegisterEmployeeStatus
    {
        //check employee password
        public string ConfirmPasswordFailed { get; set; }
        public string DuplicateEmail { get; set; }
        public string DuplicatePhoneNo { get; set; }
        public string DuplicateUsername { get; set; }
        public string OtherError { get; set; }
        public bool isError { get; set; } = false;
    }
}
