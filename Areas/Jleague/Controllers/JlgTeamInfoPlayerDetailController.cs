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
 * Namespace	: Splg.Areas.Jleague.Controllers
 * Class		: JlgTeamInfoPlayerDetailController
 * Developer	: e-concier
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
#endregion

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgTeamInfoPlayerDetailController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Jleague to get db.
        /// </summary>
        JlgEntities jlg = new JlgEntities();
        #endregion

        #region Index
        // GET: Jleague/JlgTeamInfoPlayerDetail
        public ActionResult Index( int? playerID)
        {
            JlgTeamInfoPlayerDetailViewModel playerInfo = new JlgTeamInfoPlayerDetailViewModel();
            if(playerID != null)
            {
                int teamID = GetTeamID((int)playerID);
                ViewBag.PlayerID = playerID;
                ViewBag.TeamID = teamID;
                ViewBag.TeamName = jlg.TeamInfoTE.Where(x => x.TeamID == teamID).Select(x => x.TeamName).FirstOrDefault();
                int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
                ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
                ViewBag.JleagueSubMenu = 6;
                ViewBag.JleagueTeamMenu = 4;
                ViewBag.JType = jType;
                ViewBag.TeamInfoMenuTabActive = (int)JlgConstants.TeamInfoMenu.TabActive_6;

                playerInfo.PlayerInfoYear = GetPlayerInfo(teamID, playerID.Value);
                playerInfo.PlayerSum = GetPlayerInfo_Sum(teamID, playerID.Value);
                playerInfo.PlayerInfoOccasion = GetPlayerInfo_Occasion(teamID, playerID.Value);
                playerInfo.TeamPostedInfoList = PostedController.GetRecentPosts(3, Constants.JLG_SPORT_ID, teamID, Constants.TEAM_TOPIC_CLASSIFICATION);
            }
            return View(playerInfo);
        }
        #endregion

        public JlgPlayerInfoYear GetPlayerInfo(int inTeamID, int inPlayerID)
        {
            var query = from playerHeader in jlg.PlayerStatsReportPS
                        join playerInfo in jlg.PlayerInfoPS on playerHeader.PlayerStatsReportPSId equals playerInfo.PlayerStatsReportPSId
                        where playerHeader.TeamID == inTeamID && playerHeader.GameKindID == 2 && playerInfo.PlayerID == inPlayerID
                        select new JlgPlayerInfoYear
                        {
                            PlayerStatsReportPS = playerHeader,
                            PlayerInfoPS = playerInfo
                        };
            return query.FirstOrDefault();
        }

        public JlgPlayerInfoSum GetPlayerInfo_Sum(int inTeamID, int inPlayerID)
        {
            var query = from playerInfo in jlg.PlayerInfoPS
                        where playerInfo.PlayerID == inPlayerID 
                        group playerInfo by playerInfo.PlayerID into newGroup
                        select new JlgPlayerInfoSum
                        {
                            Game = newGroup.Sum(p => p.Game),
                            Goal = newGroup.Sum(p => p.Goal)
                        };
            return query.FirstOrDefault(); 
        }

        public IEnumerable<JlgPlayerInfoOccasion> GetPlayerInfo_Occasion(int inTeamID, int inPlayerID)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            var query = from scheduleInfo in jlg.ScheduleInfo
                        join statsReportLS in jlg.StatsReportLS on scheduleInfo.GameID equals statsReportLS.GameID
                        join teamInfoLS in jlg.TeamInfoLS on statsReportLS.StatsReportLSId equals teamInfoLS.StatsReportLSId
                        from playerLS in jlg.PlayerLS.Where(x => x.TeamInfoLSId == teamInfoLS.TeamInfoLSId && x.ID == inPlayerID).DefaultIfEmpty()
                        where statsReportLS.GameDate <= now && teamInfoLS.ID == inTeamID
                        orderby scheduleInfo.OccasionNo
                        select new JlgPlayerInfoOccasion
                        {
                            OccasionNo = scheduleInfo.OccasionNo,
                            Time = (playerLS == null ? 0 : playerLS.Time),
                            StartF = (playerLS == null ? 0 : playerLS.StartF),
                            Score = (playerLS == null ? 0 : playerLS.Score),
                            PKScore = (playerLS == null ? 0 : playerLS.PKScore),
                            Shoot = (playerLS == null ? 0 : playerLS.Shoot),
                            Yellow = (playerLS == null ? 0 : playerLS.Yellow),
                            Red = (playerLS == null ? 0 : playerLS.Red)
                        };

            return query;
        }

        public int GetTeamID(int playerID)
        {
            int query = (from idi in jlg.PlayerInfoDI
                         join ddi in jlg.DirectoryDI on idi.DirectoryDIId equals ddi.DirectoryDIId
                         where idi.PlayerID == playerID
                         select ddi.TeamID).FirstOrDefault();

            return query;
        }
    }
}