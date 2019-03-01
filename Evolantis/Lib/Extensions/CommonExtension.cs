using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolantis.Lib.Extensions
{
    public static class CommonExtension
    {
        public static string SetNewFilename(this string value)
        {
            string filename = "";
            if (!string.IsNullOrWhiteSpace(value))
            {
                DateTime baseDate = new DateTime(1970, 1, 1);
                TimeSpan diff = DateTime.Now - baseDate;
                filename = Path.GetFileNameWithoutExtension(value);
                string fileExt = Path.GetExtension(value);
                filename = diff.TotalMilliseconds.ToString().URLFriendly().Replace("-", "") + "_" + filename.URLFriendly() + fileExt;
            }
            return filename;
        }

        public static string GetFirstLetter(this string value)
        {
            if (value.Length > 0)
                return value.Substring(0, 1);

            return value;
        }

        public static string SafeName(this string value)
        {
            return value.URLFriendly().Replace('-', '_');
        }
    }
}
