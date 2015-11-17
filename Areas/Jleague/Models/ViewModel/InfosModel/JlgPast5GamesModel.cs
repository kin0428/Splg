using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgPast5GamesModel
    {
        public int Gameid { get; set; }

        public int? HomeTeamid { get; set; }        

        public string HomeTeamnames { get; set; }

        public int? AwayTeamid { get; set; }

        public string AwayTeamnames { get; set; }

        public string HomeScore { get; set; }

        public string AwayScore { get; set; }

        public decimal? Win { get; set; }

        public decimal? Lose { get; set; }

        public decimal? Draw { get; set; }

        public int GameDate{ get; set; }
    }
}