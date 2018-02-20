using System;
using System.Security.Cryptography;
using System.Text;

namespace GTA.PI.API.Utility
{
    /// <summary>
    /// 一般通用类
    /// </summary>
    public class GeneralHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordFormat"></param>
        /// <returns></returns>
        public static string HashPasswordForStoringInConfigFile(string password, string passwordFormat)
        {
            HashAlgorithm algorithm;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            if (passwordFormat == null)
            {
                throw new ArgumentNullException("passwordFormat");
            }
            if (EqualsIgnoreCase(passwordFormat, "sha1"))
            {
                algorithm = new SHA1Cng();
            }
            else if (EqualsIgnoreCase(passwordFormat, "md5"))
            {
                algorithm = new MD5Cng();
            }
            else
            {
                object[] args = new object[] { "passwordFormat" };
                throw new ArgumentException("InvalidArgumentValue");
            }
            using (algorithm)
            {
                return BinaryToHex(algorithm.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        /// <summary>
        /// 大小写不敏感处理
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
            {
                return true;
            }
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return false;
            }
            if (s2.Length != s1.Length)
            {
                return false;
            }
            return (string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        /// Binary到Hex转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string BinaryToHex(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            char[] chArray = new char[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                byte num2 = data[i];
                chArray[2 * i] = NibbleToHex((byte)(num2 >> 4));
                chArray[(2 * i) + 1] = NibbleToHex((byte)(num2 & 15));
            }
            return new string(chArray);
        }

        /// <summary>
        /// Nibble到Hex转换
        /// </summary>
        /// <param name="nibble"></param>
        /// <returns></returns>
        public static char NibbleToHex(byte nibble)
        {
            return ((nibble < 10) ? ((char)(nibble + 0x30)) : ((char)((nibble - 10) + 0x41)));
        }
    }
}