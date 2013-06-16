using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExtension.Helper
{
    public class EnumHelper
    {
        public static IEnumerable<T> GetList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();

        }
        /// <summary>
        /// 用于枚举值前台显示，不限对应的实际值
        /// </summary>
        public static byte NotLimited = 255;
    }
}
