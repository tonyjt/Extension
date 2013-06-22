using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExtension.Helper
{
    public static class ManagerHelper
    {
        public static T GetModel<P, T>(P key, Func<P, T> fun, log4net.ILog log)
        {
            try
            {
                return fun(key);
            }
            catch (Exception ex)
            {
                log.ErrorInFunction(ex);
                return default(T);
            }
        }

        public static T GetModel<P, P2, T>(P key, P2 key2, Func<P, P2, T> fun, log4net.ILog log)
        {
            try
            {
                return fun(key, key2);
            }
            catch (Exception ex)
            {
                log.ErrorInFunction(ex);
                return default(T);
            }
        }
        public static T GetModel<T>(int key, Func<int, T> fun, log4net.ILog log)
        {
            if (key <= 0) return default(T);

            return GetModel<int, T>(key, fun, log);
        }

        public static T GetModel<T>(string key, Func<string, T> fun, log4net.ILog log)
        {
            if (string.IsNullOrWhiteSpace(key)) return default(T);
            else
                return GetModel<string, T>(key, fun, log);
        }

        public static T GetModel<T>(Guid key, Func<Guid, T> fun, log4net.ILog log)
        {
            if (key == Guid.Empty) return default(T);
            else
                return GetModel<Guid, T>(key, fun, log);
        }

        public static T GetModel<T>(Func<T> fun, log4net.ILog log)
        {
            try
            {
                return fun();
            }
            catch (Exception ex)
            {
                log.ErrorInFunction(ex);
                return default(T);
            }
        }

        public static bool ActionBool<P>(P parameter, Func<P, bool> fun, log4net.ILog log)
        {
            return GetModel<P, bool>(parameter, fun, log);
        }

        public static bool ActionVoid<P>(P key, Action<P> fun, log4net.ILog log)
        {
            try
            {
                fun(key);
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorInFunction(ex);
                return false;
            }
        }
    }
}
