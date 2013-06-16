using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomExtension
{
    public static class EnumExtension
    {
        public static int GetEnumValue(this Enum source)
        {
            return (int)Enum.Parse(source.GetType(), source.ToString());
        }

        public static byte GetByteEnumValue(this Enum source)
        {
            return (byte)Enum.Parse(source.GetType(), source.ToString());
        }

        public static String GetByteEnumValueToString(this Enum source)
        {
            return source.GetByteEnumValue().ToString();
        }

        public static String GetEnumValueToString(this Enum source)
        {
            return source.GetEnumValue().ToString();
        }

        public static string GetEnumDescription(this Enum source)
        {
            FieldInfo field = source.GetType().GetField(source.ToString());

            DisplayAttribute attrs = (DisplayAttribute)field.
                          GetCustomAttributes(typeof(DisplayAttribute), false).First();

            return attrs.GetName();
        }

        public static Boolean IsDefinedAttribute<T>(this Enum source) where T : Attribute
        {
            Type enumType = source.GetType();
            if (!enumType.IsEnum)
            {
                return false;
            }

            var name = Enum.GetName(enumType, Convert.ToInt32(source));
            Object[] objs = enumType.GetField(name).GetCustomAttributes(typeof(T), false);
            return objs.Length > 0;
        }

        
    }
}
