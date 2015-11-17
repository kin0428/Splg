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
 * Class		: JlgTeamInfoViewModel
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-04-02
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
    public class JlgTeamInfoViewModel
    {
        /// <summary>
        /// チームID
        /// </summary>
        public Nullable<int> TeamID { get; set; }

        /// <summary>
        /// 出力＆ジャンプ
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 出力
        /// </summary>
        public string TeamIcon { get; set; }

    }
}