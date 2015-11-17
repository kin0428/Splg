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
 * Class		: MlbTeamInfoTeamTopViewModel
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directivesusing System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
using System;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    public class MlbTeamInfoTeamTopViewModel
    {
        public int TeamID { get; set; }
        public string ShortNameLeague { get; set; }
        public string Team { get; set; }
        public int LeagueID { get; set; }
        public int Ranking { get; set; }

        private string teamIcon;
        public string TeamIcon
        {
            get
            {
                string result = "/Content/News/PN_UTF8/photo/default.png";
                if (!String.IsNullOrEmpty(teamIcon))
                {
                    if (!teamIcon.StartsWith("/") && !teamIcon.StartsWith("~"))
                        teamIcon = "/" + teamIcon;

                    return teamIcon;
                }

                return result;
            }
            set { teamIcon = value; }
        }
        public  OfficialStatsMlb OfficialStatsMlb { get; set; }

        public TeamInfo TeamInfo { get; set; }

        public List<JapanesePlayers> ExistingJapanesePlayers { get; set; }
        public List<JapanesePlayers> ExistedJapanesePlayers { get; set; }
        public List<StatLast5Years> StatLast5Years { get; set; }
        public decimal ExpectationsDeviation { get; set; }
        public decimal BetrayalDeviation { get; set; }
        public IEnumerable<PostedInfoViewModel> TeamPostList { get; set; }
    }
}