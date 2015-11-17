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
 * Class		: JlgResultViewModel
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-04-17
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
    /// View mode using at Controllers JlgResultViewModel
    /// </summary>
    public class JlgResultViewModel
    {
        public JlgTeamInfos JlgResultTeamInfo { get; set; }  
        public IEnumerable<JlgTeamInfos> JlgResultTeamInfos { get; set; }  
        public IEnumerable<JlgGameInfos> JlgResultGameInfos { get; set; }
        public IEnumerable<JlgGameInfos> JlgResultGameInfosForMobile { get; set; }
        public int CurrentSeasonID { get; set; }
    }
}