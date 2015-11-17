using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague.Models
{
    public class JlgScheduleResultViewModel
    {
        public int ScheduleInfoId { get; set; }

        public int GameID { get; set; }

        public int HomeTeamID { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayTeamID { get; set; }

        public string AwayTeamName { get; set; }

        public int HomeRanking { get; set; }

        public int AwayRanking { get; set; }

        public int HomeWin { get; set; }

        public int AwayWin { get; set; }

        /// <summary>
        /// 節
        /// </summary>
        public int OccasionNo { get; set; }

        /// <summary>
        /// 節最大値
        /// </summary>
        public int MaxOccasionNo { get; set; }

        /// <summary>
        /// リーグ名
        /// </summary>
        public string LeagueName { get; set; }

        /// <summary>
        /// ゲーム種別
        /// </summary>
        public int GameKind { get; set; }

        /// <summary>
        /// ゲーム種別名
        /// </summary>
        public string GameKindName { get; set; }

        /// <summary>
        /// シーズンId
        /// </summary>
        public int SeasonId { get; set; }

        /// <summary>
        /// JType（フラグ）
        /// </summary>
        public JlgConst.JType JType { get; set; }

        /// <summary>
        /// Jリーグメニュー
        /// </summary>
        public int JLeagueMenu { get; set; }

        /// <summary>
        /// Jリーグサブメニュー
        /// </summary>
        public int JLeagueSubMenu { get; set; }

        /// <summary>
        /// ページ名
        /// </summary>
        public string PageTitle {get;set;}

        /// <summary>
        /// ページタイトル
        /// </summary>
        public string PageName { get; set; }

        public int GameDate { get; set; }

        public IEnumerable<JlgScheduleResultNabiscoInfoModel> ScheduleInfoNb { get; set; }

        public int finalCnt { get; set; }

        /// <summary>
        /// タブアクティブ判定(J1,Nabisco)
        /// </summary>
        public string IsActiveTabMenu(int target)
        {
            return (target == SeasonId) ? "active" : string.Empty;
        }

        /// <summary>
        /// J1通年タブ内アクティブ節判定
        /// </summary>
        public string IsActiveOccasionNoInJ1AllTab(int targetOccasionNo,int targetSeasonId)
        {
            return (targetOccasionNo == OccasionNo && targetSeasonId == SeasonId) ? "active" : string.Empty;
        }

        /// <summary>
        /// J1シーズンタブ内アクティブ節判定
        /// </summary>
        public string IsActiveOccasionNoInJ1SeasonTab(int targetOccasionNo)
        {
            return (targetOccasionNo == OccasionNo) ? "active" : string.Empty;
        }

        /// <summary>
        /// J2シーズンタブ内アクティブ節判定
        /// </summary>
        public string IsActiveOccasionNoInJ2SeasonTab(int targetOccasionNo)
        {
            return (targetOccasionNo == OccasionNo) ? "active" : string.Empty;
        }
    }

    public class JlgScheduleResultNabiscoInfoModel
    {

        public string finalRoundName { get; set; }

        public int finalRound { get; set; }

        public int finalOccasion { get; set; }
    }

}