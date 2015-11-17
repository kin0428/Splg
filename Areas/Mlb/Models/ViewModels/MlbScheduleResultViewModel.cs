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
 * Class		: MlbScheduleResultViewModel
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directivesusing System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    public class MlbScheduleResultViewModel
    {
        public int GameID { get; set; }

        public int GameDate { get; set; }

        public string Time { get; set; }

        public string StadiumName { get; set; }

        public int? GameTypeID { get; set; }

        public string GameTypeName { get; set; }

        public int? LeagueID { get; set; }

        public string LeagueName { get; set; }

        public int? HomeTeamID { get; set; }

        public string HomeTeamName { get; set; }

        private string homeTeamIcon;
        public string HomeTeamIcon
        {
            get
            {
                string result = "/Content/News/PN_UTF8/photo/default.png";
                if (!String.IsNullOrEmpty(homeTeamIcon))
                {
                    if (!homeTeamIcon.StartsWith("/") && !homeTeamIcon.StartsWith("~"))
                        homeTeamIcon = "/" + homeTeamIcon;

                    return homeTeamIcon;
                }

                return result;
            }
            set { homeTeamIcon = value; }
        }

        public int? HomeTeamRanking { get; set; }

        public int? HomeTeamWin { get; set; }

        public int? VisitorTeamID { get; set; }

        public string VisitorTeamName { get; set; }

        private string vistorTeamIcon;
        public string VisitorTeamIcon
        {
            get
            {
                string result = "/Content/News/PN_UTF8/photo/default.png";
                if (!String.IsNullOrEmpty(vistorTeamIcon))
                {
                    if (!vistorTeamIcon.StartsWith("/") && !vistorTeamIcon.StartsWith("~"))
                        vistorTeamIcon = "/" + vistorTeamIcon;

                    return vistorTeamIcon;
                }

                return result;
            }
            set { vistorTeamIcon = value; }
        }

        public int? VisitorTeamRanking { get; set; }

        public int? VisitorTeamWin { get; set; }

        public int? HomeScore { get; set; }

        public int? VisitorScore { get; set; }

        public string InningBottomTop { get; set; }
    }
}