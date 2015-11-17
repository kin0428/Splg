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
 * Namespace	: Splg.Areas.Jleague.Models.ViewModel
 * Class		: JlgTeamInfoPlayerViewModel
 * Developer	: e-concier
 * Update date  : 2015-04-27
 */
#endregion

#region Using directives
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Jleague.Models.ViewModel
{
    /// <summary>
    /// Layer class to store news get from db.
    /// </summary>
    public class JlgTeamInfoPlayerViewModel
    {
        public IEnumerable<JlgTeamInfoPlayerInfos> TeamInfoPlayerInfosGK { get; set; }
        public IEnumerable<JlgTeamInfoPlayerInfos> TeamInfoPlayerInfosDF { get; set; }
        public IEnumerable<JlgTeamInfoPlayerInfos> TeamInfoPlayerInfosMF { get; set; }
        public IEnumerable<JlgTeamInfoPlayerInfos> TeamInfoPlayerInfosFW { get; set; }
    }
}