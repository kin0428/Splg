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
 * Namespace	: Splg.Areas.Mlb.ViewModels
 * Class		: MlbTeamInfoDailyResultViewModel
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directivesusing System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    public class MlbTeamInfoDailyResultViewModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string GameId { get; set; }

        public int GameDate { get; set; }

        public int GameStatus { get; set; }

        public string StadiumNameS { get; set; }

        public string HomeTeamName { get; set; }

        public int VisitorTeamID { get; set; }

        public string VisitorTeamName { get; set; }

        public string HomeTeamIcon { get; set; }

        public string VisitorTeamIcon { get; set; }

        public string Time { get; set; }

        public int? HomeScore { get; set; }

        public int? VisitorScore { get; set; }

    }
}