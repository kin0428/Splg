using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models
{
    public class ScheduleResultViewModel
    {
        public int ScheduleInfoId { get; set; }

        public int GameID { get; set; }

        public int HomeTeamID { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayTeamID { get; set; }

        public string AwayTeamName { get; set; }

        public int HomeRanking { get; set; }

        public int AwayRanking { get; set; }

        public int HomeWin { get; set; }

        public int AwayWin { get; set; }
    }
}