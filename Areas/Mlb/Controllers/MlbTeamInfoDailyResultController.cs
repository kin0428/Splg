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
 * Class		: MlbTeamInfoDailyResultController
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directives
using Splg.Areas.Mlb;
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
    public class MlbTeamInfoDailyResultController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        MlbEntities mlb = new MlbEntities();
        #endregion
        
        // GET: Mlb/MlbTeamInfoDailyResult
        public ActionResult Index(int teamId)
        {
            var teamName = (from t in mlb.TeamInfo
                          where t.TeamID == teamId
                          select t.TeamName).FirstOrDefault();
            ViewBag.TeamName = teamName;
            ViewBag.TeamId = teamId;
            ViewBag.MonthOfGameDate = GetMonthOfGameDate(teamId);
            ViewBag.TeamInfoMenuTabActive = (int)MlbConstants.TeamInfoMenu.TabActive_2;
            return View();
        }

        /// <summary>
        /// Get month of gamedate by current year
        /// </summary>
        /// <returns>list month</returns>
        public List<string> GetMonthOfGameDate(int teamId)
        {
            MlbEntities Mlb = new MlbEntities();
            var query = (from seasonSchedule in mlb.SeasonSchedule
                         join dayGroup in mlb.DayGroup on seasonSchedule.DayGroupId equals dayGroup.DayGroupId
                         where (dayGroup.GameDateJPN / 10000) == DateTime.Now.Year && (seasonSchedule.HomeTeamID == teamId || seasonSchedule.VisitorTeamID == teamId)
                         select dayGroup.GameDateJPN.ToString().Substring(4, 2)).Distinct().ToList();
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="teamID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDataTeamInfoDailyResult(int year, int month, int teamId)
        {
            var query = (from SeasonSchedule in mlb.SeasonSchedule
                         join DayGroup in mlb.DayGroup on SeasonSchedule.DayGroupId equals DayGroup.DayGroupId
                         join MonthGroup in mlb.MonthGroup on DayGroup.MonthGroupId equals MonthGroup.MonthGroupId
                         join SeasonScheduleHeader in mlb.SeasonScheduleHeader on MonthGroup.SeasonScheduleHeaderId equals SeasonScheduleHeader.SeasonScheduleHeaderId
                         join teamIconMST in mlb.TeamIconMlb on SeasonSchedule.VisitorTeamID equals teamIconMST.TeamCD
                         into teamIcon from j in teamIcon.DefaultIfEmpty()
                         where (SeasonScheduleHeader.Year == year && MonthGroup.Month == month && (SeasonSchedule.HomeTeamID == teamId))
                         select new MlbTeamInfoDailyResultViewModel
                         {
                             Year = (int)SeasonScheduleHeader.Year,
                             Month = MonthGroup.Month,
                             GameId = SeasonSchedule.GameID.ToString(),
                             StadiumNameS = SeasonSchedule.StadiumName,
                             GameDate = (int)DayGroup.GameDateJPN,
                             HomeTeamName = (from teamInfo in mlb.TeamInfo 
                                             where (teamInfo.TeamID == SeasonSchedule.HomeTeamID)
                                             select teamInfo.TeamName).FirstOrDefault(),
                             VisitorTeamID = SeasonSchedule.VisitorTeamID.HasValue ? SeasonSchedule.VisitorTeamID.Value : 0,
                             VisitorTeamName = SeasonSchedule.VisitorTeamName,
                             HomeTeamIcon =(from teamIconMlb in mlb.TeamIconMlb
                                            where (SeasonSchedule.HomeTeamID.HasValue && teamIconMlb.TeamCD == SeasonSchedule.HomeTeamID.Value) 
                                            select teamIconMlb.TeamIcon).FirstOrDefault(),
                             VisitorTeamIcon = (from teamIconMlb in mlb.TeamIconMlb
                                                where (SeasonSchedule.VisitorTeamID.HasValue && teamIconMlb.TeamCD == SeasonSchedule.VisitorTeamID.Value)
                                             select teamIconMlb.TeamIcon).FirstOrDefault(),
                             Time = SeasonSchedule.Time,
                             HomeScore = (from RealGame in mlb.RealGameInfo
                                          where (RealGame.GameID == SeasonSchedule.GameID)
                                          select RealGame.HomeScore).FirstOrDefault(),
                             VisitorScore = (from RealGame in mlb.RealGameInfo
                                             where (RealGame.GameID == SeasonSchedule.GameID)
                                             select RealGame.VisitorScore).FirstOrDefault(),
                         }).Union
                         (from SeasonSchedule in mlb.SeasonSchedule
                         join DayGroup in mlb.DayGroup on SeasonSchedule.DayGroupId equals DayGroup.DayGroupId
                         join MonthGroup in mlb.MonthGroup on DayGroup.MonthGroupId equals MonthGroup.MonthGroupId
                         join SeasonScheduleHeader in mlb.SeasonScheduleHeader on MonthGroup.SeasonScheduleHeaderId equals SeasonScheduleHeader.SeasonScheduleHeaderId
                         join teamIconMST in mlb.TeamIconMlb on SeasonSchedule.VisitorTeamID equals teamIconMST.TeamCD
                         into teamIcon from j in teamIcon.DefaultIfEmpty()
                         where (SeasonScheduleHeader.Year == year && MonthGroup.Month == month && (SeasonSchedule.VisitorTeamID == teamId))
                         select new MlbTeamInfoDailyResultViewModel
                         {
                             Year = (int)SeasonScheduleHeader.Year,
                             Month = MonthGroup.Month,
                             GameId = SeasonSchedule.GameID.ToString(),
                             StadiumNameS = SeasonSchedule.StadiumName,
                             GameDate = (int)DayGroup.GameDateJPN,
                             HomeTeamName = (from teamInfo in mlb.TeamInfo 
                                             where (teamInfo.TeamID == SeasonSchedule.VisitorTeamID)
                                             select teamInfo.TeamName).FirstOrDefault(),
                             VisitorTeamID = SeasonSchedule.VisitorTeamID.HasValue ? SeasonSchedule.HomeTeamID.Value : 0,
                             VisitorTeamName = SeasonSchedule.HomeTeamName,
                             HomeTeamIcon =(from teamIconMlb in mlb.TeamIconMlb
                                            where (SeasonSchedule.HomeTeamID.HasValue && teamIconMlb.TeamCD == SeasonSchedule.VisitorTeamID.Value) 
                                            select teamIconMlb.TeamIcon).FirstOrDefault(),
                             VisitorTeamIcon = (from teamIconMlb in mlb.TeamIconMlb
                                                where (SeasonSchedule.VisitorTeamID.HasValue && teamIconMlb.TeamCD == SeasonSchedule.HomeTeamID.Value)
                                             select teamIconMlb.TeamIcon).FirstOrDefault(),
                             Time = SeasonSchedule.Time,
                             HomeScore = (from RealGame in mlb.RealGameInfo
                                          where (RealGame.GameID == SeasonSchedule.GameID)
                                          select RealGame.VisitorScore).FirstOrDefault(),
                             VisitorScore = (from RealGame in mlb.RealGameInfo
                                             where (RealGame.GameID == SeasonSchedule.GameID)
                                             select RealGame.HomeScore).FirstOrDefault(),
                         });

            // ソートをかける
            query.OrderBy(s=>s.GameDate);

            // リストに格納
            query.ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="gameId"></param>
        /// <param name="teamID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTeamWind(int date, int gameId, int teamId)
        {
            var query = from realGameInfo in mlb.RealGameInfo
                        join seasonSchedule in mlb.SeasonSchedule on realGameInfo.GameID equals seasonSchedule.GameID 
                        join dayGroup in mlb.DayGroup on seasonSchedule.DayGroupId equals dayGroup.DayGroupId 
                        where (dayGroup.GameDateJPN.Value == date) && (realGameInfo.GameID == gameId && realGameInfo.HomeTeamID == teamId)
                        select new
                        {
                            WinTeamCD = realGameInfo.HomeScore > realGameInfo.VisitorScore ? realGameInfo.HomeTeamID : (realGameInfo.HomeScore < realGameInfo.VisitorScore ? realGameInfo.VisitorTeamID : null),
                            WinTeamName = realGameInfo.HomeScore > realGameInfo.VisitorScore ? realGameInfo.HomeTeamName : (realGameInfo.HomeScore < realGameInfo.VisitorScore ? realGameInfo.VisitorTeamName : null)
                        };
            return Json(query, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetStatusMatch(string gameId)
        {
            var result = MlbCommon.GetStatusMatch(gameId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

