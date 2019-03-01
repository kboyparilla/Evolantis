using System;
using System.ComponentModel;

namespace Evolantis.Lib.Extensions
{
    public static class ConvertionStringExtenstion
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static int AsInt(this string value)
        {
            return value.As<int>();
        }

        public static long AsLong(this string value)
        {
            return value.As<long>();
        }

        public static int AsInt(this string value, int defaultValue)
        {
            return value.As<int>(defaultValue);
        }

        public static Decimal AsDecimal(this string value)
        {
            return value.As<Decimal>();
        }

        public static Decimal AsDecimal(this string value, Decimal defaultValue)
        {
            return value.As<Decimal>(defaultValue);
        }

        public static float AsFloat(this string value)
        {
            return value.As<float>();
        }

        public static float AsFloat(this string value, float defaultValue)
        {
            return value.As<float>(defaultValue);
        }

        public static DateTime AsDateTime(this string value)
        {
            return value.As<DateTime>();
        }

        public static DateTime AsDateTime(this string value, DateTime defaultValue)
        {
            return value.As<DateTime>(defaultValue);
        }

        public static TValue As<TValue>(this string value)
        {
            return value.As<TValue>(default(TValue));
        }

        public static bool AsBool(this string value)
        {
            return value.As<bool>(false);
        }

        public static bool AsBool(this string value, bool defaultValue)
        {
            return value.As<bool>(defaultValue);
        }

        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                TypeConverter converter1 = TypeDescriptor.GetConverter(typeof(TValue));
                if (converter1.CanConvertFrom(typeof(string)))
                    return (TValue)converter1.ConvertFrom((object)value);
                TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(string));
                if (converter2.CanConvertTo(typeof(TValue)))
                    return (TValue)converter2.ConvertTo((object)value, typeof(TValue));
            }
            catch (Exception)
            {
                throw;
            }
            return defaultValue;
        }

        public static bool IsBool(this string value)
        {
            return value.Is<bool>();
        }

        public static bool IsInt(this string value)
        {
            return value.Is<int>();
        }

        public static bool IsDecimal(this string value)
        {
            return value.Is<Decimal>();
        }

        public static bool IsFloat(this string value)
        {
            return value.Is<float>();
        }

        public static bool IsDateTime(this string value)
        {
            return value.Is<DateTime>();
        }

        public static bool Is<TValue>(this string value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
                return converter.IsValid((object)value);
            return false;
        }

        public static string ToEmpty(this string value)
        {          
            if (!string.IsNullOrWhiteSpace(value))
                return value;              
            return "";         
        }

        public static string ToEmpty(this string value, string text)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            return text;
        }

        public static string ToZero(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            return "0";
        }
    }
}