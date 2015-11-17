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
 * Class		: JlgTeamInfoTeamTopController
 * Developer	: Nojima
 * Updated date : 2015-05-06
 */
#endregion

#region Using directives
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
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

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgTeamInfoTopController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Jlg to get db.
        /// </summary>
        JlgEntities jlg = new JlgEntities();
        /// <summary>
        /// Declare context News to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        #endregion

        #region Index
        // GET: Jlg/JlgTeamInfoTeamTop
        public ActionResult Index(string teamId)
        {
            JlgTeamInfoTopViewModel viewModel = null;

            ViewBag.TeamID = teamId;
            int teamIdInt = Int32.Parse(teamId);
            ViewBag.TeamName = jlg.TeamInfoTE.Where(x => x.TeamID == teamIdInt).Select(x => x.TeamName).FirstOrDefault();

            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
            ViewBag.JleagueSubMenu = 6;
            ViewBag.JleagueTeamMenu = 1;
            //Add jtype to session to use for in User_Article page.
            Session["JType"] = jType;
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

            //入力チェック
            if (string.IsNullOrEmpty(teamId))
            {
                return View(viewModel);                  //TODO エラー処理
            }

            //ビューモデルのセット

            var teamCD = 0;
            if (!Int32.TryParse(teamId, out teamCD))
            {
                return View(viewModel);                  //TODO エラー処理
            }

            //チーム情報　期待度偏差値
            viewModel = GetTeamInfo(teamCD, gameKindID);

            //TeamID not exist in db, not show.
            if (viewModel != null)
            {
                ViewBag.TeamID = teamCD;
                ViewBag.TeamInfoMenuTabActive = (int)JlgConstants.TeamInfoMenu.TabActive_1;

                //新着の投稿記事
                viewModel.TeamPostList = PostedController.GetRecentPosts(Constants.NPB_POST_TEAM_TYPE, Constants.JLG_SPORT_ID, teamCD, Constants.TEAM_TOPIC_CLASSIFICATION);


                viewModel.Top5Players = GetPersonalAchieveInfos(gameKindID,teamCD);
                viewModel.ResultInfoSS = getResultInfoSS(teamCD);
                viewModel.RankInfoRT = (from r in jlg.RankInfoRT where r.TeamID == teamCD select r).FirstOrDefault();
            }

            return View(viewModel);


        }
        #endregion


         #region Get GetPersonalAchieveInfos
        /// <summary>
        /// Join 4 table RankGoalReportRG, RankInfoRG, PlayerInfoDI ,PlayerInfoPS   to get :
        /// 1. RankInfoRG : PlayerID, PlayerNames, TeamID, TeamNameS, Goal, Shoot,  Time,  GoalPK
        /// 2.PlayerInfoDI : Position
        /// 3.PlayerInfoPS : Yellow , Red
        /// 4. GoalShoot = Goal/Shoot
        /// 5. GoalTime = Goal/(Time*90)
        /// </summary>       
        /// <returns>List data </returns>
        public IEnumerable<JlgPersonalAchieveInfos> GetPersonalAchieveInfos(int inGameKind ,int teamCD)
        {
            IEnumerable<JlgPersonalAchieveInfos> infos = (from goalReport in jlg.RankGoalReportRG
                                                          join info in jlg.RankInfoRG on goalReport.RankGoalReportRGId equals info.RankGoalReportRGId
                                                          join playerInfoDI in jlg.PlayerInfoDI on info.PlayerID equals playerInfoDI.PlayerID into playerInfo
                                                          from player in playerInfo.DefaultIfEmpty()
                                                          join playerInfoPS in jlg.PlayerInfoPS on info.PlayerID equals playerInfoPS.PlayerID into infoPS
                                                          from playerPS in infoPS.DefaultIfEmpty()
                                                          join playerStatsReportPS in jlg.PlayerStatsReportPS on playerPS.PlayerStatsReportPSId equals playerStatsReportPS.PlayerStatsReportPSId
                                                          where goalReport.GameKindID == inGameKind
                                                          && playerStatsReportPS.GameKindID == inGameKind
                                                          && info.TeamID == teamCD
                                                          orderby info.Ranking
                                                          select new JlgPersonalAchieveInfos
                                                          {
                                                              Ranking = info.Ranking,
                                                              PlayerID = info.PlayerID,
                                                              PlayerNames = info.PlayerNameS,
                                                              PlayerName = info.PlayerName,
                                                              TeamID = info.TeamID,
                                                              TeamNameS = info.TeamNameS,
                                                              Goal = info.Goal,
                                                              Shoot = info.Shoot,
                                                              Time = info.Time,
                                                              Position = player.Position,
                                                              GoalPK = info.GoalPK,
                                                              Game = info.Game,
                                                              Yellow = playerPS.Yellow,
                                                              Red = playerPS.Red,
                                                              RateGoalShoot = (info.Shoot == null || info.Shoot.Value == 0) ? null : info.Goal / (info.Shoot * 1m),
                                                              RateGoalTime = (info.Time == null || info.Time.Value == 0) ? null : info.Goal / (info.Time * 90m)
                                                          }).Distinct();
            return infos.Take(5);
        }
        #endregion


        private List<ResultInfoSS> getResultInfoSS(int teamId)
        {

            var a = (from ts in jlg.TeamSeasonReportSS
                     join ri in jlg.ResultInfoSS on ts.TeamSeasonReportSSId equals ri.TeamSeasonReportSSId
                     join li in jlg.LeagueInfo on ri.GameKindID  equals li.LeagueID
                     where ts.TeamID == teamId
                     orderby ri.Year descending
                         select ri).Take(5);
            return a.ToList();

        }

        #region Get Team Info
        /// <summary>
        /// Get team name, league name, ranking from team ID 
        /// </summary>
        /// <param name="teamID">Team ID</param>
        /// <returns>Info of team</returns>
        public JlgTeamInfoTopViewModel GetTeamInfo(int teamID, int gameKindID)
        {
            var result = default(JlgTeamInfoTopViewModel);

            var dateNow = DateTime.Now;
            var weekOfMonth = (short)Utils.GetWeekNumberOfMonth(dateNow);
            var classDeviation = (short)2;

            var queryFirst = (from rk in jlg.RankInfoRT      //一試合速報_順位情報
                              join ic in jlg.TeamIconJlg on rk.TeamID equals ic.TeamCD into temp
                              from icon in temp.DefaultIfEmpty()
                              join sc in jlg.TeamInfoTE on rk.TeamID equals sc.TeamID
                              join gk in jlg.GameKindInfo on gameKindID equals gk.GameKindID
                              where rk.TeamID == teamID
                              select new JlgTeamInfoTopViewModel
                              {
                                  LeagueID = gameKindID,
                                  TeamID = teamID,
                                  ShortNameLeague = gk.GameKindName,
                                  Team = sc.TeamName,
                                  Ranking = rk.Ranking,
                                  TeamIcon = icon.TeamIcon,
                              });

            //Get record from member context.
            var querySecond = com.Deviation.Where(m => m.UniqueID == teamID && m.ClassClass == classDeviation && m.SportsID == Constants.MLB_SPORT_ID && m.StartYear == dateNow.Year).ToList();

            //Join 2 list to get all infos.
            if (querySecond.Count != 0)
            {
                var lstFirst = queryFirst.ToList();
                result = (from q1 in lstFirst
                          join q2 in querySecond on q1.TeamID equals q2.UniqueID into tmp
                          from q3 in tmp.DefaultIfEmpty()
                          select new JlgTeamInfoTopViewModel
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


    }
}