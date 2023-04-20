using System;
using System.Collections.Generic;
using System.Globalization;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Commons
{
    public static class Commons
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

        public const string CUSTOMER_NAME = "Customer";

        public const string TOTAL_INTERNAL_ROLE_NAME = "Manager,Pharmacist,Owner,Admin";


        public static readonly List<string> ROLES = new()
        {
            MANAGER,
            PHARMACIST,
            OWNER,
            ADMIN
        };

        public const string CHECKOUT_ORDER_DIRECTLY_ID = "1";
        public const string CHECKOUT_ORDER_PICKUP_ID = "2";
        public const string CHECKOUT_ORDER_DELIVERY_ID = "5";

        public const int ORDER_TYPE_DIRECTLY = 1;
        public const int ORDER_TYPE_PICKUP = 2;
        public const int ORDER_TYPE_DELIVERY = 3;

        public const string ORDER_PICKUP_AFTERVALIDATE_ACCEPT = "3";
        public const string ORDER_PICKUP_AFTERVALIDATE_DENY = "10";

        public const string ORDER_DELIVERY_AFTERVALIDATE_ACCEPT = "6";
        public const string ORDER_DELIVERY_AFTERVALIDATE_DENY = "11";

        public const string ORDER_PICKUP_STATUS_DONE = "4";
        public const string ORDER_DELIVERY_STATUS_DONE = "8";

        public static readonly string[] COMPLETED_ORDERSTATUS_ID = new string[] { "1", "4", "8", "10", "11" };
        public enum OrderType
        {
            AtStore = 1,
            PickUp = 2,
            Delivery = 3
        }

        public enum OrderPayType
        {
            COD = 1,
            VNPay = 2
        }

        public static string ConvertToVietNamDatetime(DateTime dateTime)
        {
            return DateTime.ParseExact(dateTime.ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).ToString("HH:mm 'ngày' dd/MM/yyyy");
        }

        public static string ConvertToOrderPayTypeString(this OrderPayType orderPayType)
        {
            switch (orderPayType)
            {
                case OrderPayType.COD:
                    return "Thanh toán khi nhận hàng";
                case OrderPayType.VNPay:
                    return "Thanh toán VN Pay";
                default:
                    throw new ArgumentException($"Invalid value for Payment Method: {orderPayType}", nameof(orderPayType));
            }
        }

        public static string ConvertToOrderTypeString(this OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.AtStore:
                    return "Bán tại chỗ";
                case OrderType.PickUp:
                    return "Đến lấy tại cửa hàng";
                case OrderType.Delivery:
                    return "Giao hàng tận nơi";
                default:
                    throw new ArgumentException($"Invalid value for DeliveryMethod: {orderType}", nameof(orderType));
            }
        }

        public const string ORDER_DELIVERY_7 = "Đơn hàng đã được nhân viên chuẩn bị và đang tiến hành giao hàng đến bạn. Ghi chú của nhân viên: ";
        public const string ORDER_PICKUP_9 = "Đơn giao đã được nhân viên chuẩn bị xong. Ghi chú của nhân viên: ";

        public static string RecommendDescription(string orderStatusId)
        {
            switch (orderStatusId)
            {
                case "7": return ORDER_DELIVERY_7;

                case "9": return ORDER_PICKUP_9;

                default: return "Ghi chú: ";
            }
        }

        public enum UserTarget
        {
            Children = 1,
            Audult = 2,
            Elderly = 3,
            BreastfeedingWoman = 4
        }

        public static string ALL_USER_TARGET_USAGE = "Phù hợp với mọi đối tượng";
        public static string ConvertToUserTargetString(this UserTarget userTarget)
        {
            switch (userTarget)
            {
                case UserTarget.Children:
                    return "Trẻ em";
                case UserTarget.Audult:
                    return "Người lớn";
                case UserTarget.Elderly:
                    return "Người cao tuổi";
                case UserTarget.BreastfeedingWoman:
                    return "Phụ nữ cho con bú";
                default:
                    return "Mọi đối tượng";
            }
        }

        //Error Message

        public static readonly string ERROR_403_FORBIDDEN_MSG = "Forbidden: You don't have permission to access this resource";

        public static readonly string ERROR_404_INVALID_DATA_MSG = "Resource is not exist";

        public static readonly string ERROR_401_LOGIN_FAILED_MSG = "Incorrect Email or Password";

        public static readonly string ERROR_500_USER_NOT_FOUND_MSG = "User Not Found in DB";

        public static readonly string DUMB_TEST = "https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=AIzaSyCopthNKjHMIgHQBGwqYJNxcbVux2tGcCk";
    }
}