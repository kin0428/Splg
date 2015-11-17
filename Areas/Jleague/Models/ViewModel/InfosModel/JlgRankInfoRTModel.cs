using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Splg.Areas.Jleague.Models.ViewModel.InfosModel;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgRankInfoRTModel
    {
        public int RankInfoRTId { get; set; }
        public int RankReportRTId { get; set; }
        public int Ranking { get; set; }
        public int Replace { get; set; }
        public string GroupID { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamNameS { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> Point { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Win90 { get; set; }
        public Nullable<int> WinEX { get; set; }
        public Nullable<int> WinPK { get; set; }
        public Nullable<int> Lose { get; set; }
        public Nullable<int> Lose90 { get; set; }
        public Nullable<int> LoseEX { get; set; }
        public Nullable<int> LosePK { get; set; }
        public Nullable<int> Draw { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> Lost { get; set; }
        public Nullable<int> Differ { get; set; }
        public Nullable<int> PromotionF { get; set; }
        public Nullable<int> PlayoffF { get; set; }
        public Nullable<int> DemotionF2 { get; set; }
        public Nullable<int> PromotionF2 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}