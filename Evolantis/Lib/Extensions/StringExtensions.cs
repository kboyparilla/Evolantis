using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Evolantis.Lib.Extensions
{
    public static class StringExtensions
    {
        public static string Append(this string source, string stringToAppend, string separator = " ", bool condition = true)
        {
            if (!condition)
                return source;
            if (string.IsNullOrWhiteSpace(StringExtensions.StripHtml(source, false).HtmlDecode()))
                return stringToAppend;
            if (string.IsNullOrWhiteSpace(StringExtensions.StripHtml(stringToAppend, false).HtmlDecode()))
                return source;
            return source + separator + stringToAppend;
        }

        public static string SafeFileName(this string s)
        {
            foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
                s = s.Replace(invalidFileNameChar.ToString(), string.Empty);
                s = s.Replace("'", string.Empty);
                s = s.Replace("\"", string.Empty);
                s = s.Replace("#", string.Empty);
                s = s.Replace("%", string.Empty);
                s = s.Replace("/", "or");
                s = s.Replace("&", "and");
            return s;
        }

        private static string[] SplitUpperCase(string source)
        {
            if (source == null)
                return new string[0];
            if (source.Length == 0)
                return new string[1] { "" };
            StringCollection stringCollection = new StringCollection();
            int startIndex = 0;
            char[] charArray = source.ToCharArray();
            for (int index = 1; index < charArray.Length; ++index)
            {
                if (char.IsUpper(charArray[index]))
                {
                    stringCollection.Add(new string(charArray, startIndex, index - startIndex));
                    startIndex = index;
                }
            }
            stringCollection.Add(new string(charArray, startIndex, charArray.Length - startIndex));
            string[] array = new string[stringCollection.Count];
            stringCollection.CopyTo(array, 0);
            return array;
        }

        public static bool isEmail(this string input)
        {
            return Regex.Match(input, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.IgnoreCase).Success;
        }

        public static bool isPhone(this string input)
        {
            return Regex.Match(input, "^\\+?(\\d[\\d-. ]+)?(\\([\\d-. ]+\\))?[\\d-. ]+\\d$", RegexOptions.IgnoreCase).Success;
        }

        public static bool isNumber(this string input)
        {
            double result;
            return double.TryParse(input, out result);
        }

        public static bool isCurrency(this string input)
        {
            double result;
            return double.TryParse(input, NumberStyles.Currency, (IFormatProvider)CultureInfo.CurrentCulture, out result);
        }

        public static string StripHtml(this string html, bool allowHarmlessTags = false)
        {
            if (html == null || html == string.Empty)
                return string.Empty;
            if (allowHarmlessTags)
                return Regex.Replace(html, "", string.Empty);
            return Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        public static object SafeDatabaseDate(this string dt)
        {
            if (dt.IsDate())
                return (object)dt;
            return (object)DBNull.Value;
        }

        public static bool IsDate(this string date)
        {
            DateTime result;
            return DateTime.TryParse(date, out result);
        }

        public static string URLFriendly(this string title)
        {
            if (title == null)
                return "";
            int length1 = title.Length;
            bool flag = false;
            StringBuilder stringBuilder = new StringBuilder(length1);
            for (int index = 0; index < length1; ++index)
            {
                char c = title[index];
                if ((int)c >= 97 && (int)c <= 122 || (int)c >= 48 && (int)c <= 57)
                {
                    stringBuilder.Append(c);
                    flag = false;
                }
                else if ((int)c >= 65 && (int)c <= 90)
                {
                    stringBuilder.Append((char)((uint)c | 32U));
                    flag = false;
                }
                else if ((int)c == 32 || (int)c == 44 || ((int)c == 46 || (int)c == 47) || ((int)c == 92 || (int)c == 45 || (int)c == 95) || (int)c == 61)
                {
                    if (!flag && stringBuilder.Length > 0)
                    {
                        stringBuilder.Append('-');
                        flag = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int length2 = stringBuilder.Length;
                    stringBuilder.Append(StringExtensions.RemapInternationalCharToAscii(c));
                    if (length2 != stringBuilder.Length)
                        flag = false;
                }
                if (index == 80)
                    break;
            }
            if (flag)
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            return stringBuilder.ToString();
        }

        private static string RemapInternationalCharToAscii(char c)
        {
            string lowerInvariant = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(lowerInvariant))
                return "a";
            if ("èéêëę".Contains(lowerInvariant))
                return "e";
            if ("ìíîïı".Contains(lowerInvariant))
                return "i";
            if ("òóôõöøőð".Contains(lowerInvariant))
                return "o";
            if ("ùúûüŭů".Contains(lowerInvariant))
                return "u";
            if ("çćčĉ".Contains(lowerInvariant))
                return nameof(c);
            if ("żźž".Contains(lowerInvariant))
                return "z";
            if ("śşšŝ".Contains(lowerInvariant))
                return "s";
            if ("ñń".Contains(lowerInvariant))
                return "n";
            if ("ýÿ".Contains(lowerInvariant))
                return "y";
            if ("ğĝ".Contains(lowerInvariant))
                return "g";
            switch (c)
            {
                case 'Þ':
                    return "th";
                case 'ß':
                    return "ss";
                case 'đ':
                    return "d";
                case 'ĥ':
                    return "h";
                case 'ĵ':
                    return "j";
                case 'ł':
                    return "l";
                case 'ř':
                    return "r";
                default:
                    return "";
            }
        }

        public static string CompressString(this string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Compress, true))
                gzipStream.Write(bytes, 0, bytes.Length);
            memoryStream.Position = 0L;
            byte[] buffer = new byte[memoryStream.Length];
            memoryStream.Read(buffer, 0, buffer.Length);
            byte[] inArray = new byte[buffer.Length + 4];
            Buffer.BlockCopy((Array)buffer, 0, (Array)inArray, 4, buffer.Length);
            Buffer.BlockCopy((Array)BitConverter.GetBytes(bytes.Length), 0, (Array)inArray, 0, 4);
            return Convert.ToBase64String(inArray);
        }

        public static string DecompressString(this string compressedText)
        {
            if (compressedText == string.Empty)
                return string.Empty;
            byte[] buffer = Convert.FromBase64String(compressedText);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int int32 = BitConverter.ToInt32(buffer, 0);
                memoryStream.Write(buffer, 4, buffer.Length - 4);
                byte[] numArray = new byte[int32];
                memoryStream.Position = 0L;
                using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Decompress))
                    gzipStream.Read(numArray, 0, numArray.Length);
                return Encoding.UTF8.GetString(numArray);
            }
        }

        public static string HtmlEncode(this string data)
        {
            return HttpUtility.HtmlEncode(data);
        }

        public static string HtmlDecode(this string data)
        {
            return HttpUtility.HtmlDecode(data);
        }

        public static NameValueCollection ParseQueryString(this string query)
        {
            return HttpUtility.ParseQueryString(query);
        }

        public static string UrlEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        public static string UrlDecode(this string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        public static string UrlPathEncode(this string url)
        {
            return HttpUtility.UrlPathEncode(url);
        }

        public static MemoryStream ToStream(this string Source)
        {
            return new MemoryStream(Encoding.ASCII.GetBytes(Source));
        }

        public static string SplitUpperCaseToString(this string source)
        {
            return string.Join(" ", StringExtensions.SplitUpperCase(source));
        }

        public static bool ToBoolean(this string value)
        {
            switch (value.ToLower().Trim())
            {
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                case "":
                    return false;
                default:
                    return true;
            }
        }

        public static int ToInteger(this bool value)
        {
            int val = 0;
            if (value)
                val = 1;
            return val;
        }
    }
}
