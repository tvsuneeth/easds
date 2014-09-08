using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TWG.EASDataService.Api.Extensions
{
    public static class StringExtensions
    {
        public static DateTime GetDateFromString(this string dateString)
        {
            int year = Convert.ToInt32(dateString.Substring(0, 4));
            int month = Convert.ToInt32(dateString.Substring(4, 2));
            int day = Convert.ToInt32(dateString.Substring(6, 2));
            int hour = Convert.ToInt32(dateString.Substring(9, 2));
            int minute = Convert.ToInt32(dateString.Substring(11, 2));
            int sec = Convert.ToInt32(dateString.Substring(13, 2));
            return new DateTime(year, month, day, hour, minute, sec);
        }
    }
}