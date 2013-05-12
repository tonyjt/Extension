using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomExtension
{
    public static class ILogExtension
    {
        public static void ErrorFormat(this ILog log, Exception ex, string format, params object[] args)
        {
            string message = string.Format(format, args);
            log.Error(message, ex);
        }

        public static void ErrorInFunction(this ILog log, Exception ex)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();

            log.Error(string.Format("{0} message:{1} source:{2} ", methodBase.Name, ex.Message, ex.StackTrace));
        }

        public static void ErrorInFunction(this ILog log, string message)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();

            log.Error(string.Format("{0} message:{1}", methodBase.Name, message));
        }
    }
}
