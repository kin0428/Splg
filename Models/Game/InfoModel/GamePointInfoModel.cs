using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Models.Game.InfoModel
{
    public class GamePointInfoModel
    {
        public long ExpectPointID { get; set; }
        public long ExpectTargetID { get; set; }
        public int SportsID { get; set; }
        public short Status { get; set; }
        public Nullable<int> FixBetSelectID { get; set; }
        public long PointID { get; set; }
        public int ExpectPoint { get; set; }
        public Nullable<int> AcquisitionPoint { get; set; }
        public Nullable<short> ClassClass { get; set; }
        public int GameID { get; set; }
        public Nullable<int> BetSelectID { get; set; }
        public Nullable<short> VorD { get; set; }
        public short SituationStatus { get; set; }
        public DateTime BetStartDate { get; set; }
        public DateTime? StartScheduleDate { get; set; }

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
        public int GiveTargetMonth { get; set; }
    }
}