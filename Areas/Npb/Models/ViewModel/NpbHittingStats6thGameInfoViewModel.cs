#region Author infomation
/* 
 * Created: Tran Sy Huynh
 */
#endregion
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class NpbHittingStats6thGameInfoViewModel
    {
        public HittingStats6thGameInfo HittingStats6thGameInfo { get; set; }
        public int GameID { get; set; }
        public string StadiumName { get; set; }
        public string Time { get; set; }
        public int GameDate { get; set; }
        public int HScore { get; set; }
        public int VScore { get; set; }
        public short? GameResult { get; set; }
        public int? HTeamID { get; set; }
        public int? VTeamID { get; set; }
    }
}