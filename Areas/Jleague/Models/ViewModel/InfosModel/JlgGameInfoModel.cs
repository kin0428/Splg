using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    /// <summary>
    /// All info of a Game
    /// ScheduleInfo + ScheduleInfoHE + GameReportLG
    /// </summary>
    public class JlgGameInfoModel
    {

        #region ScheduleInfo

        public int ScheduleInfoId { get; set; }
        public int GameCategoryId { get; set; }
        public int ScheduleNO { get; set; }
        public int GameID { get; set; }
        public Nullable<int> GameDateScheduled { get; set; }
        public Nullable<int> GameTime { get; set; }
        public Nullable<int> LocalDate { get; set; }
        public Nullable<int> LocalTime { get; set; }
        public Nullable<int> OccasionNo { get; set; }
        public Nullable<int> Round { get; set; }
        public string RoundName { get; set; }
        public string GroupID { get; set; }
        public Nullable<short> Subject { get; set; }
        public Nullable<short> DayNight { get; set; }
        public Nullable<int> StadiumID { get; set; }
        public string StadiumName { get; set; }
        public string StadiumNameS { get; set; }
        public string Site { get; set; }
        public Nullable<int> HomeTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamNameS { get; set; }
        public string HomeTeamNameE { get; set; }
        public string HomeTeamNameES { get; set; }
        public Nullable<int> AwayTeamID { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamNameS { get; set; }
        public string AwayTeamNameE { get; set; }
        public string AwayTeamNameES { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        #endregion

        #region ScheduleInfoHE

        public int ScheduleInfoHEId { get; set; }
        public int GameCategoryHEId { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> HomeWin { get; set; }
        public Nullable<int> HomeWin90 { get; set; }
        public Nullable<int> HomeWinEX { get; set; }
        public Nullable<int> HomeWinPK { get; set; }
        public string HomeScore { get; set; }
        public Nullable<int> AwayWin { get; set; }
        public Nullable<int> AwayWin90 { get; set; }
        public Nullable<int> AwayWinEX { get; set; }
        public Nullable<int> AwayWinPK { get; set; }
        public string AwayScore { get; set; }
        public Nullable<int> Draw { get; set; }

        #endregion

        #region GameReportLG

        public int GameReportLGId { get; set; }
        public int GameDateReported { get; set; }
        public int GameKindID { get; set; }
        public string GameKindName { get; set; }
        public Nullable<int> SeasonID { get; set; }
        public string SeasonName { get; set; }
        public Nullable<int> StartTime { get; set; }
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


        #endregion
    }
}