using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgScoreDetailsModel
    {

        public int? Hv { get; set; }

        public int No { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<int> Half { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamNameS { get; set; }
        public Nullable<int> GPlayerID { get; set; }
        public string GPlayerName { get; set; }
        public string GPlayerNameS { get; set; }
        public string GPlayerUni { get; set; }
        public string Body { get; set; }
        public string Operation { get; set; }
        public Nullable<int> APlayerID { get; set; }
        public string APlayerName { get; set; }
        public string APlayerNameS { get; set; }
        public string APlayerUni { get; set; }
        public string AssistKind { get; set; }
        public Nullable<short> OwnGoalF { get; set; }
        public Nullable<int> ScoreTeamID { get; set; }
        public string ScoreTeamNameS { get; set; }

    }
}