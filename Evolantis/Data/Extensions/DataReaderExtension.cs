using System;
using System.Data;

namespace Evolantis.Data
{
    public static class DataReaderExtension
    {
        public static string GetValue(this IDataReader rdr, string columnName)
        {
            return rdr.GetValue<string>(columnName) ?? string.Empty;
        }

        public static T GetValue<T>(this IDataReader rdr, string columnName)
        {
            return rdr.GetValue<T>(columnName, default(T));
        }

        public static T GetValue<T>(this IDataReader rdr, string columnName, T defaultValue)
        {
            object obj = rdr.GetValue(rdr.GetOrdinal(columnName));
            if (obj == DBNull.Value)
                return defaultValue;
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
