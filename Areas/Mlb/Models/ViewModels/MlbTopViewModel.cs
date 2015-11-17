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
 * Namespace	: Splg.Areas.Mlb.Models.ViewModels
 * Class		: MlbTopViewModel
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directivesusing System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
using Splg.Models.Game.ViewModel;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    /// <summary>
    /// Layer class for MlbTop Page
    /// </summary>
    public class MlbTopViewModel
    {
        public IEnumerable<TeamRankingDeviation> ListTeamExpectationsDeviation { get; set; }    // 期待度ランキング
        public IEnumerable<TeamRankingDeviation> ListTeamBetrayalDeviation { get; set; }        // 裏切度ランキング
        public IEnumerable<PostedInfoViewModel> MlbPostedList { get; set; }                     // 投稿記事
        public IEnumerable<GameInfoViewModel> ListGames { get; set; }                           // 試合情報
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