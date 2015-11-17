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
 * Namespace	: Splg.Areas.Npb.Models.ViewModel.InfosModel
 * Class		: NpbPersonalResultInfos
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-03-06
 */
#endregion
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion
namespace Splg.Areas.Npb.Models.ViewModel.InfosModel
{
    public class NpbPersonalResultInfos
    {
        public Nullable<int> Ranking { get; set; }
        public Nullable<int> TeamCD { get; set; }
        public Nullable<int> PlayerCD { get; set; }
        public string Name { get; set; }
        public string ShortNameTeam { get; set; }
        public string BattingAverage { get; set; }
        public Nullable<int> Homerun { get; set; }
        public Nullable<int> RunsBattingIn { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Lost { get; set; }
        public Nullable<int> Strikeout { get; set; }
        public string EarnedRunAverage { get; set; }
        public Nullable<int> InningsPitched { get; set; }
        public Nullable<int> InningsPitched3rd { get; set; }
        public Nullable<int> Save { get; set; }
        public Nullable<int> GamePitched { get; set; }
        public string Display { get; set; }       
    }
}