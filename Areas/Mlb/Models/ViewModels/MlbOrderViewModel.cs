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
 * Namespace	: Splg.Areas.Mlb.ViewModels
 * Class		: MlbOrderViewModel
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directivesusing System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    public class MlbOrderViewModel
    {
        public Nullable<int> GameDate { get; set; }
        public int LeagueID { get; set; }
        public int DivID { get; set; }
        public IEnumerable<MlbOfficialStatsViewModel> officialStatsViewModels { get; set; }
    }

    public class MlbOfficialStatsViewModel
    {
        public int TeamID { get; set; }
        public string TeamIcon { get; set; }
        public int Ranking { get; set; }
        public string TeamName { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Lose { get; set; }
        public string WinningPercentage { get; set; }
        public string GameBehind { get; set; }
        public string StatHome { get; set; }
        public string StatVisitor { get; set; }
        public string Streak { get; set; }
        public int LeagueID { get; set; }
        public int DivID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}