#region Author Information
/*
 * Namespace	: Splg.Areas.Npb.Models.ViewModel
 * Class		: NpbOrderViewModel
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class NpbOfficialStatsViewModel
    {
        public int TeamID { get; set; }
        public int GameAssortment { get; set; }
        public string TeamIcon { get; set; }
        public int Ranking { get; set; }
        public string ShortNameTeam { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Lose { get; set; }
        public Nullable<int> Draw { get; set; }
        public string WinningPercentage { get; set; }
        public string GameBehind { get; set; }
        public Nullable<int> RestGame { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class NpbOrderViewModel
    {
        public int GameAssortment { get; set; }
        public Nullable<int> Matchday { get; set; }
		
        public IEnumerable<NpbOfficialStatsViewModel> officialStatsViewModels { get; set; }
   }
}