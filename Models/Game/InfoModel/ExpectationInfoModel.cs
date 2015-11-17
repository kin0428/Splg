using System;
using System.Collections.Generic;
using System.Linq;

namespace Splg.Models.Game.InfoModel
{
    public class ExpectationInfoModel
    {
        public long MemberID { get; set; }
        public long PointID { get; set; }
        public int GiveTargetMonth { get; set; }

        public int SportID { get; set; }
        public int ExpectPoint { get; set; }
        public long ExpectPointID { get; set; }
        private decimal odds;

        public decimal Odds
        {
            get
            {
                decimal reault = Math.Round(odds, 1, MidpointRounding.AwayFromZero);
                return reault;
            }
            set { odds = value; }
        }

        public int BetSelectID { get; set; }
        public int? GameID { get; set; }
        public DateTime BetStartDate { get; set; }
        public DateTime? StartScheduleDate { get; set; }
        public DateTime GameDate
        {
            get
            {
                return (DateTime)StartScheduleDate;
            }
        }

        /// <summary>
        ///  0: 試合情報なし？
        ///  1: 受付前
        ///  2: 5分前以前、ベットなし
        ///  3: 5分前以前、ベットあり
        ///  4: 5分前以降、ベットなし
        ///  5: 5分前以降、ベットあり
        ///  6: 試合中、ベットなし
        ///  7: 試合中、ベットあり
        ///  8: 試合終了、ベットなし
        ///  9: 試合終了、ベットあり
        /// 10: 試合中止
        /// </summary>
        public int GameStatus { get; set; }

        public string ReloadUrl { get; set; }
        public string ReloadArea { get; set; }
    }
}