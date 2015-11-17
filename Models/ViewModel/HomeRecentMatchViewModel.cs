#region  Author Information
/*  
 * 
 * Createdted      : Tran Sy Huynh
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;
#endregion

namespace Splg.Models.ViewModel
{
    public class HomeRecentMatchViewModel
    {
        public int GameID { get; set; }
        public int? MatchDay { get; set; }
        public string ShortNameHomeTeam { get; set; }
        public string ShortNameVisitorTeam { get; set; }
        public int HomeTeamID { get; set; }
        public int VisitorTeamID { get; set; }
        public int? TotalHomeTeamScore { get; set; }
        public int? TotalVisitorTeamScore { get; set; }
        public string GameStateName { get; set; }
        public string GameSetSituationCD { get; set; }
        public int? Inning { get; set; }
    }
}