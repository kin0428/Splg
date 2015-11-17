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
using Splg.Models.News.ViewModel;
#endregion

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgTopViewModel
    {
        public IEnumerable<NewsInfoViewModel> TopMostViewNews { get; set; }
        public IEnumerable<NewsInfoViewModel> TopRecentNews { get; set; }
        public IEnumerable<PostedInfoViewModel> TopRecentPost { get; set; }
        public IEnumerable<JlgTeamExpectedRankingViewModel> TopExpectationsDeviation { get; set; }
        public IEnumerable<JlgTeamExpectedRankingViewModel> TopBetrayalDeviation { get; set; }
        public int JlgGameKind { get; set; }
        public int JlgGameDate { get; set; }
        public DateTime JlgDispGameDate { set; get; }
        public int JlgFirstGameDate { get; set; }
        public int JlgLastGameDate { get; set; }
    }
    ///// <summary>
    ///// Common class for all sport : npb, mlb, jleague, ....
    ///// </summary>
    //public class TeamRankingDeviation
    //{
    //    public int TeamID { get; set; }
    //    public string TeamName { get; set; }
    //    public int Ranking { get; set; }
    //    public string LeagueName { get; set; }
    //    public string TeamIcon { get; set; }
    //    public decimal ExpectationsDeviation { get; set; }
    //    public decimal BetrayalDeviation { get; set; }
    //}

}