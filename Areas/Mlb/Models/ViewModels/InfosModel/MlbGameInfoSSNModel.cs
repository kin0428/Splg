using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Mlb.Models.ViewModels.InfosModel
{
    public class MlbGameInfoSSNModel
    {
        public long GameInfoSSNId { get; set; }
        public long MonthGroupSSNId { get; set; }
        public int SeqNo { get; set; }
        public int ID { get; set; }
        public int GameKindID { get; set; }
        public int DateJPN { get; set; }
        public string TimeJPN { get; set; }
        public Nullable<int> WeekDayJPN { get; set; }
        public Nullable<int> StadiumID { get; set; }
        public string StadiumName { get; set; }
        public Nullable<int> Round { get; set; }
        public Nullable<short> DhF { get; set; }
        public Nullable<short> GameState { get; set; }

        public string NpbGameSituationID
        {
            get
            {
                return MlbCommon.GetNpbGameSetSituationCode(this.GameState);
            }
        }
        public Nullable<short> GameResult { get; set; }
        public Nullable<int> HScore { get; set; }
        public Nullable<int> VScore { get; set; }
        public Nullable<int> HTeamID { get; set; }
        public string HTeamNameS { get; set; }
        public Nullable<int> VTeamID { get; set; }
        public string VTeamNameS { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}