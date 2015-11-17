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
 * Namespace	: Splg
 * Class		: Constants
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
#endregion

namespace Splg
{
    /// <summary>
    /// Define const for this project.
    /// </summary>
    public class Constants
    {
        #region Optional
        public const string EMAIL_SEND = "Success";
        public const string FACEBOOK = "Facebook";
        public const string GOOGLE = "Google+";
        public const string TWITTER = "Twitter";
        public const string FAQ = "faq";
        public const string IMG_DEFAULT_PROFILE = @"/Content/img/upload/member/DefaultProfilePicture.png";
        public const int NPB_SPORT_ID = 1;
        public const int JLG_SPORT_ID = 2;
        public const int MLB_SPORT_ID = 3;
        public const int WS_SPORT_ID = 4;
        #endregion

        #region Member
        public const string REGISTER_REVIEW = "2-1-1";
        public const string USER_INFO = "UserInfo";
        public const string AUTH_COOKIE = "auth_cookie";

        /// <summary>
        /// 会員.ステータス
        /// 0：仮入会
        /// </summary>
        public const int MEMBER_STATUS_APPLIED = 0;

        /// <summary>
        /// 会員.ステータス
        ///  1：入会
        /// </summary>
        public const int MEMBER_STATUS_REGISTERD = 1;

        /// <summary>
        /// 会員.ステータス
        /// 2：退会
        /// </summary>
        public const int MEMBER_STATUS_LEFT = 2;

        /// <summary>
        /// 会員.ステータス
        /// 3：強制退会
        /// </summary>
        public const int MEMBER_STATUS_EXCLUDED = 3;
        #endregion

        #region PartialView News
        public const string NPBTOP = "8-1";
        public const string NPBSCHEDULE = "8-2";
        public const string NPBORDER = "8-3";
        public const string NPBSTATS = "8-4";
        public const string NPBTEAMINFO = "8-5";
        public const string NPBGAMEINFO = "8-6";
        public const string NPBNEWS = "8-7";
        public const string NPBTEAMINFOTOP = "8-5-1";
        public const string NPBTEAMINFOSCHE = "8-5-2";
        public const string NPBTEAMINFOSTAT = "8-5-3";
        public const string NPBTEAMINFOPIT = "8-5-4";
        public const string NPBTEAMINFOBAT = "8-5-5";
        public const string NPBTEAMINFOSIR = "8-5-6";
        public const string NPBTEAMINFONEWS = "8-5-7";
        public const string NPBTEAMINFOPITCHER = "8-5-8";
        public const string NPBTEAMINFOBATCHER = "8-5-9";
        public const string NEWSHEADER_1 = "プロ野球 新着ニュース";
        public const string NEWSHEADER_1_MLB = "MLB 新着ニュース";
        public const string NEWSHEADER_1_Jleague = "Jリーグ 新着ニュース";
        public const string NEWSHEADER_2 = "新着ニュース";
        public const string NEWSHEADER_3 = "ホームチームの新着ニュース";
        public const string NEWSHEADER_4 = "ビジターチームの新着ニュース";
        public const string NEWSHEADER_4_Jleague = "アウエイチームの新着ニュース";
        public const string NEWSLINK_1 = "プロ野球";
        public const string NEWSLINK_1_MLB = "MLB";
        public const string NEWSLINK_1_Jleague = "Jリーグ";
        public const string NEWSLINK_2 = "ニュース";
        public const string NEWSLINK_3 = "一覧を見る";
        public const string NEWSLINK_COMMON = "の一覧を見る";
        #endregion

        #region Page 8-6
        /// <summary>
        /// Used for PitcherArm
        /// 1=左投げ、2=右投げ
        /// </summary>       
        public const string PITCHERARM_1 = "左投げ";
        public const string PITCHERARM_2 = "右投げ";
        /// <summary>
        /// Used for BattingType
        /// 1=左打ち、2=右打ち、3=両打ち
        /// </summary>
        public const string BATTINGTYPE_1 = "左打ち";
        public const string BATTINGTYPE_2 = "右打ち";
        public const string BATTINGTYPE_3 = "両打ち";
        /// <summary>
        /// Page 8-6-1
        /// </summary>
        public const string STRING_WIN = "勝";
        public const string STRING_LOSE = "敗";
        public const string STRING_SAVE = "S";
        public const string TOTALC = "号";
        public const string ROUND_BOTTOM = "回裏";
        public const string ROUND_TOP = "回表";
        public const string STRING_ROUND = "回";
        /// <summary>
        /// For right content game.
        /// </summary>
        public const string HOMETEAM = "ホーム";
        public const string VISITORTEAM = "ビジター";
        public const short HvHOME = 1;
        public const short HvVISITOR = 2;
        #endregion

