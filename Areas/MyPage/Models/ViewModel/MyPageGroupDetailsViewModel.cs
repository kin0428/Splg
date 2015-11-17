#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Areas.MyPage.Models
 * Class		: MyPageTopViewModel
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using Splg.Models;
using Splg.Models.News.ViewModel;
using Splg.Models.Game.ViewModel;
using Splg.Areas.Npb.Models;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.MyPage.Models.InfoModel;
#endregion

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageGroupDetailsViewModel
    {
        /// <summary>
        /// 初期表示時の表示件数
        /// </summary>
        public const int INITIAL_SIZE = 0;

        /// <summary>
        /// １ページあたりの件数初期値
        /// </summary>
        public const int INITIAL_PAGE_SIZE = 10;


        public MyPageGroupModel GroupInfo { get; set; }

        #region 年月用変数
        public string ThisYear { get; set; } // 今月
        public string ThisMonth { get; set; } // 今月
        public string[] MonthListClassRanking = new string[12]; //　アクティブ月のとき'class="active"'、そうでないとき''
        public string[] MonthListClassCorrect = new string[12]; //　アクティブ月のとき'class="active"'、そうでないとき''
        #endregion

        // ポイントランキング表示用

        public const int RANKING_TOP_NUM = 6;

        public IEnumerable<MyPageGroupMemberModel> MemberRanking { get; set; }


        /// <summary>
        /// この試合の投稿記事
        /// </summary>
        public IEnumerable<PostedInfoViewModel> PostedInfos { get; set; }

        public class ScoreGameInfo
        {
            public int GameID { get; set; }
            public int TeamID { get; set; }
            public int? R { get; set; }
            public int? Inning { get; set; }
            public int? TB { get; set; }
        }

        /// <summary>
        /// Use to get db with many conditions.
        /// </summary>
        public class GameInfoSSByCondidtion
        {
            public GameInfoSS GameInfoSSNpb { get; set; }
            public int GameTypeID { get; set; }
            public string GameTypeName { get; set; }
        }

        public IEnumerable<TeamRankingDeviation> ListTeamExpectationsDeviation { get; set; }
        public IEnumerable<TeamRankingDeviation> ListTeamBetrayalDeviation { get; set; }
        public IEnumerable<PostedInfoViewModel> NpbPostedList { get; set; }

        public List<MyPageGameInfoViewModel> ListGames { get; set; }
        public class MyPageGameInfoViewModel
        {
            public string SortKey { get; set; }
            public int SportsID { get; set; }
            public string GameTypeName { get; set; }
            public int statusMatch { get; set; }
            public GameInfoViewModel npbGameInfo { get; set; }
            public GameInfoViewModel mlbGameInfo { get; set; }
            public JlgGameInfos jlgGameInfo { get; set; }
            public GameOddsInfo gOdds { get; set; }


            public FollowMemberList FollowMembers;

        }

    }

    //フォローユーザーの予想
    public class FollowMemberList
    {
        /// <summary>
        /// //ホームの勝ち
        /// </summary>
        public List<Member> FollowMembersBetToWin { get; set; }
        /// <summary>
        /// 引き分け
        /// </summary>
        public List<Member> FollowMembersBetToLose { get; set; }
        /// <summary>
        /// ビジターの勝ち
        /// </summary>
        public List<Member> FollowMembersBetToDraw { get; set; }
    }

}