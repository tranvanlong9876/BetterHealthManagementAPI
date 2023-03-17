using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public static class CustomDateTime
    {
        private static TimeZoneInfo _timeZone;

        static CustomDateTime()
        {
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        }

        public static DateTime Now
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone); }
        }
    }
}
