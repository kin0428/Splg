#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Areas.Npb.Models.ViewModel
 * Class		: NpbTeamInfoPitcherDetailViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
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
    public class NpbTeamInfoPitcherDetailViewModel
    {
        public PitchingStats PitchingStats { get; set; }
        public IEnumerable<NpbPitchingStats6thGameInfoViewModel> ListPitchingStats6thGameInfo { get; set; }
        public IEnumerable<PitchingStatsTeamsImageInfo> ListPitchingStatsTeamOpponentInfo { get; set; }
        public IEnumerable<PitchingStats3YearsInfo> ListPitchingStats3YearInfo { get; set; }
        public IEnumerable<PostedInfoViewModel> TeamPostedInfoList { get; set; }
    }

    public class PitchingStatsTeamsImageInfo
    {
        public PitchingStatsTeamsOpponentInfo PitchingStatsTeamsInfo { get; set; }
        public string TeamIcon { get; set; }
    }
}