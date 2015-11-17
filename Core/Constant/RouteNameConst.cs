using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class RouteNameConst
    {
        /// <summary>
        /// マイページ-予想結果Route名(3-10)
        /// </summary>
        public static readonly string MyPageToday = "MyPage_3-10";
        
        /// <summary>
        /// 他ユーザーページRoute名(4-1)
        /// </summary>
        public static readonly string UserTopIndex = "User_4-1";

        /// <summary>
        /// 他ユーザー対象年月取得用Route名(4-1-1)
        /// </summary>
        public static readonly string UserTopChangeYearMonth = "User_4-1-1";



        /// <summary>
        /// 投稿記事（Key:スポーツId）
        /// </summary>
        public static readonly string UserArticleBySportsId = "5_2_by_sport";

        /// <summary>
        /// 投稿記事（Key:投稿Id）
        /// </summary>
        public static readonly string UserArticleByContributeId = "5_3";

        /// <summary>
        /// 懸賞Top
        /// </summary>
        public static readonly string PrizeTop = "Prize_6-1";

        /// <summary>
        /// 懸賞詳細
        /// </summary>
        public static readonly string PrizeDetal = "Prize_6-2";

        /// <summary>
        /// ニュース詳細
        /// </summary>
        public static readonly string NewsDetail = "7_1";

        public static readonly string NpbGameDetail = "Npb_8-6";

        public static readonly string MlbGameDetail = "Mbl_9-7";



        public static readonly string JlgGameDetail = "Jlg_10_5";

        /// <summary>
        /// Jlgニュースホーム
        /// </summary>
        public static readonly string JlgNewsHome = "Jleague_10_6";



    }
}