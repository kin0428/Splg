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
 * Class		: NpbTeamInfoPlayerViewModel
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-03-12
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
    public class NpbTeamInfoPlayerViewModel
    {
        public IEnumerable<NpbTeamInfoPlayerInfos> TeamInfoPitchingInfos { get; set; }
        public IEnumerable<NpbTeamInfoPlayerInfos> TeamInfoCatcherFielderInfos { get; set; } 
        public IEnumerable<NpbTeamInfoPlayerInfos> TeamInfoDirectorStaffInfos { get; set; }              
    }
}