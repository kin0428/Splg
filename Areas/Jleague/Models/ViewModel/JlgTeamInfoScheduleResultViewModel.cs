using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgTeamInfoScheduleResultViewModel
    {
        public int GameID { get; set; }
        public string StadiumName { get; set; }
        public int HomeTeamID { get; set; }

        public string HomeIcon{ get; set; }

        public int AwayTeamID { get; set; }

        public string AwayTeamName { get; set; }

        public int GameDate { get; set; }

        public int GameTime { get; set; }

        public string GroupID { get; set; }

        public int OccasionNo { get; set; }

        public int SeasonID { get; set; }

        public string GameResult { get; set; }

        public string ScoreLose { get; set; }
        
    }
}