using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Extend
{
    public static class DateTimeExtend
    {
        /// <summary>
        /// 月末日取得
        /// </summary>
        public static DateTime EndOfTheMonth(this DateTime target)
        {
            return new DateTime(target.Year, target.Month, DaysInMonth(target));
        }

        /// <summary>
        /// 月末日(+時間)取得
        /// </summary>
        public static DateTime EndOfTheMonthWithTime(this DateTime target)
        {
            return new DateTime(target.Year, target.Month, DaysInMonth(target),23,59,59);
        }

        /// <summary>
        /// 日数取得
        /// </summary>
        public static int DaysInMonth(this DateTime target)
        {
            return DateTime.DaysInMonth(target.Year, target.Month);
        }

        /// <summary>
        /// 差分年数取得
        /// </summary>
        public static int GetYearDiff(this DateTime target, DateTime compare)
        {
            var age = compare.Year - target.Year;

            if (compare.Month < target.Month ||
               (compare.Month == target.Month &&
               compare.Day < target.Day))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// 日付をintに変換(yyyymmdd形式の数値にする2015/3/3ならば20150303)
        /// </summary>
        public static int ParseToInt(this DateTime dt)
        {
            return dt.Year * 10000 + dt.Month * 100 + dt.Day; ;
        }

        public static bool IsToday(this DateTime target)
        {
            return (target.Date == DateTime.Today.Date);
        
        }


    }
}