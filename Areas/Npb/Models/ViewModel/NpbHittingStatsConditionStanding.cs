#region Author infomation
/* 
 * Created: Tran Sy Huynh
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class NpbHittingStatsConditionStanding
    {
        public HittingStatsTeamDifferenceInfo HittingStatsTeamDifferenceInfo { get; set; }
        public string ShortNameLeague { get; set; }
        public string TeamIcon { get; set; }
    }
}