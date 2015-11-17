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
 * Class		: NpbPitchingStats6thGameInfoViewModel
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
    public class NpbPitchingStats6thGameInfoViewModel
    {
        public PitchingStats6thGameInfo PitchingStats6thGameInfo { get; set; }
        public int GameID { get; set; }
        public string StadiumName { get; set; }
        public string Time { get; set; }
        public int GameDate { get; set; }
        public int HScore { get; set; }
        public int VScore { get; set; }
    }
}