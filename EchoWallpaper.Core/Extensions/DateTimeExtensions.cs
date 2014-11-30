using System;

namespace EchoWallpaper.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool LastDateOk(this DateTime lastRunDate)
        {
            var date = DateTime.Now;

            if (date.Date == lastRunDate.Date)
            {
                return false;
            }

            if (date.Date.Day != 1 && lastRunDate.Date.Month == date.Date.Month)
            {
                return false;
            }

            return true;
        }
    }
}
