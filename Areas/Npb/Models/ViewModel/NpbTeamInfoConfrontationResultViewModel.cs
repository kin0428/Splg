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
 * Class		: NpbTeamInfoConfrontationResultViewModel
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-03-06
 */
#endregion

#region Using directives
using Splg.Areas.Npb.Models.ViewModel.InfosModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Npb.Models.ViewModel
{
    /// <summary>
    /// Layer class to store news get from db.
    /// </summary>
    public class NpbTeamInfoConfrontationResultViewModel
    {
        public NpbTeamInfoConfrontationResultInfos TeamInfo { get; set; }
        public NpbTeamInfoConfrontationResultInfos TeamsOpponent { get; set; }
        public IEnumerable<NpbTeamInfoConfrontationResultInfos> GameInfos { get; set; }
        public IEnumerable<NpbTeamInfoConfrontationResultInfos> TeamInfoMSTs { get; set; }
        public IEnumerable<NpbTeamInfoConfrontationResultInfos> TeamStatsCardDifferenceInfos { get; set; }
        public IEnumerable<NpbTeamInfoConfrontationResultInfos> ResultGameInfos { get; set; }
       
    }
}