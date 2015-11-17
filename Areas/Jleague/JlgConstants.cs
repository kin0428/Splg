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
 * Namespace	: Splg.Areas.Npb.Controllers
 * Class		: JlgConstants
 * Developer	: Nguyen Minh Kiet
 * Updated date : 2015-03-30
 */
#endregion

#region Using directives
using System.Web.Mvc;
#endregion

namespace Splg.Areas.Jleague
{
    public class JlgConstants 
    {
        public const string JLG_TOPIC_NAME = "jleague";
        public const string JLG_LEAGUE_NAME_J1 = "J1";
        public const string JLG_LEAGUE_NAME_J2 = "J2";
        public const string JLG_LEAGUE_NAME_NABISCO = "ナビスコC";
        public const int JLG_SPORT_ID = 2;

        public const int JLG_GAMEKIND_J1 = 2;
        public const int JLG_GAMEKIND_J2 = 6;
        public const int JLG_GAMEKIND_NABISCO = 4;
        public const int JLG_GAMECATEGORY_NABISCO_FINAL = 32;
        // ページタイトル
        public const string JLG_TOP_INDEX = "10-1";  //JリーグTOP
        public const string JLG_TOP_J1 = "10-2";  //J1TOP
        public const string JLG_TOP_J2 = "10-3";  //J2TOP
        public const string JLG_TOP_NABISCO = "10-4";  //J2TOP
        public const string JLG_J1_SDL = "10-2-1";  //J1 日程・結果
        public const string JLG_J2_SDL = "10-3-1";  //J2 日程・結果
        public const string JLG_NB_SDL = "10-4-1";  //ナビスコ 日程・結果
        public const string JLG_J1_ODR = "10-2-2";  //J1 順位表
        public const string JLG_J2_ODR = "10-3-2";  //J2 順位表
        public const string JLG_NB_ODR = "10-4-2";  //ナビスコ 順位表
        public const string JLG_J1_MAT = "10-2-3";  //J1 戦績表
        public const string JLG_J2_MAT = "10-3-3";  //J2 戦績表
        public const string JLG_J1_STA = "10-2-3";  //J1 個人成績
        public const string JLG_J2_STA = "10-3-3";  //J2 個人成績
        public const string JLG_J1_GI = "10-2-5";  //J1 チーム情報
        public const string JLG_J2_GI = "10-3-5";  //J2 チーム情報
        public const string JLG_J1_T_TOP = "10-2-5-1"; //J1 チーム情報　TOP
        public const string JLG_J1_T_SCH = "10-2-5-2"; //J1 チーム情報　日程・結果
        public const string JLG_J1_T_RST = "10-2-5-3";       //J1 チーム情報　対戦成績
        public const string JLG_J1_T_PYR = "10-2-5-4";   //J1 チーム情報　選手
        public const string JLG_J1_T_NEWS = "10-2-5-5";   //J1 チーム情報　ニュース
        public const string JLG_J1_T_PYRDTL = "10-2-5-6";   //J1 チーム情報　選手詳細
        public const string JLG_J1_NEWSLIST = "10-3-5-6";   //J1ニュースリスト
        public const string JLG_J2_T_TOP = "10-3-5-1"; //J2 チーム情報　TOP
        public const string JLG_J2_T_SCH = "10-3-5-2"; //J2 チーム情報　日程・結果
        public const string JLG_J2_T_RST = "10-3-5-3";       //J2 チーム情報　対戦成績
        public const string JLG_J2_T_PYR = "10-3-5-4";   //J2 チーム情報　選手
        public const string JLG_J2_T_NEWS = "10-3-5-5";   //J2 チーム情報　ニュース
        public const string JLG_J2_T_PYRDTL = "10-3-5-6";   //J2 チーム情報　選手詳細
        public const string JLG_J2_NEWSLIST = "10-3-5-6";   //J2ニュースリスト
        public const string JLG_NB_NEWSLIST = "10-4-4";   //ナビスコニュースリスト

        public const int JLG_DATETYPE_NEXT = 1; // 次の試合
        public const int JLG_DATETYPE_PREV = 2; // 前の試合

        

        /// <summary>
        /// 
        /// </summary>
        public enum TeamInfoMenu
        {

            //J1, J2, Nabisco共通
        /** 10-2-5-1	チーム情報　チームトップ（J1）
            10-2-5-2	チーム情報　日程・結果（J1）
            10-2-5-3	チーム情報　対戦成績（J1）
            10-2-5-4	チーム情報　選手（J1）
            10-2-5-5	チーム情報　ニュース（J1）
            10-2-5-6	チーム情報　選手詳細（J1）**/

            /// <summary>
            /// チーム情報　チームトップ
            /// TabActive = 1      
            /// Active menu MlbTeamInfoTeamTop
            /// 物理名称	MlbTeamInfoTeamTop																												
            /// Html:10-2-5-1
            /// </summary>
            TabActive_1 = 1,

            /// <summary>
            /// 日程・結果
            /// TabActive = 2      
            /// Active menu MlbTeamInfoDailyResult
            /// 物理名称	MlbTeamInfoDailyResult
            /// Html: 10-2-5-2
            /// </summary>
            TabActive_2 = 2,

            /// <summary>
            /// チーム情報　対戦成績
            /// TabActive = 2      
            /// Active menu MlbTeamInfoDailyResult
            /// 物理名称	MlbTeamInfoDailyResult
            /// Html: 10-2-5-3
            /// </summary>
            TabActive_3 = 3,

            /// <summary>
            /// チーム情報　選手
            /// TabActive = 3     
            /// Active menu MlbTeamInfoConfrontationResult
            /// 物理名称	MlbTeamInfoConfrontationResult																												
            /// Html: 10-2-5-4
            /// </summary>
            TabActive_4 = 4,

            /// <summary>
            /// チーム情報　ニュース
            /// TabActive = 3     
            /// Active menu MlbTeamInfoConfrontationResult
            /// 物理名称	MlbTeamInfoConfrontationResult																												
            /// Html: 10-2-5-5
            /// </summary>
            TabActive_5 = 5,


            /// <summary>
            /// チーム情報　選手詳細
            /// TypeThree = 6
            /// Active menu MlbTeamInfoNews
            /// 物理名称	MlbTeamInfoNews											
            /// 10-2-5-6
            /// </summary>  
            TabActive_6 = 6

        }
    }
}