using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class JlgConst
    {
        /// <summary>
        /// Jリーグタイプ
        /// </summary>
        public enum JType
        {
            Empty = 0,

            /// <summary>
            /// J1
            /// </summary>
            J1 = 1,

            /// <summary>
            /// J2
            /// </summary>
            J2 = 2,

            /// <summary>
            /// ナビスコ
            /// </summary>
            Jleaguecup = 3
        }

        /// <summary>
        /// 記事種類
        /// </summary>
        public enum ArticleKind
        {
            /// <summary>
            /// ニュース
            /// </summary>
            News = 0,

            /// <summary>
            /// 投稿記事
            /// </summary>
            UserPosts = 1
        }

        /// <summary>
        /// イエローカードのCssClass名
        /// </summary>
        public static string CssClassYellowCard = "socStarsIcon-cardYel";
        /// <summary>
        /// レッドカードのCssClass名
        /// </summary>
        public static string CssClassRedCard = "socStarsIcon-cardRed";
        /// <summary>
        /// イエローカード×２（イエロー＋レッド）のCssClass名
        /// </summary>
        public static string CssClassStackedRedCard = "socStarsIcon-cardRedYel";

    }
}