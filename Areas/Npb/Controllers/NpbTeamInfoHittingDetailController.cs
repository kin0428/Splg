using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Areas.Npb.Models;
using Splg.Controllers;

namespace Splg.Areas.Npb.Controllers
{
    public class NpbTeamInfoHittingDetailController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        NpbEntities npb = new NpbEntities();

        #endregion
        
        #region Index
        // GET: Npb/NpbTeamInfoBattingDetail
        public ActionResult Index(int? teamID, int? playerID)
        {
            //#1297対応
            //return RedirectToActionPermanent("Show", "NpbTeamInfoHittingDetail", new { area = "Npb", playerID = playerID });

            NpbTeamInfoHittingDetailModelView npbTeamInfoHittingDetail = new NpbTeamInfoHittingDetailModelView();
            if (playerID != null && teamID != null)
            {
                ViewBag.PlayerID = playerID;
                ViewBag.TeamID = teamID;
                ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_5;
                ViewBag.TeamName = npb.TeamInfoMST.Where(x => x.TeamCD == teamID).Select(x => x.Team).FirstOrDefault();
                string leagueName = npb.TeamInfoMST.Where(x => x.TeamCD == teamID).Select(x => x.ShortNameLeague).FirstOrDefault();
                ViewBag.LeagueName = Constants.LEAGUENAME_AFTER;
                npbTeamInfoHittingDetail.TeamPostedInfoList = PostedController.GetRecentPosts(Constants.NPB_POST_TEAM_TYPE, Constants.NPB_SPORT_ID, teamID.Value, Constants.TEAM_TOPIC_CLASSIFICATION);
                npbTeamInfoHittingDetail.ListHittingStats6thGameInfo = GetHittingStats6thByPlayerID(teamID.Value, playerID.Value);
                npbTeamInfoHittingDetail.HittingStatsConditionStandingList = GetHittingConditionStanding(teamID.Value, playerID.Value);
                npbTeamInfoHittingDetail.HittingStats3YearsInfoList = GetHittingStats3YearsInfo(teamID.Value, playerID.Value);
            }
            return View(npbTeamInfoHittingDetail);
        }


        // GET: Npb/NpbTeamInfoBattingDetail
        public ActionResult Show(int? playerID)
        {
            NpbTeamInfoHittingDetailModelView npbTeamInfoHittingDetail = new NpbTeamInfoHittingDetailModelView();
            if (playerID != null )
            {
                ViewBag.PlayerID = playerID;

                int? teamID = (from tis in npb.TeamInfoST
                               join pis in npb.PlayerInfoST on tis.TeamInfoSTId equals pis.TeamInfoSTId
                               where pis.PlayerID == playerID
                               select tis.TeamID).FirstOrDefault();
                ViewBag.TeamID = teamID;

                ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_5;

                if (teamID != null)
                {
                    ViewBag.TeamName = npb.TeamInfoMST.Where(x => x.TeamCD == teamID).Select(x => x.Team).FirstOrDefault();
                    string leagueName = npb.TeamInfoMST.Where(x => x.TeamCD == teamID).Select(x => x.ShortNameLeague).FirstOrDefault();

                    ViewBag.LeagueName = Constants.LEAGUENAME_AFTER;
                    npbTeamInfoHittingDetail.TeamPostedInfoList = PostedController.GetRecentPosts(Constants.NPB_POST_TEAM_TYPE, Constants.NPB_SPORT_ID, teamID.Value, Constants.TEAM_TOPIC_CLASSIFICATION);
                    npbTeamInfoHittingDetail.ListHittingStats6thGameInfo = GetHittingStats6thByPlayerID(teamID.Value, playerID.Value);
                    npbTeamInfoHittingDetail.HittingStatsConditionStandingList = GetHittingConditionStanding(teamID.Value, playerID.Value);
                    npbTeamInfoHittingDetail.HittingStats3YearsInfoList = GetHittingStats3YearsInfo(teamID.Value, playerID.Value);
                }
            }
            return View(@"Index", npbTeamInfoHittingDetail);
        }
        #endregion
        
