using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Evolantis.Lib.Utility
{
    public class Utility
    {
        public static string BaseUrl()
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd();
        }

        public static string SafeString(object value)
        {
            if (!DBNull.Value.Equals(value))
            {
                if (!string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return value.ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public static int SafeInteger(object value)
        {
            if (!DBNull.Value.Equals(value))
            {
                if (value != null)
                { return Convert.ToInt32(value); }
                else
                { return 0; }
            }
            else
            { return 0; }
        }

        public static string FormatFileSize(long TotalBytes)
        {
            if (TotalBytes >= 1073741824) //Giga Bytes
            {
                decimal FileSize = decimal.Divide(TotalBytes, 1073741824);
                return string.Format("{0:##.##} GB", FileSize);
            }
            else if (TotalBytes >= 1048576 && TotalBytes < 1073741824) //Mega Bytes
            {
                decimal FileSize = decimal.Divide(TotalBytes, 1048576);
                return string.Format("{0:##.##} MB", FileSize);
            }
            else if (TotalBytes >= 1024 && TotalBytes < 1048576) //Kilo Bytes
            {
                decimal FileSize = decimal.Divide(TotalBytes, 1024);
                return string.Format("{0:##.##} KB", FileSize);
            }
            else if (TotalBytes > 0 & TotalBytes < 1024)
            {
                decimal FileSize = TotalBytes;
                return string.Format("{0:##.##} Bytes", FileSize);
            }
            else
            {
                return "0 Bytes";
            }
        }

        //-- Get client IP Address
        public static string GetClientIPAddress()
        {
            //The X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a 
            //client connecting to a web server through an HTTP proxy or load balancer
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            ip = ip.Replace("::ffff:", "");

            return ip;
        }

        static readonly string[] Columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        //-- Get alphabet letter from number
        public static string NumberToAlphabet(int index)
        {
            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");
            if (index > Columns.Length)
                throw new IndexOutOfRangeException("index is more than the lenght of alphabet");

            return Columns[index - 1];
        }

        public static string Alert(string type = "success", string template = "", bool isDismissible = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<div class='alert alert-{0}{1}' role='alert' >", type, (isDismissible ? " alert-dismissible fade show" : "")));
            if (isDismissible)
                sb.Append("<button type='button' class='close' data-dismiss='alert' aria-label='close'><span aria-hidden='true'>x</span></button>");

            sb.Append(template);
            sb.Append("</div>");

            return sb.ToString();
        }

        public static bool ColumnExists(IDataReader reader, string columnName)
        {

            return reader.GetSchemaTable()
                         .Rows
                         .OfType<DataRow>()
                         .Any(row => row["ColumnName"].ToString() == columnName);
        }

        public static string UrlSegment(int index)
        {
            string retval = string.Empty;
            try
            {
                IList<string> segments = HttpContext.Current.Request.Url.Segments;
                retval = segments[index].Replace("/", "");
            }
            catch (Exception)
            { retval = string.Empty; }

            return retval;
        }

        public static int UrlSegmentLength()
        {
            IList<string> segments = HttpContext.Current.Request.Url.Segments;
            return segments.Count();
        }
    }
}
