using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgOfficialStatsModel
    {
        public int Ranking { get; set; }
        public int TeamCD { get; set; }
        public string ShortNameTeam { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Lose { get; set; }
        public Nullable<int> Draw { get; set; }
        public string WinningPercentage { get; set; }
        public string GameBehind { get; set; }
        public Nullable<int> Run { get; set; }
        public Nullable<int> PointLost { get; set; }
        public Nullable<int> Homerun { get; set; }
        public Nullable<int> StolenBase { get; set; }
        public string BattingAverage { get; set; }
        public string EarnedRunAverage { get; set; }
        public Nullable<int> RestGame { get; set; }
        public Nullable<int> WinnerMagic { get; set; }
        public Nullable<int> POLastRanking { get; set; }
        public Nullable<short> RankShiftF { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public long OfficialStatsNpbId { get; set; }
        public long OfficialStatsHeaderNpbId { get; set; }
    }
}