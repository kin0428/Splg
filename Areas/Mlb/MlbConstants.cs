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
 * Namespace	: Splg.Areas.Mlb
 * Class		: MlbConstants
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directives
using System.Web.Mvc;
#endregion

namespace Splg.Areas.Mlb
{
    public class MlbConstants
    {
        public const string MLB_TOP_INDEXTOP_INDEX = "9-1";  //MLBTOP
        public const string MLB_SCHEDULE_RESULT = "9-2";  //日程・結果（MLB）
        public const string MLB_ORDER_INDEX = "9-3";     //順位表（MLB）
        public const string MLB_TEAMINFORMATION = "9-5"; //チーム情報（MLB）
        public const string MLB_TEAMINFO_TEAMTOP = "9-5-1";       //チーム情報　チームトップ（MLB）
        public const string MLB_TEAMINFO_DAILYRESULT = "9-5-2";   //チーム情報　日程・結果（MLB）
        public const string MLB_TEAMINFO_CONFRONTATIONRESULT = "9-5-3";   //チーム情報　対戦成績（MLB）
        public const string MLB_TEAMINFO_NEWS = "9-5-6";  //チーム情報　ニュース（MLB）
        public const string MLB_GAME_INFORMATION = "9-7"; //試合情報（MLB）／試合前・試合後
        public const string MLB_NEWS_LIST = "9-8";        //ニュースリスト（MLB）


        public enum GameAssortment
        {

            /// <summary>
            /// GameAssortment = 8, 
            /// Japan : アメリカン・リーグ, English : American League.       
            /// </summary>
            A = 8,

            /// <summary>
            /// GameAssortment = 9, 
            /// Japan : ナショナル・リーグ, English : National League.               
            /// </summary>  
            Na = 9
        }

        public enum TeamInfoMenu
        {
            /// <summary>
            /// チーム情報　チームトップ（MLB）
            /// TabActive = 1      
            /// Active menu MlbTeamInfoTeamTop
            /// 物理名称	MlbTeamInfoTeamTop																												
            /// Html: 9-5-1
            /// </summary>
            TabActive_1 = 1,

            /// <summary>
            /// チーム情報　日程・結果（MLB）
            /// TabActive = 2      
            /// Active menu MlbTeamInfoDailyResult
            /// 物理名称	MlbTeamInfoDailyResult
            /// Html: 9-5-2
            /// </summary>
            TabActive_2 = 2,

            /// <summary>
            /// チーム情報　対戦成績（MLB）
            /// TabActive = 3     
            /// Active menu MlbTeamInfoConfrontationResult
            /// 物理名称	MlbTeamInfoConfrontationResult																												
            /// Html: 9-5-3
            /// </summary>
            TabActive_3 = 3,


            /// <summary>
            /// チーム情報　ニュース（MLB）
            /// TypeThree = 6
            /// Active menu MlbTeamInfoNews
            /// 物理名称	MlbTeamInfoNews											
            /// Html: 9-5-6
            /// </summary>  
            TabActive_6 = 6

        }

        public enum DivGroup
        {

            /// <summary>
            /// DivGroup = 1 
            /// Japan : 東地区, English : East Division.       
            /// </summary>
            E = 1,

            /// <summary>
            /// DivGroup = 2 
            /// Japan : 中地区, English : Central Division.       
            /// </summary>
            C = 2,

            /// <summary>
            /// DivGroup = 3
            /// Japan : 西地区, English : Central Division.       
            /// </summary>
            W = 3
        }

        public const int OFFICIALSTATS_LEAGUE_AMERICAN = 8;
        public const int OFFICIALSTATS_LEAGUE_NATIONAL = 9;
        public const int OFFICIALSTATS_DIVGROUP_EAST = 1;
        public const int OFFICIALSTATS_DIVGROUP_CENTRAL = 2;
        public const int OFFICIALSTATS_DIVGROUP_WEST = 3;




        #region PartialView News
        public const string MLBTOP = "9-1";
        public const string MLBGAMEINFO = "9-7";
        public const string MLBTEAMINFOTOP = "9-5-1";
        public const string MLBTEAMINFOPITCHER = "8-5-8";
        public const string MLBTEAMINFOBATCHER = "8-5-9";
        public const string NEWSHEADER_1 = "MLB新着ニュース";
        public const string NEWSHEADER_2 = "新着ニュース";
        public const string NEWSHEADER_3 = "ホームチームの新着ニュース";
        public const string NEWSHEADER_4 = "ビジターチームの新着ニュース";
        public const string NEWSLINK_1 = "MLB";
        public const string NEWSLINK_2 = "ニュース";
        public const string NEWSLINK_3 = "一覧を見る";
        public const string NEWSLINK_COMMON = "の一覧を見る";
        #endregion
        /*
9-1	MlbTopController	MLBTOP
9-2	MlbScheduleResultController	日程・結果（MLB）
9-3	MlbOrderController	順位表（MLB）
9-5	MlbTeamInformationController	チーム情報（MLB）
9-5-1	MlbTeamInfoTeamTopController	チーム情報　チームトップ（MLB）
9-5-2	MlbTeamInfoDailyResultController	チーム情報　日程・結果（MLB）
9-5-3	MlbTeamInfoConfrontationResultController	チーム情報　対戦成績（MLB）
9-5-6	MlbTeamInfoNewsController	チーム情報　ニュース（MLB）
9-7	MlbGameInformationController	試合情報（MLB）／試合前・試合後
9-8	MlbNewsListController	ニュースリスト（MLB）
        */


        #region Mlb
        // use in MlbOrderController
        public const int OFFICIALSTATS_GAMEASSORTMENT_CENTRAL_LEAGUE = 1;
        public const int OFFICIALSTATS_GAMEASSORTMENT_PACIFFC_LEAGUE = 2;
        public const int OFFICIALSTATS_GAMEASSORTMENT_EXCHANGE_GAME = 26;
        public const int EXHIBITIONGAMESTATS_GAMEASSORTMENT_EXHIBITION_GAME = 5;
        //use for News URL
        public const string MLB_TOPIC_NAME = "mlb";

        public const int MLB_POST_SPORT_TYPE = 0;
        public const int MLB_POST_TEAM_TYPE = 1;
        public const int MLB_POST_TYPE = 2;
        public const string REMOVE_ZEROS = "G29";
        public const int NEWSCLASSID_GAME = 4;
        public const int NEWSCLASSID_GAME_RESULT = 5;
        #endregion
    
    
    }
}