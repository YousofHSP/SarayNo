﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class StringExtensions
    {
        public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }
        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }
        public static decimal ToDecimal(this string value)
        {
            return Convert.ToDecimal(value);
        }
        public static string ToNumeric(this int value)
        {
            return value.ToString("N0"); // "123,456"
        }
        public static string ToNumeric(this float value)
        {
            return value.ToString("N0");
        }
        public static string ToNumeric(this decimal value)
        {
            return value.ToString("N0");
        }
        public static string ToCurrency(this int value)
        {
            return value.ToString("C0");
        }
        public static string ToCurrency(this decimal value)
        {
            return value.ToString("C0");
        }
        public static string En2Fa(this string str)
        {
            return str.Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }
        public static string Fa2En(this string str)
        {
            return str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9");
        }
        public static string FixPersianChars(this string str)
        {
            return str;
        }
        public static string CleanString(this string str)
        {
            return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
        }
        public static string NullIfEmpty(this string str)
        {
            return str?.Length == 0 ? null : str;
        }
        public static string ToShamsi(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            int year = persianCalendar.GetYear(dateTime);
            int month = persianCalendar.GetMonth(dateTime);
            int day = persianCalendar.GetDayOfMonth(dateTime);
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;

            return $"{hour.ToString("00")}:{minute.ToString("00")} {year}/{month.ToString("00")}/{day.ToString("00")}";


        }
        public static string ToShamsi(this DateTimeOffset dateTimeOffset)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            var dateTime = dateTimeOffset.LocalDateTime;
            int year = persianCalendar.GetYear(dateTime);
            int month = persianCalendar.GetMonth(dateTime);
            int day = persianCalendar.GetDayOfMonth(dateTime);
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;

            return $"{hour.ToString("00")}:{minute.ToString("00")} {year}/{month.ToString("00")}/{day.ToString("00")}";


        }
        public static DateTime ToGregorian (this string shamsiString)
        {
            
                if (shamsiString == "")
                    return new DateTime();
                shamsiString = shamsiString.Fa2En();
                var parts = shamsiString.Split(' ');
                var timeParts = parts[0].Split(':');
                var dateParts = parts[1].Split('/');
                
                // Convert to integer
                var year = int.Parse(dateParts[0]);
                var month = int.Parse(dateParts[1]);
                var day = int.Parse(dateParts[2]);
                var hour = int.Parse(timeParts[0]);
                var minute = int.Parse(timeParts[1]);
                
                // Use PersianCalendar to convert
                var persianCalendar = new PersianCalendar();
                return persianCalendar.ToDateTime(year, month, day, hour, minute, 0, 0);
        }
    }
}
