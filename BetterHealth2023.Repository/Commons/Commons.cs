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

        // Show Status

        public static readonly string DRAFT = "Draft";
        public static readonly string APPROVE = "Approve";
        public static readonly string REJECT = "Reject";
        public static readonly string PUBLIC = "Public";
        public static readonly string UNPUBLIC = "UnPublic";
        public static readonly string PENDING = "Pending";
        public static readonly string HIDDEN = "Hidden";
        public static readonly string ENDED = "Ended";

        public static readonly List<string> SHOWSTATUS = new()
        {
            DRAFT,
            APPROVE,
            REJECT,
            PUBLIC,
            UNPUBLIC,
            PENDING,
            HIDDEN
        };

        //Show Type

        public static readonly string INDOOR = "Indoor";
        public static readonly string OUTDOOR = "Outdoor";
        public static readonly string ALL = "All";

        public static readonly List<string> SHOWTYPES = new()
        {
            INDOOR,
            OUTDOOR,
        };

        //Role

        public static readonly int ADMIN = 1;
        public static readonly int ORGANIZER = 1;
        public static readonly int STAFF = 2;
        public static readonly int TICKETINSPECTOR = 3;
        public static readonly int ARTIST = 4;
        public static readonly int CUSTOMER = 5;


        public static readonly List<int> ROLES = new()
        {
            ADMIN,
            ORGANIZER,
            STAFF,
            TICKETINSPECTOR,
            ARTIST,
            CUSTOMER
        };

        //BookingLink

        public static readonly string BOOKING_LINK_PREFIX = "https://ultratix.net/show/";

        public static readonly string CAMPAIGN_ID_PARAMETER = "?c=";

        //Hub

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