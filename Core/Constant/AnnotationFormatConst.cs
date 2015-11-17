using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class AnnotationFormatConst
    {
        /// <summary>
        /// カンマ区切り
        /// </summary>
        public const string IsCommaSeparated = "{0:#,##0}";

        /// <summary>
        /// 西暦（スラッシュ区切り）Int用
        /// </summary>
        public const string IsAdWithSlashSeparatorForInt = "{0:####/##/##}";

        /// <summary>
        /// 西暦（日本語区切り）
        /// </summary>
        public const string IsAdWithJapaneseSeparator = "{0:yyyy年MM月dd日}";

        /// <summary>
        /// 西暦（スラッシュ区切り）
        /// </summary>
        public const string IsAdWithSlashSeparator = "{0:yyyy/MM/dd}";

        /// <summary>
        /// 西暦（ハイフン区切り）
        /// </summary>
        public const string IsAdWithHyphenSeparator = "{0:yyyy-MM-dd}";

        /// <summary>
        /// 日付＋(曜日)(日本語区切り)
        /// </summary>
        public const string IsDateAndDayOfWeekWithJapaneseSeparator = "{0:M月d日(ddd)}";

        /// <summary>
        /// 日付＋(曜日)(スラッシュ区切り)
        /// </summary>
        public const string IsDateAndDayOfWeekWithSlashSeparator = "{0:M/d(ddd)}";

        /// <summary>
        /// 日付＋(曜日)＋時間：分(スラッシュ区切り)
        /// </summary>
        public const string IsDateAndDayOfWeekPlusTimeWithSlashSeparator = "{0:M/d(ddd) hh:mm}";

    }
}