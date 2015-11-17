using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class Flotr2Const
    {
        /// <summary>
        /// ログタイプ
        /// </summary>
        public enum ChartType
        {
            /// <summary>
            /// 円グラフ
            /// </summary>
            Pie,

            /// <summary>
            /// 棒グラフ
            /// </summary>
            Bar,

            /// <summary>
            /// バブルチャート
            /// </summary>
            Bubble,

            /// <summary>
            /// フォーメーション(バブルチャート)
            /// </summary>
            Formation,
        }

        /// <summary>
        /// 凡例ポジション
        /// </summary>
        public enum LegendPosition
        {
            /// <summary>
            /// 左上
            /// </summary>
            nw,

            /// <summary>
            /// 左中
            /// </summary>
            cw,

            /// <summary>
            /// 左下
            /// </summary>
            sw,

            /// <summary>
            /// 右上
            /// </summary>
            ne,

            /// <summary>
            /// 右中
            /// </summary>
            ce,

            /// <summary>
            /// 右下
            /// </summary>
            se
        }

        public static readonly string FormationSheredTemplete = "@FormationSheredTemplete";

        public static readonly string Home = "Home";

        public static readonly string Away = "Away";

        public static readonly string HomePlayerName = "HomePlayerName";

        public static readonly string AwayPlayerName = "AwayPlayerName";

        public static readonly string HomePlayerNo = "HomePlayerNo";

        public static readonly string AwayPlayerNo = "AwayPlayerNo";
    }
}