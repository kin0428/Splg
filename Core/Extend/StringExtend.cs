using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Constant;

namespace Splg.Core.Extend
{
    public static class StringExtend
    {

        public static string Enclosed(this string target, string enclosedFormat)
        {
            return enclosedFormat.Replace("{0}", target);
        }

        /// <summary>
        /// カンマ区切り済文字列
        /// </summary>
        public static string CommaSeparated(this List<string> target)
        {
            return string.Join(",", target);
        }

        /// <summary>
        /// シングルクォート囲い
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<string> singleQuoteEnclosed(this List<string> target)
        {
            var result = new List<string>();

            foreach (var item in target)
            {
                result.Add(item.Enclosed(ExtendConst.SingleQuoteEnclosedFormat));
            }

            return result;
        }


    }
}