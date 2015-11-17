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
 * Class		: MlbScheduleResultController
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
    public class MlbScheduleResultController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        MlbEntities mlb = new MlbEntities();
        ComEntities com = new ComEntities();
        #endregion

        #region Index
        // GET: Mlb/ScheduleResult
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Old Code 
        /// <summary>
        /// Get data display in game schedule Mlb
        /// Table SeasonSchedule,GameInfo,MonthGroup,GameClassMST
        /// Get subquery form table officialStatsHeader,OfficialStatsMlb,realGameInfoRootRGI,gameInfoRGI,scoreRGI
        /// </summary>
        /// <Author>ENDO</Author>
        /// <param name="gameId"></param>
        /// <returns>List json object MlbScheduleResultViewModel</returns>
        [HttpPost]
        public JsonResult GetDataScheduleResult(int[] gameId)
        {
            if (gameId != null)
            {
                var query = (from ss in mlb.SeasonSchedule
                            join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                            join hti in mlb.TeamInfo on ss.HomeTeamID equals hti.TeamID
                            join vti in mlb.TeamInfo on ss.VisitorTeamID equals vti.TeamID
                            join hos in mlb.OfficialStatsMlb on ss.HomeTeamID equals hos.TeamID 
                            join hdg in mlb.DivGroupMlb on hos.DivGroupMlbId equals hdg.DivGroupMlbId
                            join hlg in mlb.LeagueGroupMlb on hdg.LeagueGroupMlbId equals hlg.LeagueGroupMlbId
                            join vos in mlb.OfficialStatsMlb on ss.VisitorTeamID equals vos.TeamID 
                            join vdg in mlb.DivGroupMlb on vos.DivGroupMlbId equals vdg.DivGroupMlbId
                            join vlg in mlb.LeagueGroupMlb on vdg.LeagueGroupMlbId equals vlg.LeagueGroupMlbId
                            join htim in mlb.TeamIconMlb on ss.HomeTeamID equals htim.TeamCD into htimt
                            from hi in htimt.DefaultIfEmpty()
                            join vtim in mlb.TeamIconMlb on ss.VisitorTeamID equals vtim.TeamCD into vtimt
                            from vi in vtimt.DefaultIfEmpty()
                             where gameId.Contains(ss.GameID)
//                             orderby hti.LeagueID , hti.TeamID
                             select new MlbScheduleResultViewModel
                             {
                                GameID = ss.GameID,
                                GameDate = (int)dg.GameDateJPN,
                                Time = ss.Time,
                                StadiumName = ss.StadiumName,
                                GameTypeID = (int)hti.LeagueID,
                                GameTypeName = hti.LeagueName,
                                LeagueID = (int)hti.LeagueID,
                                LeagueName = hti.LeagueName,

                                HomeTeamID = ss.HomeTeamID.Value,
                                HomeTeamName = ss.HomeTeamName != null ? ss.HomeTeamName : string.Empty,
                                HomeTeamIcon = hi.TeamIcon,
                                HomeTeamRanking = hos.Ranking != null ? hos.Ranking : 0,
                                HomeTeamWin = hos.Win.Value != null ? hos.Win.Value : 0,

                                VisitorTeamID = ss.VisitorTeamID.Value,
                                VisitorTeamName = ss.VisitorTeamName != null ? ss.VisitorTeamName : string.Empty,
                                VisitorTeamIcon = vi.TeamIcon,
                                VisitorTeamRanking = vos.Ranking != null ? vos.Ranking : 0,
                                VisitorTeamWin = vos.Win.Value != null ? vos.Win.Value : 0,
                             }).ToList();

                return Json(query, JsonRequestBehavior.AllowGet);

            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Ratio Bet by gameId and bet type
        /// betSelectID=1 home win
        /// betSelectID=2 home draw
        /// betSelectID=3 home draw
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="gameId"></param>
        /// <param name="betSelectID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRatioBet(int gameId, int betSelectID)
        {
            var expectTargetID = (from expectTarget in com.ExpectTarget
                                  where expectTarget.SportsID == 1 && expectTarget.ClassClass == 4 && expectTarget.UniqueID == gameId
                                  select expectTarget.ExpectTargetID).FirstOrDefault();
            var query = (from oddsInfo in com.OddsInfo
                         join betSelectMst in com.BetSelectMaster on oddsInfo.BetSelectMasterID equals betSelectMst.BetSelectMasterID
                         where (expectTargetID != null && expectTargetID == oddsInfo.ExpectTargetID)
                         && oddsInfo.ClassificationType == 2 && betSelectMst.BetSelectID == betSelectID && (oddsInfo.UniqueID == gameId)
                         select oddsInfo.Odds).SingleOrDefault();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get gameid
        /// </summary>
        /// <param name="dateInput"></param>
        /// <param name="firstDateWeek"></param>
        /// <param name="lastDateWeek"></param>
        /// <returns>return array gameId</returns>
        [HttpPost]
        public JsonResult GetGameID(int dateInput, int firstDateWeek, int lastDateWeek)
        {
            //dateInput = 20150309;
            //firstDateWeek = 20150309;
            //lastDateWeek = 20150310;
            var query = (from ss in mlb.SeasonSchedule
                         join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                         join mg in mlb.MonthGroup on dg.MonthGroupId equals mg.MonthGroupId
                         where (dateInput != 0 ? dg.GameDateJPN == dateInput : dg.GameDateJPN >= firstDateWeek && dg.GameDateJPN <= lastDateWeek)
                         select ss.GameID).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get status game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetStatusMatch(int gameId)
        {
            string currentUser = string.Empty;
            if (Session["CurrentUser"] != null)
            {
                currentUser = Session["CurrentUser"].ToString();
            }
            var result = MlbCommon.GetStatusMatch(gameId, currentUser);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMonthAndDayOfWeek(DateTime dateInput)
        {
            var result = Utils.GetMonthAndDayOfWeek(dateInput);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}