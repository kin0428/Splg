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
 * Class		: NpbTeamInfDailyResult
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion
#region Using directives
using Splg.Areas.Npb.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Npb.Models;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbTeamInfoDailyResultController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        NpbEntities npb = new NpbEntities();
        #endregion

        // GET: Npb/NpbTeamInfoDailyResult

        public ActionResult Index(int teamId)
        {
            ///Begin
            ///Edit: Nguyen Minh Kiet
            ///Set active menu team infor  _NpbTeamInfoMenu.cshtml
            ///Update Date : 2015-03-20 
            ///
            ViewBag.TeamId = teamId;
            ViewBag.TeamID = teamId;
            ViewBag.MonthOfGameDate = GetMonthOfGameDate(teamId);
            ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_2;
            ViewBag.TeamName = npb.TeamInfoMST.FirstOrDefault(a => a.TeamCD == teamId).Team;
            ///
            ///End KietNM
            ///
            return View();
        }

        /// <summary>
        /// Get month of gamedate by current year
        /// </summary>
        /// <returns>list month</returns>
        public List<string> GetMonthOfGameDate(int teamId)
        {
            NpbEntities npb = new NpbEntities();
            var query = (from gameInfoSS in npb.GameInfoSS
                         where (gameInfoSS.GameDate / 10000) == DateTime.Now.Year && (gameInfoSS.HomeTeamID == teamId || gameInfoSS.VisitorTeamID == teamId)
                         select gameInfoSS.GameDate.ToString().Substring(4, 2)).Distinct().ToList();
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
            var query = (from seasonSchedule in npb.SeasonScheduleSS
                         join monthGroup in npb.MonthGroupSS on seasonSchedule.SeasonScheduleSSId equals monthGroup.SeasonScheduleSSId
                         join gameInfo in npb.GameInfoSS on monthGroup.MonthGroupSSId equals gameInfo.MonthGroupSSId
                         //join teamIconMST in npb.TeamIconNpb on gameInfo.VisitorTeamID equals teamIconMST.TeamCD
                         //into teamIcon
                         //from j in teamIcon.DefaultIfEmpty()
                         where (seasonSchedule.Year == year && monthGroup.No == month)
                         && (gameInfo.HomeTeamID.HasValue && gameInfo.HomeTeamID.Value == teamId || gameInfo.VisitorTeamID.HasValue && gameInfo.VisitorTeamID.Value == teamId)
                         select new NpbTeamInfoDailyResultViewModel
                         {
                             Year = seasonSchedule.Year,
                             No = monthGroup.No,
                             GameId = gameInfo.ID.ToString(),
                             StadiumNameS = gameInfo.StadiumNameS,
                             GameDate = gameInfo.GameDate,
                             TeamID = gameInfo.HomeTeamID == teamId ? gameInfo.VisitorTeamID : gameInfo.HomeTeamID,
                             TeamIcon = (from teamIconNpb in npb.TeamIconNpb
                                         where gameInfo.HomeTeamID == teamId ? (gameInfo.VisitorTeamID.HasValue && teamIconNpb.TeamCD == gameInfo.VisitorTeamID.Value) : (gameInfo.HomeTeamID.HasValue && teamIconNpb.TeamCD == gameInfo.HomeTeamID.Value)
                                         select teamIconNpb.TeamIcon).FirstOrDefault(),
                             TeamNameS = gameInfo.HomeTeamID == teamId ? gameInfo.VisitorTeamNameS : gameInfo.HomeTeamNameS,
                             Time = gameInfo.Time,

                             HomeScore = (from realGameInfoRootRGIs in npb.RealGameInfoRootRGI
                                          join scoreRGIs in npb.ScoreRGI on realGameInfoRootRGIs.RealGameInfoRootRGIId equals scoreRGIs.RealGameInfoRootRGIId
                                          where (realGameInfoRootRGIs.Matchday.HasValue && realGameInfoRootRGIs.Matchday.Value == gameInfo.GameDate) && (scoreRGIs.TeamCD == teamId)
                                          select scoreRGIs).FirstOrDefault().TotalScore,

                             VisitorScore = (from aa in npb.RealGameInfoRootRGI
                                             join bb in npb.ScoreRGI on aa.RealGameInfoRootRGIId equals bb.RealGameInfoRootRGIId
                                             where (aa.Matchday.HasValue && aa.Matchday.Value == gameInfo.GameDate)
                                             && (gameInfo.HomeTeamID == teamId ? (gameInfo.VisitorTeamID.HasValue && bb.TeamCD == gameInfo.VisitorTeamID.Value) : (gameInfo.HomeTeamID.HasValue && bb.TeamCD == gameInfo.HomeTeamID.Value))
                                             select bb).FirstOrDefault().TotalScore,

                             PitcherID = (from gameInfoPSP in npb.GameInfoPSP
                                          join teamInfoPSP in npb.TeamInfoPSP on gameInfoPSP.GameInfoPSPId equals teamInfoPSP.GameInfoPSPId
                                          where gameInfoPSP.GameID == gameInfo.ID
                                          && (gameInfo.HomeTeamID == teamId ? (gameInfo.VisitorTeamID.HasValue && teamInfoPSP.ID == gameInfo.VisitorTeamID.Value) : (gameInfo.HomeTeamID.HasValue && gameInfo.HomeTeamID.Value == teamInfoPSP.ID))
                                          select teamInfoPSP).FirstOrDefault().PitcherID,


                             PitcherNameS = (from gameInfoPSP in npb.GameInfoPSP
                                             join teamInfoPSP in npb.TeamInfoPSP on gameInfoPSP.GameInfoPSPId equals teamInfoPSP.GameInfoPSPId
                                             where gameInfoPSP.GameID == gameInfo.ID
                                             && (gameInfo.HomeTeamID == teamId ? (gameInfo.VisitorTeamID.HasValue && teamInfoPSP.ID == gameInfo.VisitorTeamID.Value) : (gameInfo.HomeTeamID.HasValue && gameInfo.HomeTeamID.Value == teamInfoPSP.ID))
                                             select teamInfoPSP).FirstOrDefault().PitcherNameS,

                             StartPosition = (from startingGameSTs in npb.StartingGameST
                                              join teamInfoSTs in npb.TeamInfoST on startingGameSTs.StartingGameSTId equals teamInfoSTs.StartingGameSTId
                                              join playerInfoST in npb.PlayerInfoST on teamInfoSTs.TeamInfoSTId equals playerInfoST.TeamInfoSTId
                                              where startingGameSTs.GameID == gameInfo.ID
                                              && (gameInfo.HomeTeamID == teamId ? (gameInfo.VisitorTeamID.HasValue && teamInfoSTs.TeamID == gameInfo.VisitorTeamID.Value) : (gameInfo.HomeTeamID.HasValue && gameInfo.HomeTeamID.Value == teamInfoSTs.TeamID))
                                              select playerInfoST.StartPosition).FirstOrDefault(),

                             PlayerID = (from startingGameSTs in npb.StartingGameST
                                         join teamInfoSTs in npb.TeamInfoST on startingGameSTs.StartingGameSTId equals teamInfoSTs.StartingGameSTId
                                         join playerInfoST in npb.PlayerInfoST on teamInfoSTs.TeamInfoSTId equals playerInfoST.TeamInfoSTId
                                         where startingGameSTs.GameID == gameInfo.ID
                                          && (gameInfo.HomeTeamID == teamId ? (gameInfo.VisitorTeamID.HasValue && teamInfoSTs.TeamID == gameInfo.VisitorTeamID.Value) : (gameInfo.HomeTeamID.HasValue && gameInfo.HomeTeamID.Value == teamInfoSTs.TeamID))
                                         select playerInfoST.PlayerID).FirstOrDefault(),

                             PlayerNameS = (from startingGameSTs in npb.StartingGameST
                                            join teamInfoSTs in npb.TeamInfoST on startingGameSTs.StartingGameSTId equals teamInfoSTs.StartingGameSTId
                                            join playerInfoST in npb.PlayerInfoST on teamInfoSTs.TeamInfoSTId equals playerInfoST.TeamInfoSTId
                                            where startingGameSTs.GameID == gameInfo.ID
                                             && (gameInfo.HomeTeamID == teamId ? (gameInfo.VisitorTeamID.HasValue && teamInfoSTs.TeamID == gameInfo.VisitorTeamID.Value) : (gameInfo.HomeTeamID.HasValue && gameInfo.HomeTeamID.Value == teamInfoSTs.TeamID))
                                            select playerInfoST.PlayerNameS).FirstOrDefault(),
                         }).ToList();
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
            var query = from realGameInfoRootRGI in npb.RealGameInfoRootRGI
                        join gameInfoRGI in npb.GameInfoRGI on realGameInfoRootRGI.RealGameInfoRootRGIId equals gameInfoRGI.RealGameInfoRootRGIId
                        join gameResultInfoRGI in npb.GameResultInfoRGI on realGameInfoRootRGI.RealGameInfoRootRGIId equals gameResultInfoRGI.RealGameInfoRootRGIId
                        join losingPitcherRGI in npb.LosingPitcherRGI on gameResultInfoRGI.GameResultInfoRGIId equals losingPitcherRGI.GameResultInfoRGIId
                        join winningPitcherRGI in npb.WinningPitcherRGI on gameResultInfoRGI.GameResultInfoRGIId equals winningPitcherRGI.GameResultInfoRGIId
                        join savingPitcherRGI in npb.SavingPitcherRGI on gameResultInfoRGI.GameResultInfoRGIId equals savingPitcherRGI.GameResultInfoRGIId
                        into temp
                        from j in temp.DefaultIfEmpty()
                        where (realGameInfoRootRGI.Matchday.HasValue && realGameInfoRootRGI.Matchday.Value == date)
                        && (gameInfoRGI.GameID == gameId)
                        //&& (gameInfoRGI.HomeTeam == teamId || gameInfoRGI.VisitorTeam.HasValue && gameInfoRGI.VisitorTeam.Value==teamId)
                        select new
                        {
                            WinTeamCD = gameResultInfoRGI.WinTeamCD,
                            GameResultInfoRGIId = gameResultInfoRGI.GameResultInfoRGIId,
                            PlayerCD = gameResultInfoRGI.WinTeamCD == teamId ? winningPitcherRGI.PlayerCD : losingPitcherRGI.PlayerCD,
                            ShortNameWinTeam = gameResultInfoRGI.WinTeamCD == teamId ? winningPitcherRGI.ShortNamePlayer : losingPitcherRGI.ShortNamePlayer,
                            PlayerID = j.PlayerCD != null ? j.PlayerCD : 0,
                            ShortNamePlayer = j.ShortNamePlayer != null ? j.ShortNamePlayer : string.Empty
                        };
            return Json(query, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetStatusMatch(string gameId)
        {
            var result = NpbCommon.GetStatusMatch(gameId, type: 1);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

