using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgLast5GamesModel
    {
        public int GameDate { get; set; }

        public int HomeTeamid { get; set; }

        public string HomeTeamnames { get; set; }

        public int AwayTeamid { get; set; }

        public string AwayTeamnames { get; set; }

        public Nullable<int> Score { get; set; }

        public Nullable<int> Lose { get; set; }

        public string GameResult { get; set; }

        public string StadiumNameS { get; set; }

    }
}