        #region News
        public const string IMAGE_THUMNAIL_DUID = "CI0002";
        public const string IMAGE_DUID = "CI0003";
        public const string IMAGE_CAPTION_DUID = "CI0001";
        public const string IMAGE_DIRECTORY = @"~/Content/News/PN_UTF8/photo/";
        public const string IMAGE_DEFAULT = "default.png";
        public const int NPB_ITPCSUBJECTCODE = 15007990;
        public const int MLB_ITPCSUBJECTCODE = 15007991;
        public const int JLEAGUE_ITPCSUBJECTCODE = 15054990;
        public const int OVERSEA_SOCCER_ITPCSUBJECTCODE = 15054991;
        public const string NEWS_VALID_STATUS = "Usable";
        //public const int GAME_TOPIC_CLASSIFICATION = 1;
        public const int TEAM_TOPIC_CLASSIFICATION = 2;
        public const int PLAYER_TOPIC_CLASSIFICATION = 3;
        public const int LEAGUE_TOPIC_CLASSIFICATION = 5;

        public const string NEWS_DEFAULT_PARAMETER = "-";
        #endregion

        #region Npb
        // use in NpbOrderController
        public const int OFFICIALSTATS_GAMEASSORTMENT_CENTRAL_LEAGUE = 1;
        public const int OFFICIALSTATS_GAMEASSORTMENT_PACIFFC_LEAGUE = 2;
        public const int OFFICIALSTATS_GAMEASSORTMENT_EXCHANGE_GAME = 26;
        public const int EXHIBITIONGAMESTATS_GAMEASSORTMENT_EXHIBITION_GAME = 5;
        //use for News URL
        public const string NPB_TOPIC_NAME = "npb";

        public const int NPB_POST_SPORT_TYPE = 0;
        public const int NPB_POST_TEAM_TYPE = 1;
        public const int NPB_POST_TYPE = 2;
        public const string REMOVE_ZEROS = "G29";
        public const int NEWSCLASSID_GAME = 4;
        public const int NEWSCLASSID_GAME_RESULT = 5;
        #endregion

        #region Mlb
        //use for News URL
        public const string MLB_TOPIC_NAME = "mlb";

        public const string MLB_TOP_INDEX = "9-1";  //MLBTOP
        public const string MLB_SCHEDULE_RESULT = "9-2";  //日程・結果（MLB）
        public const string MLB_ORDER_INDEX = "9-3";     //順位表（MLB）
        public const string MLB_TEAMINFO_RMATION = "9-5"; //チーム情報（MLB）
        public const string MLB_TEAMINFO_TEAMTOP = "9-5-1";       //チーム情報　チームトップ（MLB）
        public const string MLB_TEAMINFO_DAILYRESULT = "9-5-2";   //チーム情報　日程・結果（MLB）
        public const string MLB_TEAMINFO_CONFRONTATIONRESULT = "9-5-3";   //チーム情報　対戦成績（MLB）
        public const string MLB_TEAMINFO_NEWS = "9-5-6";  //チーム情報　ニュース（MLB）
        public const string MLB_GAME_INFORMATION = "9-7"; //試合情報（MLB）／試合前・試合後
        public const string MLB_NEWS_LIST = "9-8";        //ニュースリスト（MLB）
        #endregion

        #region JLeague
        //use for News URL
        public const string JLG_TOPIC_NAME = "jleague";

        public const string JLG_TOP_INDEX = "10-1";  //JリーグTOP


        public const string JLG_TOP_J1 = "10-2";  //J1TOP
        public const string JLG_J1_SDL = "10-2-1";  //J1 日程・結果
        public const string JLG_J1_ODR = "10-2-2";  //順位表
        public const string JLG_J1_RESULT = "10-2-3";  //戦績表
        public const string JLG_J1_PERSONAL = "10-2-4";  //個人成績
        public const string JLG_J1_T_INFO = "10-2-5";  //チーム情報
        public const string JLG_J1_T_TOP = "10-2-5-1"; //J1 チーム情報　TOP
        public const string JLG_J1_T_SDL = "10-2-5-2";       //J1 チーム情報　日程・結果
        public const string JLG_J1_T_RESULT = "10-2-5-3";       //J1 チーム情報　対戦結果
        public const string JLG_J1_T_PYR = "10-2-5-4";   //J1 チーム情報　選手
        public const string JLG_J1_T_NEWS = "10-2-5-5";       //J1 チーム情報　ニュース
        public const string JLG_J1_T_PYR_DTL = "10-2-5-6";       //J1 チーム情報　選手詳細
        public const string JLG_J1_NEWS = "10-2-6";       //J1 ニュースリスト

