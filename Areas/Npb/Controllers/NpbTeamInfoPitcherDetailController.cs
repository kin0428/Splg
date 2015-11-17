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
 * Class		: NpbTeamInfoPitcherDetailController
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
using Splg.Controllers;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbTeamInfoPitcherDetailController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Npb to get db.
        /// </summary>
        NpbEntities npb = new NpbEntities();
        #endregion

        #region Index
        // GET: Npb/NpbTeamInfoPitcherDetail
        public ActionResult Index(int? teamID, int? playerID)
        {
            //#1297対応
            //return RedirectToActionPermanent("Show", "NpbTeamInfoPitcherDetail", new { area = "Npb", playerID = playerID });

            NpbTeamInfoPitcherDetailViewModel pitcherInfo = new NpbTeamInfoPitcherDetailViewModel();
            if(playerID != null && teamID != null)
            {
                ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_4;
                ViewBag.PlayerID = playerID;
                ViewBag.TeamID = teamID;
                ViewBag.LeagueName = Constants.LEAGUENAME_FIRST + GetLeagueNameByTeamID(teamID.Value) + " " + Constants.LEAGUENAME_AFTER;
                ViewBag.TeamName = npb.TeamInfoMST.Where(x => x.TeamCD == teamID).Select(x => x.Team).FirstOrDefault();
                pitcherInfo.PitchingStats = GetPitchingStatsByPlayerID(teamID.Value, playerID.Value);
                pitcherInfo.ListPitchingStats6thGameInfo = GetPitchingStats6thByPlayerID(teamID.Value, playerID.Value);
                pitcherInfo.ListPitchingStatsTeamOpponentInfo = GetPitchingStatsTeamsOpponentInfoByPlayerID(teamID.Value, playerID.Value);
                pitcherInfo.ListPitchingStats3YearInfo = GetPitchingStats3YearsInfoByPlayerID(teamID.Value, playerID.Value);
                pitcherInfo.TeamPostedInfoList = PostedController.GetRecentPosts(Constants.NPB_POST_TEAM_TYPE, Constants.NPB_SPORT_ID, teamID.Value, Constants.TEAM_TOPIC_CLASSIFICATION);
            }
            return View(pitcherInfo);
        }


        public ActionResult Show(int? playerID)
        {
            NpbTeamInfoPitcherDetailViewModel pitcherInfo = new NpbTeamInfoPitcherDetailViewModel();
            if (playerID != null)
            {
                ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_4;
                ViewBag.PlayerID = playerID;

                int? teamID = (from tis in npb.TeamInfoST
                               join pis in npb.PlayerInfoST on tis.TeamInfoSTId equals pis.TeamInfoSTId
                               where pis.PlayerID == playerID
                               select tis.TeamID).FirstOrDefault();
                ViewBag.TeamID = teamID;

                if (teamID != null)
                {
                    ViewBag.LeagueName = Constants.LEAGUENAME_FIRST + GetLeagueNameByTeamID(teamID.Value) + " " + Constants.LEAGUENAME_AFTER;
                    ViewBag.TeamName = npb.TeamInfoMST.Where(x => x.TeamCD == teamID).Select(x => x.Team).FirstOrDefault();
                    pitcherInfo.PitchingStats = GetPitchingStatsByPlayerID(teamID.Value, playerID.Value);
                    pitcherInfo.ListPitchingStats6thGameInfo = GetPitchingStats6thByPlayerID(teamID.Value, playerID.Value);
                    pitcherInfo.ListPitchingStatsTeamOpponentInfo = GetPitchingStatsTeamsOpponentInfoByPlayerID(teamID.Value, playerID.Value);
                    pitcherInfo.ListPitchingStats3YearInfo = GetPitchingStats3YearsInfoByPlayerID(teamID.Value, playerID.Value);
                    pitcherInfo.TeamPostedInfoList = PostedController.GetRecentPosts(Constants.NPB_POST_TEAM_TYPE, Constants.NPB_SPORT_ID, teamID.Value, Constants.TEAM_TOPIC_CLASSIFICATION);
                }
            }
            return View(@"Index", pitcherInfo);
        }
        #endregion

        #region Get PitchingStats By PlayerID, TeamID
        /// <summary>
        /// Get pitching stats by playerID, TeamID.
        /// </summary>
        /// <param name="playerID">Player ID.</param>
        /// <param name="teamID">TeamID.</param>
        /// <returns>Pitching Info.</returns>
        public PitchingStats GetPitchingStatsByPlayerID(int teamID, int playerID)
        {
            var query = (from pitchingStats in npb.PitchingStats 
                         join pitchingStatsH in npb.PitchingStatsHeader on pitchingStats.PitchingStatsHeaderId equals pitchingStatsH.PitchingStatsHeaderId
                         join playerInfoMST in npb.PlayerInfoMST on pitchingStats.PlayerCD equals playerInfoMST.PlayerCD
                         join playerHeader in npb.PlayerInfoMSTHeader on new { k1 = playerInfoMST.PlayerInfoMSTHeaderId, k2 = (int)pitchingStatsH.TeamCD } equals new { k1 = playerHeader.PlayerInfoMSTHeaderId, k2 = playerHeader.TeamCD }
                         where pitchingStats.PlayerCD == playerID 
                         where pitchingStats.TeamCD == teamID
                         where pitchingStatsH.GameAssortment != Constants.OFFICIALSTATS_GAMEASSORTMENT_EXCHANGE_GAME 
                         select pitchingStats
                         ).FirstOrDefault();
            return query;
        }
        #endregion

        #region Get PitchingStats6th By PlayerID, TeamID.
        /// <summary>
        /// Get pitching stats 6th by playerID, TeamID.
        /// </summary>
        /// <param name="playerID">PlayerID.</param>
        /// <param name="teamID">TeamID.</param>
        /// <returns>Pitching Stats 6th Game info.</returns>
        public IEnumerable<NpbPitchingStats6thGameInfoViewModel> GetPitchingStats6thByPlayerID(int teamID, int playerID)
        {
            var query = from pi in npb.PitchingStats6thGame
                        join pis in npb.PitchingStats6thGameInfo on pi.PitchingStats6thGameId equals pis.PitchingStats6thGameId
                        join giss in npb.GameInfoSSN on pis.GameDate equals giss.DateJPN
                        where (pi.PlayerCD == playerID && pi.TeamCD == teamID) &&
                              (giss.HTeamID == teamID || giss.VTeamID == teamID)
                        orderby pis.GameDate descending
                        select new NpbPitchingStats6thGameInfoViewModel
                        {
                            PitchingStats6thGameInfo = pis,
                            GameID = giss.ID,
                            StadiumName = giss.StadiumName,
                            Time = giss.TimeJPN,
                            GameDate = giss.DateJPN,
                            HScore = giss.HScore.Value == null ? 0 : giss.HScore.Value,
                            VScore = giss.VScore.Value == null ? 0 : giss.VScore.Value
                        };
            return query;
        }
        #endregion

        #region Get LeagueName By TeamID
        /// <summary>
        /// Get league name of team ID.
        /// </summary>
        /// <param name="teamID">TeamID.</param>
        /// <returns>Short league name of TeamID.</returns>
        public string GetLeagueNameByTeamID(int teamID)
        {
            var query = npb.TeamInfoMST.Where(x=>x.TeamCD == teamID).Select(x=>x.ShortNameLeague).FirstOrDefault();
            return query;
        }
        #endregion        

        #region Get PitchingStats TeamsOpponent Info By PlayerID, TeamID
        /// <summary>
        /// Get pitchingstats TeamOpponentInfo by PlayerID and TeamID
        /// </summary>
        /// <param name="teamID">TeamID</param>
        /// <param name="playerID">PlayerID</param>
        /// <returns>List TeamOpponent Info.</returns>
        public IEnumerable<PitchingStatsTeamsImageInfo> GetPitchingStatsTeamsOpponentInfoByPlayerID(int teamID, int playerID)
        {
            var query = from pso in npb.PitchingStatsTeamsOpponent
                        join psoi in npb.PitchingStatsTeamsOpponentInfo on pso.PitchingStatsTeamsOpponentId equals psoi.PitchingStatsTeamsOpponentId
                        join ti in npb.TeamIconNpb on psoi.TeamsOpponentCD equals ti.TeamCD
                        where pso.TeamCD == teamID && pso.PlayerCD == playerID
                        select new PitchingStatsTeamsImageInfo
                        {
                            PitchingStatsTeamsInfo = psoi,
                            TeamIcon = ti.TeamIcon
                        };
            return query;
        }
        #endregion

        #region Get PitchingStats 3Years Info By PlayerID, TeamID
        /// <summary>
        /// Get pitchingstats 3 years info by TeamID, PlayerID.
        /// </summary>
        /// <param name="teamID">TeamID</param>
        /// <param name="playerID">PlayerID</param>
        /// <returns>List pitchingstats 3 years.</returns>
        public IEnumerable<PitchingStats3YearsInfo> GetPitchingStats3YearsInfoByPlayerID(int teamID, int playerID)
        {
            var query = from ps3t in npb.PitchingStats3YearsTeams
                        join ps3 in npb.PitchingStats3Years on ps3t.PitchingStats3YearsTeamsId equals ps3.PitchingStats3YearsTeamsId
                        join ps3gi in npb.PitchingStats3YearsInfo on ps3.PitchingStats3YearsId equals ps3gi.PitchingStats3YearsId
                        where ps3t.TeamID == teamID && ps3.ID == playerID
                        select ps3gi;
            return query;
        }
        #endregion
    }
}