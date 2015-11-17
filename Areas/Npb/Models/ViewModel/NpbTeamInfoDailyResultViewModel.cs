using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class NpbTeamInfoDailyResultViewModel
    {
        public int Year { get; set; }

        public int No { get; set; }

        public string GameId { get; set; }

        public int GameDate { get; set; }

        public int GameStatus { get; set; }

        public string StadiumNameS { get; set; }

        public int? TeamID { get; set; }

        public string TeamIcon { get; set; }

        public string TeamNameS { get; set; }

        public string Time { get; set; }

        public int? HomeScore { get; set; }

        public int? VisitorScore { get; set; }

        public int? PitcherID { get; set; }

        public string PitcherNameS { get; set; }

        public int? StartPosition { get; set; }

        public int PlayerID { get; set; }
        public string PlayerNameS { get; set; }

    }
}