        public const string JLG_TOP_J2 = "10-3";  //J2TOP
        public const string JLG_J2_SDL = "10-3-1";  //J2 日程・結果
        public const string JLG_J2_ODR = "10-3-2";  //順位表
        public const string JLG_J2_RESULT = "10-3-3";  //戦績表
        public const string JLG_J2_PERSONAL = "10-3-4";  //個人成績
        public const string JLG_J2_T_INFO = "10-3-5";  //チーム情報
        public const string JLG_J2_T_TOP = "10-3-5-1"; //J2 チーム情報　TOP
        public const string JLG_J2_T_SDL = "10-3-5-2";       //J2 チーム情報　日程・結果
        public const string JLG_J2_T_RESULT = "10-3-5-3";       //J2 チーム情報　対戦結果
        public const string JLG_J2_T_PYR = "10-3-5-4";   //J2 チーム情報　選手
        public const string JLG_J2_T_NEWS = "10-3-5-5";       //J2 チーム情報　ニュース
        public const string JLG_J2_T_PYR_DTL = "10-3-5-6";       //J2 チーム情報　選手詳細
        public const string JLG_J2_NEWS = "10-3-6";       //J2 ニュースリスト

        public const string JLG_TOP_NABISCO = "10-4";  //NABISCO TOP
        public const string JLG_NABISCO_SDL = "10-4-1";  //NABISCO 日程・結果
        public const string JLG_NABISCO_ODR = "10-4-2";  //NABISCO 順位表（ナビスコC）

        public const string JLG_GAME_INFO = "10-5";   //試合情報（Jリーグ）／試合前・試合中・試合後
        public const string JLG_NEWS = "10-6";   //ニュースリスト（Jリーグ）

        #endregion

        #region Page 8-5-8
        public const string PITCHINGRESULT_0 = "○";
        public const string PITCHINGRESULT_1 = "●";
        public const string PITCHINGRESULT_2 = "セーブ";
        public const string PITCHINGRESULT_3 = "";
        public const string PITCHINGRESULT_4 = "ホールド";
        public const string INNINGSPITCHED3RD_0 = "空白";
        public const string LEAGUENAME_FIRST = "対";
        public const string LEAGUENAME_AFTER = "チーム別成績";
        public const string TEAMVS = "vs";

        //0=先発、1=完投、2=交代了、3=その他.
        public const string MOUND_0 = "先発";
        public const string MOUND_1 = "完投";
        public const string MOUND_2 = "交代";
        public const string MOUND_3 = "その他";

        //1=投手, 2=捕手, 3=内野手, 4=外野手.
        public const string POSITIONTYPE_1 = "投手";
        public const string POSITIONTYPE_2 = "捕手";
        public const string POSITIONTYPE_3 = "内野手";
        public const string POSITIONTYPE_4 = "外野手";

        public const string HITTINGRESULT_0 = "";
        public const string HITTINGRESULT_1 = "○";
        public const string HITTINGRESULT_2 = "●";
        public const string HITTINGRESULT_3 = "引き分け";
        #endregion

        #region Game
        public const int POINT_DEFAULT = 100;
        public const string ENCRYTION = "Authentication Token";

        /// <summary>
        /// 試合ステータス
        /// 0:試合前
        /// </summary>
        public const int GAME_STATUS_PRE_GAME = 0;

        /// <summary>
        /// 試合ステータス
        /// 1:試合中
        /// </summary>
        public const int GAME_STATUS_DURING_GAME = 1;

        /// <summary>
        /// 試合ステータス
        /// 2:試合後
        /// </summary>
        public const int GAME_STATUS_POST_GAME = 2;

        #endregion

        #region 選手情報
        // ポジション
        public const short PositionPither = 1;
        #endregion
        #region Point
        /// <summary>
        /// 応募除外ポイント
        /// </summary>
        public static readonly int ExcludedPoint = 100000;
        #endregion
    }
}