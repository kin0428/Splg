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
 * Class		: NpbPittingStatsInfoViewModel
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
    public class NpbPitchingStatsInfoViewModel
    {
        public string NamePlayerERA { get; set; }
        public int PlayerIDERA { get; set; }
        public string EarnedRunAverage { get; set; }
        public string NamePlayerW { get; set; }
        public int PlayerIDW { get; set; }
        public int Win { get; set; }
        public string NamePlayerSO { get; set; }
        public int PlayerIDSO { get; set; }
        public int StrikeOut { get; set; }
        public string NamePlayerS { get; set; }
        public int PlayerIDS { get; set; }
        public int Save { get; set; }
        public string NameH { get; set; }
        public int PlayerIDH { get; set; }
        public int Hold { get; set; }
             
    }
}