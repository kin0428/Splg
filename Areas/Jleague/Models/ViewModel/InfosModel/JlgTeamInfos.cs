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
 * Namespace	: Splg.Areas.Jleague.Models.ViewModel.InfosModel
 * Class		: JlgTeamInfos
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-04-16
 */
#endregion
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion
namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    /// <summary>
    /// Infos Model using at JlgResultViewModel
    /// </summary>
    public class JlgTeamInfos
    {   
        public int TeamID { get; set; }
        public int TeamNo { get; set; }       
        public string TeamName { get; set; }
        public string TeamNameS { get; set; }
        public string TeamIcon { get; set; }
    }
}