        #region Get PitchingStats6th By PlayerID, TeamID
        /// <summary>
        /// Get pitching stats 6th by playerID, TeamID.
        /// </summary>
        /// <param name="playerID">PlayerID.</param>
        /// <param name="teamID">TeamID.</param>
        /// <returns>Pitching Stats 6th Game info.</returns>
        public IEnumerable<NpbHittingStats6thGameInfoViewModel> GetHittingStats6thByPlayerID(int teamID, int playerID)
        {
            var query = from hi in npb.HittingStats6thGame
                        join his in npb.HittingStats6thGameInfo on hi.HittingStats6thGameId equals his.HittingStats6thGameId
                        join giss in npb.GameInfoSSN on his.GameDate equals giss.DateJPN
                        join mg in npb.MonthGroupSSN on giss.MonthGroupSSNId equals mg.MonthGroupSSNId
                        where (hi.PlayerCD == playerID && hi.TeamCD == teamID) &&
                              (giss.HTeamID == teamID || giss.VTeamID == teamID) &&
                              (giss.DateJPN.ToString().Substring(4, 2) == (mg.No < 10 ? ("0" + mg.No.ToString()) : mg.No.ToString()))
                        orderby his.GameDate descending
                        select new NpbHittingStats6thGameInfoViewModel
                        {
                            HittingStats6thGameInfo = his,
                            GameID = giss.ID,
                            StadiumName = giss.StadiumName,
                            Time = giss.TimeJPN,
                            GameDate = giss.DateJPN,
                            HScore = giss.HScore.Value == null ? 0 : giss.HScore.Value,
                            VScore = giss.VScore.Value == null ? 0 : giss.VScore.Value,
                            GameResult = giss.GameResult == null ? 0 : giss.GameResult,
                            HTeamID = giss.HTeamID == null ? 0 : giss.HTeamID,
                            VTeamID = giss.VTeamID == null ? 0 : giss.VTeamID,
                        };

            return query;
        }
        #endregion

        #region Get Condition Standing By PlayerID, TeamID
        /// <summary>
        /// Get pitching stats 6th by playerID, TeamID.
        /// </summary>
        /// <param name="playerID">PlayerID.</param>
        /// <param name="teamID">TeamID.</param>
        /// <returns></returns>
        public IEnumerable<NpbHittingStatsConditionStanding> GetHittingConditionStanding(int teamID, int playerID)
        {
            var query = from hstdh in npb.HittingStatsTeamDifferenceHeader
                        join hstd in npb.HittingStatsTeamDifference on hstdh.HittingStatsTeamDifferenceHeaderId equals hstd.HittingStatsTeamDifferenceHeaderId
                        join hstdi in npb.HittingStatsTeamDifferenceInfo on hstd.HittingStatsTeamDifferenceId equals hstdi.HittingStatsTeamDifferenceId
                        join gc in npb.TeamInfoMST on hstd.TeamCD equals gc.TeamCD
                        join ti in npb.TeamIconNpb on hstdi.TeamsOpponentCD equals ti.TeamCD
                        where (hstd.PlayerCD == playerID && hstd.TeamCD == teamID)
                        select new NpbHittingStatsConditionStanding
                        {
                            HittingStatsTeamDifferenceInfo = hstdi,
                            ShortNameLeague = gc.ShortNameLeague,
                            TeamIcon = ti.TeamIcon
                        };

            return query;
        }
        #endregion

        #region Get HittingStats 3Years Info By PlayerID, TeamID
        /// <summary>
        /// Get hittingstats 3 years info by TeamID, PlayerID.
        /// </summary>
        /// <param name="teamID">TeamID</param>
        /// <param name="playerID">PlayerID</param>
        /// <returns>List hittingstats 3 years.</returns>
        public IEnumerable<HittingStats3YearsInfo> GetHittingStats3YearsInfo(int teamID, int playerID)
        {
            var query = from ps3t in npb.HittingStats3YearsTeams
                        join ps3 in npb.HittingStats3Years on ps3t.HittingStats3YearsTeamsId equals ps3.HittingStats3YearsTeamsId
                        join ps3gi in npb.HittingStats3YearsInfo on ps3.HittingStats3YearsId equals ps3gi.HittingStats3YearsId
                        where ps3t.TeamID == teamID && ps3.ID == playerID
                        select ps3gi;
            return query;
        }
        #endregion
    }
}