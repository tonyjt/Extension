using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExtension
{
    public static class DateTimeExtension
    {
        public static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        public static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        public static int ToTicket(this DateTime target)
        {
            DateTime baseTime = new DateTime(1970, 1, 1);
            TimeSpan ts = target.ToUniversalTime() - baseTime.ToUniversalTime();
            return Convert.ToInt32(ts.TotalSeconds);
        }

        /// <summary>
        /// 格式 yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToStandardString(this DateTime target)
        {
            return target.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 格式 yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToStandardString(this DateTime? target,string defaultStrng = "--")
        {
            if (target.HasValue)
                return target.Value.ToStandardString();
            else
                return defaultStrng;
        }
        /// <summary>
        /// 格式 yyyy-MM-dd
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToStandardDateString(this DateTime target)
        {
            return target.ToString("yyyy-MM-dd");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToStandardDateString(this DateTime? target, string defaultStrng = "--")
        {
            if (target.HasValue)
                return target.Value.ToStandardDateString();
            else
                return defaultStrng;
        }
        /// <summary>
        /// 格式 HHmm
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToStandardTimeString(this DateTime target)
        {
            return target.ToString("HHmm");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToStandardTimeString(this DateTime? target, string defaultStrng = "--")
        {
            if (target.HasValue)
                return target.Value.ToStandardTimeString();
            else
                return defaultStrng;
        }
        /// <summary>
        /// 格式 yyMMdd
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToLocalShortDateString(this DateTime target)
        {
            return target.ToString("yyMMdd");
        }


        public static string ToOrderDateString(this DateTime target)
        {
            return target.ToString("yyyyMMddHH");
        }

        /// <summary>
        /// 获取ddMMMyy格式 例如 12JAN12  2012/01/12
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToUsMonthString(this DateTime target)
        {
            switch (target.ToString("MM"))
            {
                case "01": return string.Format("{0}JAN{1}", target.ToString("dd"), target.ToString("yy"));
                case "02": return string.Format("{0}FEB{1}", target.ToString("dd"), target.ToString("yy"));
                case "03": return string.Format("{0}MAR{1}", target.ToString("dd"), target.ToString("yy"));
                case "04": return string.Format("{0}APR{1}", target.ToString("dd"), target.ToString("yy"));
                case "05": return string.Format("{0}MAY{1}", target.ToString("dd"), target.ToString("yy"));
                case "06": return string.Format("{0}JUN{1}", target.ToString("dd"), target.ToString("yy"));
                case "07": return string.Format("{0}JUL{1}", target.ToString("dd"), target.ToString("yy"));
                case "08": return string.Format("{0}AUG{1}", target.ToString("dd"), target.ToString("yy"));
                case "09": return string.Format("{0}SEP{1}", target.ToString("dd"), target.ToString("yy"));
                case "10": return string.Format("{0}OCT{1}", target.ToString("dd"), target.ToString("yy"));
                case "11": return string.Format("{0}NOV{1}", target.ToString("dd"), target.ToString("yy"));
                case "12": return string.Format("{0}DEC{1}", target.ToString("dd"), target.ToString("yy"));
                default: return "";
            }
        }

        /// <summary>
        /// 获取ddMMM格式 例如 12MAY
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToUsMonthShortString(this DateTime target)
        {
            string usMonthString = target.ToUsMonthString();
            if (string.IsNullOrEmpty(usMonthString))
                return "";
            return usMonthString.Substring(0, 5);
        }



        public static bool IsWeekend(this DateTime target)
        {
            int week = (int)target.DayOfWeek;
            if (week == 0) //星期天是7
                week = 7;

            if (week > 5)
                return true;
            else
                return false;
        }



    }
}
