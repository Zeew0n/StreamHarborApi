using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowTaskManager.Application.Helper.Date
{
    public static class DateHelper
    {
        public static string FormatDateTime(this DateTime dateTime)
        {
            return dateTime != DateTime.MinValue ? dateTime.ToString("dd/MM/yyyy HH:mm:ss tt") : string.Empty;
        }

        public static string FormatDateTime(this DateTime dateTime, string format)
        {
            return dateTime != DateTime.MinValue ? dateTime.ToString(format) : string.Empty;
        }

        public static string FormatDateTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? Convert.ToDateTime(dateTime.Value).ToString("dd/MM/yyyy HH:mm:ss tt") : string.Empty;
        }

        public static string FormatDateTime(this DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? Convert.ToDateTime(dateTime.Value).ToString(format) : string.Empty;
        }

        public static string FormatDateTime(this string dateTime)
        {
            return string.IsNullOrEmpty(dateTime) ? string.Empty : Convert.ToDateTime(dateTime).ToString("dd/MM/yyyy HH:mm:ss tt");
        }

        public static string FormatDateTime(this string dateTime, string format)
        {
            return string.IsNullOrEmpty(dateTime) ? string.Empty : Convert.ToDateTime(dateTime).ToString(format);
        }
    }
}
