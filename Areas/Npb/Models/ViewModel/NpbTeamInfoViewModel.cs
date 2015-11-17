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
 * Class		: NpbTeamInfoViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
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
    /// <summary>
    /// Layer class to store teaminfo and logo image from db
    /// </summary>
    public class NpbTeamInfoViewModel
    {
        public TeamInfoMST TeamInfoMST { get; set; }
        public string TeamIcon { get; set; }
        public int SortOrd { get; set; }
    }
}