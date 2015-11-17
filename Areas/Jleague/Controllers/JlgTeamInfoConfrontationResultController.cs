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
 * Namespace	: Splg.Areas.Jlg.Controllers
 * Class		: JlgTeamInfoController
 * Developer	: Nojima
 * Updated date : 2015-05-06
 */
#endregion

using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Splg.Core.Attributes;

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgTeamInfoConfrontationResultController : Controller
    {

        #region Global Properties
        /// <summary>
        /// Declare context Jlg to get db.
        /// </summary>
        JlgEntities jlg = new JlgEntities();
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        #endregion

        public ActionResult Index(int teamId)
        {
            List<JlgTeamInfoConfrontationResultViewModel> viewModel = null;

            ViewBag.TeamID = teamId.ToString();
            ViewBag.TeamName = jlg.TeamInfoTE.Where(x => x.TeamID == teamId).Select(x => x.TeamName).FirstOrDefault();

            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
            ViewBag.JleagueSubMenu = 6;
            ViewBag.JleagueTeamMenu = 3;
            ViewBag.TeamInfoMenuTabActive = (int)JlgConstants.TeamInfoMenu.TabActive_3;

            ViewBag.JType = jType;
            int gameKindID = 0;
            switch (jType)
            {
                case 1:
                    gameKindID = 2;
                    break;
                case 2:
                    gameKindID = 6;
                    break;
            }


            string query = "SELECT    b.teamid, " +
                            "          b.teamname, " +
                            "          b.gamekindname, " +
                            "          b.teamicon, " +
                            "          b.draw, " +
                            "          b.win, " +
                            "          b.game, " +
                            "          b.lose, " +
                            "          b.lost, " +
                            "          b.score, " +
                            "          point " +
                            "FROM      ( " +
                            "                     SELECT     c.id AS teamid, " +
                            "                                g.teamname, " +
                            "                                a.gamekindid, " +
                            "                                a.gamekindname, " +
                            "                                i.teamicon, " +
                            "                                Sum(c.draw)               AS draw, " +
                            "                                Sum(c.win)                AS win, " +
                            "                                Sum(c.game)               AS game, " +
                            "                                Sum(c.lose)               AS lose, " +
                            "                                Sum(c.lost)               AS lost, " +
                            "                                Sum(c.score)              AS score " +
                            "                     FROM       splg.jlg.teamcardreporttc AS a " +
                            "                     INNER JOIN splg.jlg.teaminfotc       AS b " +
                            "                     ON         a.teamcardreporttcid = b.teamcardreporttcid " +
                            "                     INNER JOIN splg.jlg.resultinfotc AS c " +
                            "                     ON         a.teamcardreporttcid = c.teaminfotcid " +
                            "                     INNER JOIN splg.jlg.teaminfogt AS g " +
                            "                     ON         g.teamid = c.id " +
                            "                     INNER JOIN splg.jlg.teamiconjlg AS i " +
                            "                     ON         i.teamcd = c.id " +
                            "                     WHERE      ( " +
                            "                                           a.gamekindid = 2) " +
                            "                     AND        ( " +
                            "                                           a.teamid = 30528) " +
                            "                     GROUP BY   c.id, " +
                            "                                g.teamname, " +
                            "                                a.gamekindid, " +
                            "                                a.gamekindname, " +
                            "                                i.teamicon ) b " +
                            "LEFT JOIN splg.jlg.teamstatsreportts tsrt " +
                            "ON        tsrt.teamid = b.teamid " +
                            "AND       tsrt.gamekindid = b.gamekindid " +
                            "LEFT JOIN splg.jlg.teaminfots tits " +
                            "ON        tits.TeamStatsReportTSId = tsrt.TeamStatsReportTSId ";

            viewModel = com.Database.SqlQuery<JlgTeamInfoConfrontationResultViewModel>(@query).ToList<JlgTeamInfoConfrontationResultViewModel>();

            return View(viewModel);
        }
    }
}