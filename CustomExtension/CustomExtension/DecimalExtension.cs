using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExtension
{
    public static class DecimalExtension
    {
        public static string ToString(this decimal source)
        {
            return source.ToString("f2");
        }

    }
}
