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
 * Namespace	: Splg.Areas.Mlb.Controllers
 * Class		: MlbTeamInformationController
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directives
using Splg.Areas.Mlb.Models;
using Splg.Areas.Mlb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
#endregion

namespace Splg.Areas.Mlb.Controllers
{
    public class MlbTeamInformationController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Npb to get db.
        /// </summary>
        MlbEntities mlb = new MlbEntities();
        #endregion

        #region Index
        // GET: Npb/NpbTeamInformation
        public ActionResult Index()
        {
            IEnumerable<MlbTeamInformationViewModel> lstTeam = GetAllTeamsInLeague();

            return View(lstTeam);
        }
        #endregion


        #region Get AllTeams In League
        /// <summary>
        /// Get all teams in league and logo image.
        /// Just get league that have name, if not have name not get.
        /// </summary>
        /// <returns>List Team taked part in league.</returns>
        public IEnumerable<MlbTeamInformationViewModel> GetAllTeamsInLeague()
        {
            var result = (from ti in mlb.TeamInfo
                          join jpl in mlb.JapanesePlayers on ti.TeamInfoId equals jpl.TeamInfoId into jplt
                          from ticon in mlb.TeamIconMlb.Where(x => x.TeamCD == ti.TeamID).DefaultIfEmpty()
                          orderby ti.LeagueID, ticon.SortOrd
                          select new MlbTeamInformationViewModel
                         {
                             TeamInfo = ti,
                             JapanesePlayers = jplt,
                             TeamIcon = ticon.TeamIcon,
                         });
            
            return result;
        }
        #endregion   
    
    }
}