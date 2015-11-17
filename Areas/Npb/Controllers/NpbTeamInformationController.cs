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
 * Namespace	: Splg.Areas.Npb.Controllers
 * Class		: NpbTeamInformationController
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbTeamInformationController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Npb to get db.
        /// </summary>
        NpbEntities npb = new NpbEntities();       
        #endregion

        #region Index
        // GET: Npb/NpbTeamInformation
        public ActionResult Index()
        {
            IEnumerable<NpbTeamInfoViewModel> lstTeam = GetAllTeamsInLeague();
            return View(lstTeam);
        }
        #endregion

        #region Get AllTeams In League
        /// <summary>
        /// Get all teams in league and logo image.
        /// Just get league that have name, if not have name not get.
        /// </summary>
        /// <returns>List Team taked part in league.</returns>
        public IEnumerable<NpbTeamInfoViewModel> GetAllTeamsInLeague()
        {
            var query = npb.TeamInfoMST.Where(m => m.ShortNameLeague != null && m.LeagueID != 0).Select(m => m.LeagueID.Value).Distinct().ToList();

            var result = from ti in npb.TeamInfoMST
                         join ticon in npb.TeamIconNpb on ti.TeamCD equals ticon.TeamCD
                         where query.Contains(ti.LeagueID.Value)
                         select new NpbTeamInfoViewModel
                         {
                             TeamInfoMST = ti,
                             TeamIcon = ticon.TeamIcon,
                             SortOrd = ticon.SortOrd
                         };
            return result;
        }
        #endregion        
    }
}