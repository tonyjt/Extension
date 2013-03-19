using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomExtension
{
    public static class IEnumerableExtension
    {
        public static String ToString(this IList<String> source, Char split)
        {
            return source.ToString(split.ToString());
        }

        public static String ToString(this IList<String> source, String split)
        {
            return source.ToList<Object>().ToString(split);
        }

        public static String ToString(this IList<Object> source, Char split)
        {
            return source.ToString(split.ToString());
        }

        public static String ToString(this IList<Object> source, String split)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in source)
                sb.AppendFormat("{0}{1}", item, split);
            if (sb.Length > 0)
                return sb.ToString().Substring(0, sb.Length - split.Length);
            return "";
        }

        public static void Add<T>(this IList<T> source, IList<T> append)
        {
            if (null == source)
                throw new ArgumentException("The Source Is Null!");
            foreach (var item in append)
                source.Add(item);
        }
    }
}
