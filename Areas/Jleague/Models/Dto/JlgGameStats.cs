using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class JlgGameStats
    {

        /// <summary>
        /// 試合ID
        /// </summary>
        public int GameID { get; set; }

        /// <summary>
        /// 試合年月日
        /// </summary>
        public int GameDate { get; set; }

        /// <summary>
        /// 試合種別ID
        /// 'J1の場合は2、J2の場合は6
        /// </summary>
        public int GameKindID { get; set; }

        /// <summary>
        /// 節
        /// </summary>
        public int OccasionNo { get; set; }

        /// <summary>
        /// ホームアウェイ識別
        /// </summary>
        public int HomeAwayClass { get; set; }

        /// <summary>
        /// チームID
        /// </summary>
        public int TeamID { get; set; }

        /// <summary>
        /// チーム名
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// チームアイコン
        /// </summary>
        public string TeamIcon { get; set; }

        /// <summary>
        /// 相手チームID
        /// </summary>
        public int OpponentTeamID { get; set; }

        /// <summary>
        /// 相手チーム名
        /// </summary>
        public string OpponentTeamName { get; set; }

        public string TeamShortName { get; set; }

        /// <summary>
        /// 試合結果ID
        /// </summary>
        public int GameResultID { get; set; }

        /// <summary>
        /// プレー
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// ゴール (OG除く)
        /// </summary>
        public int GoalCount { get; set; }

        /// <summary>
        /// OGによる加点
        /// </summary>
        public int OGCount { get; set; }

        /// <summary>
        /// シュート
        /// </summary>
        public int ShootCount { get; set; }

        /// <summary>
        /// 枠内シュート
        /// </summary>
        public int WithinShootCount { get; set; }

        /// <summary>
        /// 支配率
        /// </summary>
        public double PossessionRatio { get; set; }

        /// <summary>
        /// パス
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// クロス
        /// </summary>
        public int CrossCount { get; set; }

        /// <summary>
        /// 左サイドクロス
        /// </summary>
        public int LeftSideCrossCount { get; set; }

        /// <summary>
        /// 右サイドクロス
        /// </summary>
        public int RightSideCrossCount { get; set; }

        /// <summary>
        /// ドリブル
        /// </summary>
        public int DribbleCount { get; set; }

        /// <summary>
        /// タックル
        /// </summary>
        public int TackleCount { get; set; }

        /// <summary>
        /// クリア
        /// </summary>
        public int ClearCount { get; set; }

        /// <summary>
        /// インターセプト
        /// </summary>
        public int InterceptCount { get; set; }

        /// <summary>
        /// ファウル
        /// </summary>
        public int FoulCount { get; set; }

        /// <summary>
        /// 30mライン進入
        /// </summary>
        public int a30mLineCount { get; set; }

        /// <summary>
        /// フリーキック
        /// </summary>
        public int FK { get; set; }

        /// <summary>
        /// コーナーキック
        /// </summary>
        public int CK { get; set; }

        /// <summary>
        /// オフサイド
        /// </summary>
        public int Offside { get; set; }


    }
}