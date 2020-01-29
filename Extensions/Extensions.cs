//  ***************************************
//                                                   
//                     CsvReader
//                                                   
//  ***************************************
//
//  Copyright (c) 2020 All Rights Reserved

namespace CsvReader.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="Extensions" />
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// The Humanize
        /// </summary>
        /// <param name="value">The value<see cref="int"/></param>
        /// <param name="condition">The condition<see cref="bool"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int Humanize(this int value, bool condition)
        {
            if (condition)
                return value - 1;
            return value;
        }

        /// <summary>
        /// The SuperTrim
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string SuperTrim(this string value)
        {
            return value.Trim().Replace("\"", "").TrimEnd().TrimStart();
        }

        /// <summary>
        /// The ToNameFile
        /// </summary>
        /// <param name="value">The value<see cref="DateTime"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string ToNameFile(this DateTime value)
        {
            return value.ToString().Replace(" ", "").Replace(":", "").Replace("/", "");
        }

        /// <summary>
        /// The ToCapitalize
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        /// <param name="condition">The condition<see cref="bool"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string ToCapitalize(this string value, bool condition)
        {
            if (condition)
                return value.ToUpper();
            return value;
        }

        /// <summary>
        /// The Pow
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="double?"/></returns>
        public static double? Pow(this string value)
        {
            if (value.Contains("E"))
            {
                string indexOf = "E";
                if (value.Contains("+"))
                {
                    indexOf = "+";
                }
                else if (value.Contains("-"))
                {
                    indexOf = "-";
                }

                // exposant
                string expStr = value.Substring(value.IndexOf(indexOf) + 1, (value.Length - (value.IndexOf(indexOf) + 1)));
                if (indexOf.Equals("-"))
                    expStr = string.Format("{0}{1}", indexOf, expStr);
                int exp = Convert.ToInt32(expStr);
                double val = Convert.ToDouble(value.Substring(0, value.IndexOf('E')));
                return val * Math.Pow(10, exp);
            }
            return Convert.ToDouble(value);
        }
    }
}
