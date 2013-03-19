using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Web;


namespace CustomExtension
{
    public static class StringExtension
    {
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://(([\w-]+\.)+[\w-]+)(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”' };

        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        public static string GetUrlDomain(this string url)
        {
            if (WebUrlExpression.IsMatch(url))
            {
                Match m = WebUrlExpression.Match(url);
                return (m.Groups[2].Value);
            }
            return null;
        }

        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, target, args);
        }

        public static string Hash(this string target)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        public static string WrapAt(this string target, int index)
        {
            const int DotCount = 3;

            return (target.Length < index) ? target : string.Concat(target.Substring(0, index), new string('.', DotCount));
        }

        public static string StripHtml(this string target)
        {
            return StripHTMLExpression.Replace(target, string.Empty);
        }

        public static Guid ToGuid(this string target)
        {
            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                byte[] base64 = Convert.FromBase64String(encoded);

                return new Guid(base64);
            }

            return Guid.Empty;
        }

        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(CultureInfo.CurrentCulture), string.Empty);
                }
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }


        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target).Replace("+", "%20");
        }

        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }

        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        public static string CleanQuoteTag(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            RegexOptions options = RegexOptions.IgnoreCase;

            for (int i = 0; i < 20; i++)
                text = Regex.Replace(text, @"\[quote(?:\s*)user=""((.|\n)*?)""\]((.|\n)*?)\[/quote(\s*)\]", "", options);
            for (int i = 0; i < 20; i++)
                text = Regex.Replace(text, @"\[quote\]([^>]+?|.+?)\[\/quote\]", "", options);
            return text;
        }



        public static Double StringToDouble(this String source)
        {
            Double value = 0;
            Double.TryParse(source, out value);
            return value;
        }

        public static Decimal StringToDecimal(this String source)
        {
            Decimal value = 0;
            Decimal.TryParse(source, out value);
            return value;
        }

        /// <summary>
        /// 测试一个字符串文本是否为Decimal类型
        /// </summary>
        /// <param name="text">测试文本</param>
        /// <returns>是Double类型返回Ture,反之False</returns>
        public static Boolean IsDecimal(this String source)
        {
            Decimal result = Decimal.Zero;
            return Decimal.TryParse(source, out result);
        }

        public static int StringToInt(this String source)
        {
            int value = 0;
            int.TryParse(source, out value);
            return value;
        }

        public static DateTime StringToDate(this String source)
        {
            DateTime value = DateTime.MinValue;
            DateTime.TryParse(source, out value);
            return value;
        }


        public static T StringToEnum<T>(this String source, T defaultValue) where T : struct
        {
            T value;
            if (!Enum.TryParse<T>(source, out value))
                return defaultValue;
            return value;
        }
        /// <summary>
        /// 将url中带有中文的参数进行GB编码
        /// </summary>
        /// <param name="source">Url地址</param>
        /// <returns></returns>
        public static String UrlEncodePart(this String source)
        {
            if (String.IsNullOrEmpty(source))
                return "";
            String body = source.GetUrlParameterValue("body");
            String royalty_parameters = source.GetUrlParameterValue("royalty_parameters");
            String subject = source.GetUrlParameterValue("subject");

            if (!String.IsNullOrEmpty(body))
                source = source.Replace(body, HttpUtility.UrlEncode(body, Encoding.GetEncoding("GB2312")));
            if (!String.IsNullOrEmpty(royalty_parameters))
                source = source.Replace(royalty_parameters, HttpUtility.UrlEncode(royalty_parameters, Encoding.GetEncoding("GB2312")));
            if (!String.IsNullOrEmpty(subject))
                source = source.Replace(subject, HttpUtility.UrlEncode(subject, Encoding.GetEncoding("GB2312")));
            return source;
        }
        /// <summary>
        /// 获取Url中Get参数值
        /// </summary>
        /// <param name="source">Url链接</param>
        /// <param name="parameterName">Get参数名</param>
        /// <returns>返回值</returns>
        public static String GetUrlParameterValue(this String source, String parameterName)
        {
            Regex urlRegex = new Regex(String.Format(@"(?:^|/?|&){0}=([^&]*)(?:&|$)", parameterName));
            Match m = urlRegex.Match(source);
            string pagenum = string.Empty;
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return "";
        }

        public static IList<String> ToList(this String source, Char split)
        {
            IList<String> list = new List<String>();
            foreach (var item in source.Split(split))
                list.Add(item);
            return list;
        }

        public static String Substring(this String source, int length, String rightString)
        {
            if (String.IsNullOrEmpty(source) || source.Length <= length)
            {
                return source;
            }
            return String.Format("{0}{1}", source.Substring(0, length), rightString);
        }

        public static bool GuidTryParse(this string s, out Guid result)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            try
            {
                if ((!string.IsNullOrEmpty(s)) && (s.Trim().Length == 22))
                {
                    string encoded = string.Concat(s.Trim().Replace("-", "+").Replace("_", "/"), "==");

                    byte[] base64 = Convert.FromBase64String(encoded);

                    result = new Guid(base64);
                    return true;
                }

                result = new Guid(s);
                return true;
            }
            catch (FormatException)
            {
                result = Guid.Empty;
                return false;
            }
            catch (OverflowException)
            {
                result = Guid.Empty;
                return false;
            }
        }
        public static String LimitByteLength(this String input, Int32 maxLength)
        {
            for (Int32 i = input.Length - 1; i >= 0; i--)
            {
                if (Encoding.Default.GetByteCount(input.Substring(0, i + 1)) <= maxLength)
                {
                    return input.Substring(0, i + 1);
                }
            }

            return String.Empty;
        }

        public static String EncodeBase64(this String input)
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
            }
            catch
            {
                return String.Empty;
            }
        }

        public static String UnCodeBase64(this String input)
        {
            try
            {
                byte[] utf8Bytes = System.Convert.FromBase64String(input);
                return Encoding.UTF8.GetString(utf8Bytes);
            }
            catch
            {
                return String.Empty;
            }
        }



        public static string Md5(this String source)
        {
            try
            {
                byte[] hashvalue = (new MD5CryptoServiceProvider()).ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(source));
                string result = BitConverter.ToString(hashvalue);
                result = result.Replace("-", "");
                return result;
            }
            catch
            {
                return String.Empty;
            }

        }

    }
}
