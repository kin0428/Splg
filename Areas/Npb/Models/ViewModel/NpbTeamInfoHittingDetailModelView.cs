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
using Splg.Models.News.ViewModel;
#endregion

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class NpbTeamInfoHittingDetailModelView
    {
        public IEnumerable<NpbHittingStats6thGameInfoViewModel> ListHittingStats6thGameInfo { get; set; }
        public IEnumerable<PostedInfoViewModel> TeamPostedInfoList { get; set; }
        public IEnumerable<NpbHittingStatsConditionStanding> HittingStatsConditionStandingList { get; set; }
        public IEnumerable<HittingStats3YearsInfo> HittingStats3YearsInfoList { get; set; }
    }
}