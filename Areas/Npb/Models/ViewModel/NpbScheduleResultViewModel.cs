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
 * Namespace	: Splg.Areas.Npb.Models
 * Class		: NpbScheduleResultViewModel
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Npb.Models
{
    public class NpbScheduleResultViewModel
    {
        public string GameId { get; set; }
        
        public string GameClass { get; set; }

        public int GameDate { get; set; }

        public string Time { get; set; }

        public string StadiumName { get; set; }

        public int? HomeTeamID { get; set; }

        public string HomeTeamName { get; set; }

        public string HomeIcon { get; set; }

        public int? HomeRanking { get; set; }

        public int? HomeWin { get; set; }

        public int? VisitorTeamID { get; set; }

        public string VisitorTeamName { get; set; }

        public string VisitorIcon { get; set; }

        public int? VisitorRanking { get; set; }

        public int? VisitorWin { get; set; }

        public int? HomeScore { get; set; }

        public int? VisitorScore { get; set; }

        public string InningBottomTop { get; set; }

    }
}