#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgTeamExpectedRankingViewModel
    {
        public string TeamIcon { get; set; }
        public string TeamName { get; set; }
        public decimal DeviationValue { get; set; }
        public string LeagueNameS { get; set; }
        public int Ranking { get; set; }
        public int GameKindID { get; set; }
        public int TeamID { get; set; }
    }
}