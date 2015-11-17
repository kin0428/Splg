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
 * Class		: NpbScheduleResultController
 * Developer	: Hai Nguyen
 * Modified Developer : CucHTP
 * Modified Date : 03/31/2015
 */
#endregion

#region Using directives
using Splg.Areas.Npb.Models;
using Splg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbScheduleResultController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        NpbEntities npb = new NpbEntities();
        ComEntities com = new ComEntities();
        #endregion

        #region Index
        // GET: Npb/ScheduleResult
        public ActionResult Index()
        {
            return View();
        }
        #endregion       

        #region Old Code - Hai Nguyen
        /// <summary>
        /// Get data display in game schedule Npb
        /// Table SeasonScheduleSS,GameInfoSS,MonthGroupSS,GameClassMST
        /// Get subquery form table officialStatsHeader,OfficialStatsNpb,realGameInfoRootRGI,gameInfoRGI,scoreRGI
        /// </summary>
        /// <Author>Hai Nguyen</Author>
        /// <param name="gameId"></param>
        /// <returns>List json object NpbScheduleResultViewModel</returns>
        [HttpPost]
        public JsonResult GetDataScheduleResult(int[] gameId)
        {
            if (gameId != null)
            {
                var query = (from seasonSchedule in npb.SeasonScheduleSS
                             join monthGroup in npb.MonthGroupSS on seasonSchedule.SeasonScheduleSSId equals monthGroup.SeasonScheduleSSId
                             join gameInfo in npb.GameInfoSS on monthGroup.MonthGroupSSId equals gameInfo.MonthGroupSSId
                             join gameClassMST in npb.GameClassMST on seasonSchedule.GameTypeID equals gameClassMST.GameClassCD
                             where gameId.Contains(gameInfo.ID)
                             orderby gameClassMST.GameClass descending
                             select new NpbScheduleResultViewModel
                             {
                                 GameId = gameInfo.ID.ToString(),
                                 GameClass = gameClassMST.GameClass,
                                 GameDate = gameInfo.GameDate,
                                 StadiumName = gameInfo.StadiumName,
                                 Time = gameInfo.Time,
                                 HomeTeamID = gameInfo.HomeTeamID,
                                 HomeTeamName = gameInfo.HomeTeamName,
                                 HomeIcon = (from teamIconNpb in npb.TeamIconNpb
                                             where teamIconNpb.TeamCD == gameInfo.HomeTeamID
                                             select teamIconNpb.TeamIcon).FirstOrDefault(),
                                 HomeRanking = (from officialStatsHeader in npb.OfficialStatsHeaderNpb
                                                join officialStat in npb.OfficialStatsNpb on officialStatsHeader.OfficialStatsHeaderNpbId equals officialStat.OfficialStatsHeaderNpbId
                                                where officialStat.TeamCD == gameInfo.HomeTeamID && (officialStatsHeader.Matchday.HasValue && officialStatsHeader.Matchday.Value == gameInfo.GameDate)
                                                select (int?)officialStat.Ranking).FirstOrDefault(),
                                 HomeWin = (from officialStatsHeader in npb.OfficialStatsHeaderNpb
                                            join officialStat in npb.OfficialStatsNpb on officialStatsHeader.OfficialStatsHeaderNpbId equals officialStat.OfficialStatsHeaderNpbId
                                            where officialStat.TeamCD == gameInfo.HomeTeamID && (officialStatsHeader.Matchday.HasValue && officialStatsHeader.Matchday.Value == gameInfo.GameDate)
                                            select officialStat.Win).FirstOrDefault(),
                                 VisitorTeamID = gameInfo.VisitorTeamID,
                                 VisitorTeamName = gameInfo.VisitorTeamName,
                                 VisitorIcon = (from teamIconNpb in npb.TeamIconNpb
                                                where teamIconNpb.TeamCD == gameInfo.VisitorTeamID
                                                select teamIconNpb.TeamIcon).FirstOrDefault(),
                                 VisitorRanking = (from officialStatsHeader in npb.OfficialStatsHeaderNpb
                                                   join officialStat in npb.OfficialStatsNpb on officialStatsHeader.OfficialStatsHeaderNpbId equals officialStat.OfficialStatsHeaderNpbId
                                                   where officialStat.TeamCD == gameInfo.VisitorTeamID && (officialStatsHeader.Matchday.HasValue && officialStatsHeader.Matchday.Value == gameInfo.GameDate)
                                                   select (int?)officialStat.Ranking).FirstOrDefault(),
                                 VisitorWin = (from officialStatsHeader in npb.OfficialStatsHeaderNpb
                                               join officialStat in npb.OfficialStatsNpb on officialStatsHeader.OfficialStatsHeaderNpbId equals officialStat.OfficialStatsHeaderNpbId
                                               where officialStat.TeamCD == gameInfo.VisitorTeamID && (officialStatsHeader.Matchday.HasValue && officialStatsHeader.Matchday.Value == gameInfo.GameDate)
                                               select officialStat.Win).FirstOrDefault(),
                                 HomeScore = (from realGameInfoRootRGI in npb.RealGameInfoRootRGI
                                              join gameInfoRGI in npb.GameInfoRGI on realGameInfoRootRGI.RealGameInfoRootRGIId equals gameInfoRGI.RealGameInfoRootRGIId
                                              join scoreRGI in npb.ScoreRGI on realGameInfoRootRGI.RealGameInfoRootRGIId equals scoreRGI.RealGameInfoRootRGIId
                                              where (realGameInfoRootRGI.Matchday.HasValue && realGameInfoRootRGI.Matchday.Value == gameInfo.GameDate) && (gameInfoRGI.GameID == gameInfo.ID)
                                                && (gameInfo.HomeTeamID.HasValue && gameInfo.HomeTeamID.Value == scoreRGI.TeamCD)
                                              select scoreRGI.TotalScore).FirstOrDefault(),
                                 VisitorScore = (from realGameInfoRootRGI in npb.RealGameInfoRootRGI
                                                 join gameInfoRGI in npb.GameInfoRGI on realGameInfoRootRGI.RealGameInfoRootRGIId equals gameInfoRGI.RealGameInfoRootRGIId
                                                 join scoreRGI in npb.ScoreRGI on realGameInfoRootRGI.RealGameInfoRootRGIId equals scoreRGI.RealGameInfoRootRGIId
                                                 where (realGameInfoRootRGI.Matchday.HasValue && realGameInfoRootRGI.Matchday.Value == gameInfo.GameDate) && (gameInfoRGI.GameID == gameInfo.ID)
                                               && (gameInfo.HomeTeamID.HasValue && gameInfo.VisitorTeamID.Value == scoreRGI.TeamCD)
                                                 select scoreRGI.TotalScore).FirstOrDefault(),
                                 InningBottomTop = npb.GameInfoRGI.Where(p => p.GameID == gameInfo.ID).Select(p => (int?)p.Inning.Value + "回" + p.BottomTop).FirstOrDefault(),
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
            var query = (from seasonSchedule in npb.SeasonScheduleSS
                         join monthGroup in npb.MonthGroupSS on seasonSchedule.SeasonScheduleSSId equals monthGroup.SeasonScheduleSSId
                         join gameInfo in npb.GameInfoSS on monthGroup.MonthGroupSSId equals gameInfo.MonthGroupSSId
                         where (dateInput != 0 ? gameInfo.GameDate == dateInput : gameInfo.GameDate >= firstDateWeek && gameInfo.GameDate <= lastDateWeek)
                         select gameInfo.ID).ToList();
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
            var result = NpbCommon.GetStatusMatch(gameId, currentUser);
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
