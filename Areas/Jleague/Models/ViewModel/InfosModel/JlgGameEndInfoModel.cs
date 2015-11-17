using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgGameEndInfoModel
    {

        public int GameDate { get; set; }
        public int GameKindID { get; set; }
        public string GameKindName { get; set; }
        public Nullable<int> SeasonID { get; set; }
        public string SeasonName { get; set; }
        public Nullable<int> StartTime { get; set; }
        public Nullable<int> StadiumID { get; set; }
        public string StadiumName { get; set; }
        public Nullable<short> DayNight { get; set; }
        public Nullable<int> StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<int> Half { get; set; }
        public Nullable<int> SituationID { get; set; }
        public string SituationName { get; set; }
        public string Referee { get; set; }
        public string Linesman1 { get; set; }
        public string Linesman2 { get; set; }
        public string Fourth { get; set; }
        public Nullable<short> HalfEndF { get; set; }
        public Nullable<short> GameFixF { get; set; }


        public string Condition { get; set; }
        public string Surface { get; set; }
        public string Weather { get; set; }
        public string Wind { get; set; }
        public string Temperature { get; set; }
        public Nullable<int> Spectators { get; set; }
        public Nullable<int> Humidity { get; set; }
    }
}