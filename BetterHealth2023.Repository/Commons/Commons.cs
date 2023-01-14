using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Commons
{
    public class Commons
    {
        public static readonly string JWTClaimID = "Id";
        public static readonly string JWTClaimName = "Name";
        public static readonly string JWTClaimEmail = "Email";
        public static readonly string JWTClaimRoleID = "RoleID";


        //Internal User Role ID

        public static readonly string MANAGER = "1";
        public static readonly string PHARMACIST = "2";
        public static readonly string OWNER = "3";
        public static readonly string ADMIN = "4";

        //Internal User Role Name
        public const string MANAGER_NAME = "Manager";
        public const string PHARMACIST_NAME = "Pharmacist";
        public const string OWNER_NAME = "Owner";
        public const string ADMIN_NAME = "Admin";

        public const string TOTAL_INTERNAL_ROLE_NAME = "Manager,Pharmacist,Owner,Admin";


        public static readonly List<string> ROLES = new()
        {
            MANAGER,
            PHARMACIST,
            OWNER,
            ADMIN
        };

        //-Local
        public static readonly string LOCAL_HUB = "https://localhost:7042/notifyhub";
        //-Server
        public static readonly string SERVER_HUB = "https://ultratixapi.azurewebsites.net/notifyhub";

        // Image Host 
        public static readonly string IMG_HOST_URL = "https://ultratiximg.blob.core.windows.net/ultratixshowimg/";

        //Error Message

        public static readonly string ERROR_403_FORBIDDEN_MSG = "Forbidden: You don't have permission to access this resource";

        public static readonly string ERROR_404_INVALID_DATA_MSG = "Resource is not exist";

        public static readonly string ERROR_401_LOGIN_FAILED_MSG = "Incorrect Email or Password";

        public static readonly string ERROR_500_USER_NOT_FOUND_MSG = "User Not Found in DB";

        public static readonly string DUMB_TEST = "https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=AIzaSyCopthNKjHMIgHQBGwqYJNxcbVux2tGcCk";
    }
}