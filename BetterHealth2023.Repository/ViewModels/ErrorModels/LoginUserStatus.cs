using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InternalUserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels
{
    public class LoginUserStatus
    {
        public string UserNotFound { get; set; }
        public string UserInactive { get; set; }
        public string WrongPassword { get; set; }

        public InternalUserTokenModel userToken { get; set; }

        public string OtherError { get; set; }
        public bool isError { get; set; } = false;
    }
}
