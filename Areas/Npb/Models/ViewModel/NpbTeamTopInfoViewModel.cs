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
 * Class		: NpbTeamTopInfoViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
#endregion

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class NpbTeamTopInfoViewModel
    {
        public IEnumerable<HittingStats> HittingRanking { get; set; }
        public NpbPitchingStatsInfoViewModel PitchingRanking { get; set; }
        public int TeamID { get; set; }
        public string ShortNameLeague { get; set; }
        public string Team { get; set; }
        public int LeagueID { get; set; }
        public int Ranking { get; set; }
        public string TeamIcon { get; set; }
        public decimal ExpectationsDeviation { get; set; }
        public decimal BetrayalDeviation { get; set; }
        public IEnumerable<PostedInfoViewModel> TeamPostList { get; set; }
    }
}