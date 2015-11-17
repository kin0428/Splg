using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Mlb.Models.ViewModels.InfosModel
{
    public class MlbGameInfos
    {

        //SeasonSchedule
        public long SeasonScheduleId { get; set; }
        public long DayGroupId { get; set; }
        public int SeqNo { get; set; }
        public int SeasonID { get; set; }
        public int GameID { get; set; }
        public string Time { get; set; }
        public string StadiumName { get; set; }
        public Nullable<int> HomeTeamID { get; set; }
        public string HomeTeamFullName { get; set; }
        public string HomeTeamName { get; set; }
        public Nullable<int> VisitorTeamID { get; set; }
        public string VisitorTeamFullName { get; set; }
        public string VisitorTeamName { get; set; }
        public long MonthGroupId { get; set; }
        public Nullable<int> GameDate { get; set; }
        public Nullable<int> GameDateJPN { get; set; }
    }
}