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
 * Namespace	: Splg.Areas.Npb.Models.ViewModel
 * Class		: NpbGameInfoViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using Splg.Models.Game.ViewModel;
using Splg.Models.News.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Models;
using Splg.Areas.Jleague.Models.Dto;
using Splg.Core.Models.Flotr2.Dto.FormationChart;
using Splg.Core.Constant;
#endregion

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgGameInfoViewModel
    {
        public JlgGameInfoViewModel()
        {
            homeTeamSpec = new JlgTeamSpec();
            awayTeamSpec = new JlgTeamSpec();
            FormationChartDto = new FormationChartDto();
        }

        public int GameID { get; set; }


        #region 基本情報の設定

        /// <summary>
        /// 試合スケジュール_試合情報  (試合前)
        /// </summary>
        private ScheduleInfo scheduleInfo;

        public ScheduleInfo ScheduleInfo
        {
            get { return scheduleInfo; }
            set
            {
                scheduleInfo = value;
                if (scheduleInfo != null)
                {
                    homeTeamID = (int)scheduleInfo.HomeTeamID;
                    homeTeamName = scheduleInfo.HomeTeamName;
                    homeTeamNameS = scheduleInfo.HomeTeamNameS;
                    awayTeamID = (int)scheduleInfo.AwayTeamID;
                    awayTeamName = scheduleInfo.AwayTeamName;
                    awayTeamNameS = scheduleInfo.AwayTeamNameS;

                    occasionNo = scheduleInfo.OccasionNo;

                    gameDateScheduled = scheduleInfo.GameDate;
                    gameTimeScheduled = scheduleInfo.GameTime;

                }
            }
        }

        int? occasionNo;

        /// <summary>
        /// 節
        /// </summary>
        public int? OccasionNo
        {
            get { return occasionNo; }
        }

        private GameReportLG gameReportLG;
        /// <summary>
        /// 試合中  一試合速報_ヘッダー情報
        /// </summary>
        public GameReportLG GameReportLG
        {
            get { return gameReportLG; }
            set
            {
                gameReportLG = value;
                if (gameReportLG != null)
                {
                    gameDateReal = gameReportLG.GameDate;
                    gameTimeReal = gameReportLG.StartTime;
                    gameKind = gameReportLG.GameKindID;
                }
            }
        }

        private GameStateLG gameStateLG;
        public bool HasPK;
        /// <summary>
        /// 試合中  一試合速報_ヘッダー情報
        /// </summary>
        public GameStateLG GameStateLG
        {
            get { return gameStateLG; }
            set
            {
                gameStateLG = value;
                if (gameStateLG != null)
                {
                    //PKあるなしの判定
                    if (gameStateLG.PK > 0)
                        HasPK = true;
                }
            }
        }

        public List<JlgRecapModel> Recaps {get;set;}

        int gameKind;

        /// <summary>
        /// 試合種別
        /// </summary>
        public int GameKind
        {
            get { return gameKind; }
        }


        /// <summary>
        /// 試合前　試合別通算対戦成績_試合情報
        /// </summary>
        public ScheduleInfoHE ScheduleInfoHE { get; set; }

        #endregion

        int homeTeamID;
        public int HomeTeamID
        {
            get
            {
                return homeTeamID;
            }
        }
        string homeTeamName;
        public string HomeTeamName
        {
            get
            {
                return homeTeamName;
            }
        }
        string homeTeamNameS;
        public string HomeTeamNameS
        {
            get
            {
                return homeTeamNameS;
            }
        }
        int awayTeamID;
        public int AwayTeamID
        {
            get
            {
                return awayTeamID;
            }
        }
        string awayTeamName;
        public string AwayTeamName
        {
            get
            {
                return awayTeamName;
            }
        }
        string awayTeamNameS;
        public string AwayTeamNameS
        {
            get
            {
                return awayTeamNameS;
            }
        }

        int? gameDateScheduled;
        /// <summary>
        /// 試合日（予定）
        /// </summary>
        public int? GameDateScheduled
        {
            get
            {
                return gameDateScheduled;
            }
        }

        int? gameTimeScheduled;
        /// <summary>
        /// 試合開始時間（予定）
        /// </summary>
        public int? GameTimeScheduled
        {
            get
            {
                return gameTimeScheduled;
            }
        }

        int? gameDateReal;
        /// <summary>
        /// 試合日（実績）
        /// </summary>
        public int? GameDateReal
        {
            get
            {
                return gameDateReal;
            }
        }

        int? gameTimeReal;
        /// <summary>
        /// 試合開始時間（実績）
        /// </summary>
        public int? GameTimeReal
        {
            get
            {
                return gameTimeReal;
            }
        }


        /// <summary>
        /// 試合日（実績）があれば実績を返す。
        /// さもなくば予定を返す。
        /// </summary>
        public int? GameDateManaged
        {
            get
            {
                int? result = GameDateScheduled;

                if (GameDateReal != null)
                    result = GameDateReal;

                return result;
            }
        }

        /// <summary>
        /// 試合開始時間（実績）があれば実績を返す。
        /// さもなくば予定を返す。
        /// </summary>
        public int? GameTimeManaged
        {
            get
            {
                int? result = GameTimeScheduled;

                if (GameTimeReal != null)
                    result = GameTimeReal;

                return result;
            }
        }

        public int GameStatus { get; set; }



        //フォローユーザーの予想
        /// <summary>
        /// //ホームの勝ち
        /// </summary>
        public List<Member> FollowMembersBetToWin { get; set; }
        /// <summary>
        /// ビジターの勝ち
        /// </summary>
        public List<Member> FollowMembersBetToDraw { get; set; }
        /// <summary>
        /// 引き分け
        /// </summary>
        public List<Member> FollowMembersBetToLose { get; set; }

        /// <summary>
        /// この試合の投稿記事
        /// </summary>
        public IEnumerable<PostedInfoViewModel> PostedInfos { get; set; }


        #region 試合前情報

        /// <summary>
        /// 試合前 過去の勝敗 ホームチーム最近５試合成績情報
        /// </summary>
        public IEnumerable<JlgLast5GamesModel> List5GamesHomeInHistory { get; set; }

        /// <summary>
        /// 試合前 過去の勝敗 アウェイチーム最近５試合成績情報
        /// </summary>
        public IEnumerable<JlgLast5GamesModel> List5GamesAwayInHistory { get; set; }
        
        /// <summary>
        /// 試合前 過去の直接対決の勝敗 チーム最近５試合成績情報
        /// </summary>
        public IEnumerable<JlgPast5GamesModel> List5GamesInHistory { get; set; }

        #endregion

        #region 試合中以降情報

        //ホームチーム一試合速報_得点情報
        public List<JlgGoalInfo> HomeGoalInfos { get; set; }

        public string HomeGoalInfo
        {
            get
            {
                string result = "";
                if (HomeGoalInfos != null)
                {
                    foreach (var g in HomeGoalInfos)
                    {
                        result += "<p>" + g.GPlayerNameS + "</p>";
                    }
                }
                return result;
            }
        }

        //アウエイチーム一試合速報_得点情報
        public List<JlgGoalInfo> AwayGoalInfos { get; set; }

        public string AwayGoalInfo
        {
            get
            {
                string result = "";
                if (AwayGoalInfos != null)
                {
                    foreach (var g in AwayGoalInfos)
                    {
                        result += "<p>" + g.GPlayerNameS + "</p>";
                    }
                }
                return result;
            }
        }

        //ホームチーム一試合速報_警告、退場情報
        public List<WarningInfoLG> HomeWarningInfos { get; set; }

        public int HomeWarningInfo(int? playerID)
        {
            {
                int result = 0;
                if (HomeWarningInfos != null)
                {
                    foreach (var w in HomeWarningInfos.Where(x => x.PlayerID == playerID))
                    {
                        result += (int)w.Divide + (int)w.SecondF;
                    }
                }
                return result;
            }
        }


        //アウエイチーム一試合速報_警告、退場情報
        public List<WarningInfoLG> AwayWarningInfos { get; set; }
        // カードの色判別用        
        public int AwayWarningInfo(int? playerID)
        {
            {
                int result = 0;
                if (AwayWarningInfos != null)
                {
                    foreach (var w in AwayWarningInfos.Where(x => x.PlayerID == playerID))
                    {
                        result += (int)w.Divide + (int)w.SecondF;
                    }
                }
                return result;
            }
        }

        //ホームチーム一試合速報_PK戦情報
        public List<JlgPKInfoModel> HomePKInfos { get; set; }

        //アウエイチーム一試合速報_PK戦情報
        public List<JlgPKInfoModel> AwayPKInfos { get; set; }

        /// <summary>
        /// 試合中 コメント配信_コメント情報
        /// </summary>
        public List<JlgGameText> ListGameTexts { get; set; }

        /// <summary>
        /// 一試合選手速報
        /// </summary>
        public List<JlgMembersInGame> HomeStartingMembers { get; set; }
        public List<JlgMembersInGame> HomeSubMembers { get; set; }
        public List<JlgMembersInGame> AwayStartingMembers { get; set; }
        public List<JlgMembersInGame> AwaySubMembers { get; set; }

        /// <summary>
        /// 欠場選手情報
        /// </summary>
        public List<JlgPlayerGameInfoModel> HomeInjuredMembers { get; set; }
        public List<JlgPlayerGameInfoModel> AwayInjuredMembers { get; set; }

        /// <summary>
        /// 出場停止情報
        /// </summary>
        public List<JlgPlayerGameInfoModel> HomeSuspendedMembers { get; set; }
        public List<JlgPlayerGameInfoModel> AwaySuspendedMembers { get; set; }

        /// <summary>
        /// 交代情報
        /// </summary>
        public List<JlgPlayerGameDetailInfoModel> HomeStartingMamberGameDetailCardInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> HomeSubMamberGameDetailCardInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwayStartingMamberGameDetailCardInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwaySubMamberGameDetailCardInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> HomeStartingMamberGameDetailGoalInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> HomeSubMamberGameDetailGoalInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwayStartingMamberGameDetailGoalInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwaySubMamberGameDetailGoalInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> HomeStartingMamberGameDetailInInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> HomeSubMamberGameDetailInInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwayStartingMamberGameDetailInInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwaySubMamberGameDetailInInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> HomeStartingMamberGameDetailOutInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> HomeSubMamberGameDetailOutInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwayStartingMamberGameDetailOutInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> AwaySubMamberGameDetailOutInfos { get; set; }

        #region 警告・退場の時間
        public string HomeStartingMemberCardTime( int PlayerId)
        {
            string result = "";
            if (HomeStartingMamberGameDetailCardInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeStartingMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                    if(counter == 1 && w.Divide == 1)
                    {
                        result += "警告 "+ w.Time + "分";
                        counter++;
                    }
                    else if(counter == 1 && w.Divide == 2)
                    {
                        result += "退場 " + w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += ",退場 " + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public int HomeStartingMemberCardType(int PlayerId)
        {
            int result = 0;
            if (HomeStartingMamberGameDetailCardInfos != null)
            {
                foreach (var w in HomeStartingMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                        result += w.Divide + w.SecondF;
                }
            }
            return result;
        }
        public string HomeSubMemberCardTime(int PlayerId)
        {
            string result = "";
            if (HomeSubMamberGameDetailCardInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeSubMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                    if (counter == 1 && w.Divide == 1)
                    {
                        result += "警告 " + w.Time + "分";
                        counter++;
                    }
                    else if (counter == 1 && w.Divide == 2)
                    {
                        result += "退場 " + w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += ",退場 " + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public int HomeSubMemberCardType(int PlayerId)
        {
            int result = 0;
            if (HomeSubMamberGameDetailCardInfos != null)
            {
                foreach (var w in HomeSubMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                    result += w.Divide + w.SecondF;
                }
            }
            return result;
        }
        public string AwayStartingMemberCardTime(int PlayerId)
        {
            string result = "";
            if (AwayStartingMamberGameDetailCardInfos != null)
            {
                int counter = 1;
                foreach (var w in AwayStartingMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                    if (counter == 1 && w.Divide == 1)
                    {
                        result += "警告 " + w.Time + "分";
                        counter++;
                    }
                    else if (counter == 1 && w.Divide == 2)
                    {
                        result += "退場 " + w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += ",退場 " + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public int AwayStartingMemberCardType(int PlayerId)
        {
            int result = 0;
            if (AwayStartingMamberGameDetailCardInfos != null)
            {
                foreach (var w in AwayStartingMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                    result += w.Divide + w.SecondF;
                }
            }
            return result;
        }
        public string AwaySubMemberCardTime(int PlayerId)
        {
            string result = "";
            if (AwaySubMamberGameDetailCardInfos != null)
            {
                int counter = 1;
                foreach (var w in AwaySubMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                    if (counter == 1 && w.Divide == 1)
                    {
                        result += "警告 " + w.Time + "分";
                        counter++;
                    }
                    else if (counter == 1 && w.Divide == 2)
                    {
                        result += "退場 " + w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += ",退場 " + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public int AwaySubMemberCardType(int PlayerId)
        {
            int result = 0;
            if (AwaySubMamberGameDetailCardInfos != null)
            {
                foreach (var w in AwaySubMamberGameDetailCardInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "card"))
                {
                    result += w.Divide + w.SecondF;
                }
            }
            return result;
        }
        #endregion
        #region 得点の時間
        public string HomeStartingMemberGoalTime(int PlayerId)
        {
            string result = "";
            if (HomeStartingMamberGameDetailGoalInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeStartingMamberGameDetailGoalInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "goal"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string HomeSubMemberGoalTime(int PlayerId)
        {
            string result = "";
            if (HomeSubMamberGameDetailGoalInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeSubMamberGameDetailGoalInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "goal"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string AwayStartingMemberGoalTime(int PlayerId)
        {
            string result = "";
            if (AwayStartingMamberGameDetailGoalInfos != null)
            {
                int counter = 1;
                foreach (var w in AwayStartingMamberGameDetailGoalInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "goal"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string AwaySubMemberGoalTime(int PlayerId)
        {
            string result = "";
            if (AwaySubMamberGameDetailGoalInfos != null)
            {
                int counter = 1;
                foreach (var w in AwaySubMamberGameDetailGoalInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "goal"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        #endregion
        #region 途中出場の時間
        public string HomeStartingMemberInTime(int PlayerId)
        {
            string result = "";
            if (HomeStartingMamberGameDetailInInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeStartingMamberGameDetailInInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "in"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string HomeSubMemberInTime(int PlayerId)
        {
            string result = "";
            if (HomeSubMamberGameDetailInInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeSubMamberGameDetailInInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "in"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string AwayStartingMemberInTime(int PlayerId)
        {
            string result = "";
            if (AwayStartingMamberGameDetailInInfos != null)
            {
                int counter = 1;
                foreach (var w in AwayStartingMamberGameDetailInInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "in"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string AwaySubMemberInTime(int PlayerId)
        {
            string result = "";
            if (AwaySubMamberGameDetailInInfos != null)
            {
                int counter = 1;
                foreach (var w in AwaySubMamberGameDetailInInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "in"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        #endregion
        #region 途中交代の時間
        public string HomeStartingMemberOutTime(int PlayerId)
        {
            string result = "";
            if (HomeStartingMamberGameDetailOutInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeStartingMamberGameDetailOutInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "out"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string HomeSubMemberOutTime(int PlayerId)
        {
            string result = "";
            if (HomeSubMamberGameDetailOutInfos != null)
            {
                int counter = 1;
                foreach (var w in HomeSubMamberGameDetailOutInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "out"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string AwayStartingMemberOutTime(int PlayerId)
        {
            string result = "";
            if (AwayStartingMamberGameDetailOutInfos != null)
            {
                int counter = 1;
                foreach (var w in AwayStartingMamberGameDetailOutInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "out"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        public string AwaySubMemberOutTime(int PlayerId)
        {
            string result = "";
            if (AwaySubMamberGameDetailOutInfos != null)
            {
                int counter = 1;
                foreach (var w in AwaySubMamberGameDetailOutInfos.Where(x => x.PlayerId == PlayerId).Where(x => x.Itype == "out"))
                {
                    if (counter == 1)
                    {
                        result += w.Time + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + w.Time + "分";
                    }
                }
            }
            return result;
        }
        #endregion        
        public List<JlgPlayerGameInfoModel> HomeStartingMamberWarningInfos { get; set; }
        public List<JlgPlayerGameInfoModel> HomeSubMamberWarningInfos { get; set; }
        public List<JlgPlayerGameInfoModel> AwayStartingWarningInfos { get; set; }
        public List<JlgPlayerGameInfoModel> AwaySubMamberWarningInfos { get; set; }

        #endregion

        #region 試合後情報

        /// <summary>
        /// 一試合速報_試合情報　（試合中、試合終了）
        /// </summary>
        public JlgGameEndScroreInfoModel HomeGameScore { get; set; }
        public JlgGameEndScroreInfoModel AwayGameScore { get; set; }


        public List<JlgScoreDetailsModel> ScoreDetails { get; set; }

        public List<JlgScoreDetailsModel> HomeScoreDetails
        {
            get
            {
                if (ScoreDetails != null)
                    return ScoreDetails.Where(x => x.Hv == 1).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> AwayScoreDetails
        {
            get
            {
                if (ScoreDetails != null)
                    return ScoreDetails.Where(x => x.Hv == 2).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> HomeFirstHalfScoreDetails
        {
            get
            {
                if (HomeScoreDetails != null)
                    return HomeScoreDetails.Where(x => x.StateID ==1 || x.StateID == 3 || x.StateID == 5).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> HomeSecondHalfScoreDetails
        {
            get
            {
                if (HomeScoreDetails != null)
                    return HomeScoreDetails.Where(x => x.StateID == 2 || x.StateID == 4 || x.StateID == 6).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> AwayFirstHalfScoreDetails
        {
            get
            {
                if (AwayScoreDetails != null)
                    return AwayScoreDetails.Where(x => x.StateID == 1 || x.StateID == 3 || x.StateID == 5).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> AwaySecondHalfScoreDetails
        {
            get
            {
                if (AwayScoreDetails != null)
                    return AwayScoreDetails.Where(x => x.StateID == 2 || x.StateID == 4 || x.StateID == 6).ToList();
                else return null;
            }
        }


        /// <summary>
        /// 試合後 一試合速報_試合終了情報
        /// </summary>
        public JlgGameEndInfoModel GameEndInfo { get; set; }  //試合終了

        /// <summary>
        /// ホーム試合スタッツ
        /// </summary>
        public JlgGameStats HomeGameStats { get; set; }

        /// <summary>
        /// アウエイ試合スタッツ
        /// </summary>
        public JlgGameStats AwayGameStats { get; set; }

        #endregion

        // フォーメーション用のデータの入れ物
        public FormationChartDto FormationChartDto { get; set; }

        // 攻撃力、守備力の入れ物
        // チーム用
        // ホーム
        public JlgTeamSpec homeTeamSpec { get; set; }
        //アウェー
        public JlgTeamSpec awayTeamSpec { get; set; }

        // プレイヤー用
        // ホーム
        public List<JlgFormationInfo> homePlayerSpec { get; set; }
        // アウェー
        public List<JlgFormationInfo> awayPlayerSpec { get; set; }
        
    }
}