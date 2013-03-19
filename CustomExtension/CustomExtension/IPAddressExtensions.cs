using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace CustomExtension
{
    public static class IPAddressExtensions
    {
        public static long ToLong(this IPAddress address)
        {
            if (address.GetAddressBytes().Length == 4)
                return (long)BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            else if (address.GetAddressBytes().Length == 8)
                return (long)BitConverter.ToUInt64(address.GetAddressBytes(), 0);
            else
                throw new InvalidOperationException("Dude, what the heck?");
        }

        public static IPAddress Current()
        {
            HttpContext context = HttpContext.Current;
            IPAddress ipAddress;
            if (context != null && context.Request.UserHostAddress != null&&context.Request.UserHostAddress!="::1")
            //ipAddress = IPAddress.Parse(context.Request.UserHostAddress);
            {
                ipAddress = IPAddress.Parse(GetIP(context));
                
            }
            else
                ipAddress = IPAddress.Any;
            return ipAddress;
        }

        /// <summary>
        /// Get real IP address from client-side.
        /// If client is using proxy, try to return real client IP, else, return proxy IP.
        /// </summary>
        /// <param name="context">current HttpContext.</param>
        /// <returns>IP address as string format.</returns>
        /// <remarks>Lance added on 2009-8-28</remarks>
        private static string GetIP(HttpContext context)
        {
            if (context.Request.ServerVariables["HTTP_VIA"] != null) // using proxy
            {
                return context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();  // Return real client IP.
            }
            else// not using proxy or can't get the Client IP
            {
                // Request.UserHostAddress's time cost is 380times than Request.ServerVariables["REMOTE_ADDR"]
                return context.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
            }
        }
    }
}
