using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class UserArticleConst
    {
        /// <summary>
        /// タイトルプレースホルダー文章
        /// </summary>
        public static readonly string TitlePlaceHolderMessage = "タイトルを入力してください（50字以内）";

        /// <summary>
        /// 本文プレースホルダー文書
        /// </summary>
        public static readonly string BodyPlaceHolderMessage = "本文を入力してください（5000字以内）";

        /// <summary>
        /// １・・スポーツ、２・・チーム、３・・選手、４・・試合、5・・リーグ 6 予想分析
        /// </summary>
        public enum ClassificationTypes
        {
            /// <summary>
            /// SPORT
            /// </summary>
            SPORT = 1,

            /// <summary>
            /// TEAM
            /// </summary>
            TEAM = 2,

            /// <summary>
            /// PLAYER
            /// </summary>
            PLAYER = 3,

            /// <summary>
            /// GAME
            /// </summary>
            GAME = 4,

            /// <summary>
            /// LEAGUE
            /// </summary>
            LEAGUE = 5,

            /// <summary>
            /// EXPECTATION
            /// </summary>
            EXPECTATION = 6
        }
    }
}