using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CustomExtension.Helper
{
    public class EncryptHelper
    {
        public static string DecryptString(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(key));
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] inputBuffer = Convert.FromBase64String(strText);
                return Encoding.ASCII.GetString(provider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch (Exception ex)
            {
                //Log.Exception(string.Format("解密字符串错误：\n\r文本：{0}\r\n密钥：{1}\r\n", strText, key), ex);
                return string.Empty;
            }
        }

        public static string DecryptUTF8String(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] inputBuffer = Convert.FromBase64String(strText);
                return Encoding.UTF8.GetString(provider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch (Exception ex)
            {
                //Log.Exception("解密字符串错误：" + key, ex);
                return string.Empty;
            }
        }

        public static string EncryptString(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(key));
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] bytes = Encoding.ASCII.GetBytes(strText);
                string str = Convert.ToBase64String(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
                provider = null;
                return str;
            }
            catch (Exception ex)
            {
                //Log.Exception("加密字符串错误：" + key, ex);
                return string.Empty;
            }
        }

        public static string EncryptUTF8String(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] bytes = Encoding.UTF8.GetBytes(strText);
                string str = Convert.ToBase64String(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
                provider = null;
                return str;
            }
            catch (Exception ex)
            {
                //Log.Exception("加密字符串错误：" + key, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 创建加密种子
        /// </summary>
        /// <returns></returns>
        public static string EncryptGenerateSalt()
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }


        /// <summary>
        ///  url加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string UrlEncode(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            return HttpUtility.UrlEncode(source, Encoding.GetEncoding("GB2312"));
        }




    }
}
