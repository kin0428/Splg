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
 * Class		: NpbTeamInfoTeamTopController
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
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbTeamInfoTeamTopController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Npb to get db.
        /// </summary>
        NpbEntities npb = new NpbEntities();
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        #endregion

        #region Index
        // GET: Npb/NpbTeamInfoTeamTop
        public ActionResult Index(string teamID)
        {
            if (!string.IsNullOrEmpty(teamID))
            {
                NpbTeamTopInfoViewModel teamTopInfo = new NpbTeamTopInfoViewModel();
                var teamCD = 0;
                if (Int32.TryParse(teamID, out teamCD))
                {
                    teamTopInfo = GetTeamInfo(teamCD);
                    //TeamID not exist in db, not show.
                    if(teamTopInfo != null)
                    {                       
                        ///Begin
                        ///Edit: Nguyen Minh Kiet
                        ///Set active menu team infor  _NpbTeamInfoMenu.cshtml
                        ///Update Date : 2015-03-20 
                        ViewBag.TeamID = teamCD;
                        ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_1;
                        ///
                        ///End KietNM
                        ///                       
                        var leagueID = teamTopInfo.LeagueID;
                        teamTopInfo.HittingRanking = GetHittingStatByTeamCD(leagueID, teamCD);
                        teamTopInfo.PitchingRanking = GetPitchingStatByTeamCD(leagueID, teamCD);
                        teamTopInfo.TeamPostList = PostedController.GetRecentPosts(Constants.NPB_POST_TEAM_TYPE, Constants.NPB_SPORT_ID, teamCD, Constants.TEAM_TOPIC_CLASSIFICATION);
                    }
                    return View(teamTopInfo);
                }
                else
                {
                    return View();
                }                
            }
            else
            {
                return View();
            }           
        }
        #endregion

        #region Utilities

        #region Get Team Info
        /// <summary>
        /// Get team name, league name, ranking from team ID 
        /// </summary>
        /// <param name="teamID">Team ID</param>
        /// <returns>Info of team</returns>
        public NpbTeamTopInfoViewModel GetTeamInfo(int teamID)
        {
            var result = default(NpbTeamTopInfoViewModel);
            var dateNow = DateTime.Now;
            //var weekOfMonth = (short)Utils.GetWeekNumberOfMonth(dateNow);
            var classDeviation = (short)2;

            //Get new ranking form table OfficialStatsHeaderNpb.
            int leagueID = npb.TeamInfoMST.Where(m => m.TeamCD == teamID).Select(m => m.LeagueID).FirstOrDefault().Value;
            var matchDay = npb.OfficialStatsHeaderNpb.Where(m => m.GameAssortment == leagueID).Max(m => m.Matchday == null ? 0 : m.Matchday);

            //Get record from Npb context.
            var queryFirst = from ti in npb.TeamInfoMST
                             join ticon in npb.TeamIconNpb on ti.TeamCD equals ticon.TeamCD
                             join osh in npb.OfficialStatsHeaderNpb on ti.LeagueID equals osh.GameAssortment
                             join os in npb.OfficialStatsNpb on osh.OfficialStatsHeaderNpbId equals os.OfficialStatsHeaderNpbId
                             where (ti.TeamCD == teamID && os.TeamCD == teamID && osh.Matchday == matchDay)
                             select new NpbTeamTopInfoViewModel
                             {
                                 LeagueID = ti.LeagueID.Value,
                                 TeamID = ti.TeamCD,
                                 ShortNameLeague = ti.ShortNameLeague,
                                 Team = ti.Team,
                                 Ranking = os.Ranking,
                                 TeamIcon = ticon.TeamIcon,
                             };

            //Get record from member context.
            //Old require.
            //var querySecond = com.Deviation.Where(m => m.UniqueID == teamID && m.ClassClass == classDeviation && m.SportsID == Constants.NPB_SPORT_ID && m.StartYear == dateNow.Year && m.Month == dateNow.Month && m.Week == weekOfMonth).ToList();
            //New require.
            var querySecond = com.Deviation.Where(m => m.UniqueID == teamID && m.ClassClass == classDeviation && m.SportsID == Constants.NPB_SPORT_ID && m.StartYear == dateNow.Year).ToList();

            //Join 2 list to get all infos.
            if (querySecond.Count != 0)
            {
                var lstFirst = queryFirst.ToList();
                result = (from q1 in lstFirst
                          join q2 in querySecond on q1.TeamID equals q2.UniqueID into tmp
                          from q3 in tmp.DefaultIfEmpty()
                          select new NpbTeamTopInfoViewModel
                          {
                              LeagueID = q1.LeagueID,
                              TeamID = q1.TeamID,
                              ShortNameLeague = q1.ShortNameLeague,
                              Team = q1.Team,
                              Ranking = q1.Ranking,
                              TeamIcon = q1.TeamIcon,
                              ExpectationsDeviation = q3.ExpectationsDeviation,
                              BetrayalDeviation = q3.BetrayalDeviation
                          }).FirstOrDefault();
            }
            else
            {
                result = queryFirst.FirstOrDefault();
            }
            return result;
        }
        #endregion

        #region Get HittingStat By TeamCD
        /// <summary>
        /// Get hitting stat of a team by team cd(id).
        /// </summary>
        /// <param name="gameType">League type.</param>
        /// <param name="teamCD">Team ID</param>
        /// <returns>Hitting stat info of team.</returns>
        public IEnumerable<HittingStats> GetHittingStatByTeamCD(int gameType, int teamID)
        {
            var matchDay = npb.HittingStatsHeader.Where(m => m.GameAssortment == gameType).Max(m => m.Matchday == null ? 0 : m.Matchday);
            var query = (from hsh in npb.HittingStatsHeader
                         join hs in npb.HittingStats on hsh.HittingStatsHeaderId equals hs.HittingStatsHeaderId
                         where hs.TeamCD == teamID && hsh.GameAssortment == gameType && hsh.Matchday == matchDay
                         select hs).ToList().OrderByDescending(m => Convert.ToDouble(m.BattingAverage)).Take(5);
            return query;
        }
        #endregion

        #region Get PitchingStat By TeamCD
        /// <summary>
        /// Get pitching stat of a team by team cd(id).
        /// </summary>
        /// <param name="gameType">League type.</param>
        /// <param name="teamCD">Team ID</param>
        /// <returns>Pitching stat info of team.</returns>
        public NpbPitchingStatsInfoViewModel GetPitchingStatByTeamCD(int gameType, int teamCD)
        {
            // List team from db.
            var matchDay = npb.PitchingStatsHeader.Where(m => m.GameAssortment == gameType).Max(m => m.Matchday == null ? 0 : m.Matchday);
            var lstInfo = from psh in npb.PitchingStatsHeader
                          join ps in npb.PitchingStats on psh.PitchingStatsHeaderId equals ps.PitchingStatsHeaderId
                          where ps.TeamCD == teamCD && psh.GameAssortment == gameType && psh.Matchday == matchDay
                          select ps;

            NpbPitchingStatsInfoViewModel pStatsInfo = null;
            if (lstInfo.Any())
            {
                pStatsInfo = new NpbPitchingStatsInfoViewModel();
                //Get EarnedRunAverage not valid : "-"  
                var lstNotValid = lstInfo.Where(x => x.EarnedRunAverage.Trim() == "-" || x.EarnedRunAverage == null).ToList();
                //Remove not valid in list.
                var lstValid = lstInfo.ToList().Except(lstNotValid).ToList();

                //防御率　選手名 - 防御率　防御率
                var topEarnedRunAverage = lstValid.OrderBy(x => Convert.ToDecimal(x.EarnedRunAverage)).First();
                //勝数　　選手名 - 勝数　　勝敗
                var topWin = lstInfo.OrderByDescending(x => x.Win).First();
                //奪三振数　選手名 - 奪三振数　奪三振数
                var topStrikeOut = lstInfo.OrderByDescending(x => x.Strikeout).First();
                //セーブ数　選手名 - セーブ数　セーブ数
                var topSave = lstInfo.OrderByDescending(x => x.Save).First();
                //ホールド数　選手名 - ホールド数　ホールド数
                var topHold = lstInfo.OrderByDescending(x => x.Hold).First();
                pStatsInfo.NamePlayerERA = topEarnedRunAverage.Name;
                pStatsInfo.PlayerIDERA = topEarnedRunAverage.PlayerCD.Value;
                pStatsInfo.EarnedRunAverage = topEarnedRunAverage.EarnedRunAverage;
                pStatsInfo.NamePlayerW = topWin.Name;
                pStatsInfo.PlayerIDW = topWin.PlayerCD.Value;
                pStatsInfo.Win = topWin.Win.Value;
                pStatsInfo.NamePlayerSO = topStrikeOut.Name;
                pStatsInfo.PlayerIDSO = topStrikeOut.PlayerCD.Value;
                pStatsInfo.StrikeOut = topStrikeOut.Strikeout.Value;
                pStatsInfo.NamePlayerS = topSave.Name;
                pStatsInfo.PlayerIDS = topSave.PlayerCD.Value;
                pStatsInfo.Save = topSave.Save.Value;
                pStatsInfo.NameH = topHold.Name;
                pStatsInfo.PlayerIDH = topHold.PlayerCD.Value;
                pStatsInfo.Hold = topHold.Hold.Value;
            }
            return pStatsInfo;
        }
        #endregion

        #endregion       
    }
}