using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels
{
    public class UpdateInternalUserStatus
    {
        public string WrongOldPassword { get; set; }
        
        public string RequireChangePasswordFailed { get; set; }
        public string ConfirmPasswordFailed { get; set; }
        public string DuplicateEmail { get; set; }
        public string DuplicatePhoneNo { get; set; }
        public string OtherError { get; set; }
        public string missingSiteID { get; set; }
        public bool isError { get; set; } = false;
    }
}
