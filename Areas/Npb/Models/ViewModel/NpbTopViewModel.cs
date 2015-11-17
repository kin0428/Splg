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
 * Namespace	: Splg.Areas.Npb.Models
 * Class		: NpbTopViewModel
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
using Splg.Models.Game.ViewModel;
#endregion

namespace Splg.Areas.Npb.Models
{
    /// <summary>
    /// Layer class for NpbTop Page
    /// </summary>
    public class NpbTopViewModel
    {
        public IEnumerable<TeamRankingDeviation> ListTeamExpectationsDeviation { get; set; }
        public IEnumerable<TeamRankingDeviation> ListTeamBetrayalDeviation { get; set; }
        public IEnumerable<PostedInfoViewModel> NpbPostedList { get; set; }
        public IEnumerable<GameInfoViewModel> ListGames { get; set; }
    }

    /// <summary>
    /// Common class for all sport : npb, mlb, jleague, ....
    /// </summary>
    public class TeamRankingDeviation
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int Ranking { get; set; }
        public string LeagueName { get; set; }
        public string TeamIcon { get; set; }
        public decimal ExpectationsDeviation { get; set; }
        public decimal BetrayalDeviation { get; set; }
    }
}