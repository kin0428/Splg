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
 * Class		: MlbTeamInfoTeamTopController
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
    public class MlbTeamInfoTeamTopController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Npb to get db.
        /// </summary>
        MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        #endregion

        #region Index
        // GET: Npb/NpbTeamInfoTeamTop
        public ActionResult Index(string teamID)
        {
            MlbTeamInfoTeamTopViewModel viewModel = null;

            ViewBag.teamId = teamID;
            ViewBag.TeamName = Utils.GetTeamNameById(Constants.MLB_SPORT_ID, Int32.Parse(teamID));

            //入力チェック
            if (string.IsNullOrEmpty(teamID))
            {
                return View(viewModel);                  //TODO エラー処理
            }

            //ビューモデルのセット

            var teamCD = 0;
            if (!Int32.TryParse(teamID, out teamCD))
            {
                return View(viewModel);                  //TODO エラー処理
            }

            //チーム情報　期待度偏差値
            viewModel = GetTeamInfo(teamCD);

            //TeamID not exist in db, not show.
            if (viewModel != null)
            {
                ViewBag.TeamID = teamCD;
                ViewBag.TeamInfoMenuTabActive = (int)MlbConstants.TeamInfoMenu.TabActive_1;

                //日本人プレイヤー
                var JapanesePlayers = (from ti in mlb.TeamInfo
                              join jpl in mlb.JapanesePlayers on ti.TeamInfoId equals jpl.TeamInfoId
                              where ti.TeamID == teamCD
                              orderby jpl.Years
                              select jpl
                              );

                viewModel.ExistingJapanesePlayers = MlbCommon.GetExistingJapanesePlayers(JapanesePlayers, DateTime.Now.Year.ToString());
                viewModel.ExistedJapanesePlayers = MlbCommon.GetExistedJapanesePlayers(JapanesePlayers, DateTime.Now.Year.ToString());

                viewModel.TeamInfo = (from ti in mlb.TeamInfo where ti.TeamID == teamCD select ti).FirstOrDefault();
                viewModel.OfficialStatsMlb = (from osm in mlb.OfficialStatsMlb where osm.TeamID == teamCD select osm).FirstOrDefault();

                viewModel.StatLast5Years = (from stats in mlb.StatLast5Years
                                            join ti in mlb.TeamInfo on stats.TeamInfoId equals ti.TeamInfoId
                                            where ti.TeamID == teamCD
                                            orderby stats.Year descending
                                            select stats).ToList();

                //新着の投稿記事
                viewModel.TeamPostList = PostedController.GetRecentPosts(Constants.NPB_POST_TEAM_TYPE, Constants.MLB_SPORT_ID, teamCD, Constants.TEAM_TOPIC_CLASSIFICATION);
            }

            return View(viewModel);


        }
        #endregion


        #region Get Team Info
        /// <summary>
        /// Get team name, league name, ranking from team ID 
        /// </summary>
        /// <param name="teamID">Team ID</param>
        /// <returns>Info of team</returns>
        public MlbTeamInfoTeamTopViewModel GetTeamInfo(int teamID)
        {
            var result = default(MlbTeamInfoTeamTopViewModel);


            var dateNow = DateTime.Now;
            var weekOfMonth = (short)Utils.GetWeekNumberOfMonth(dateNow);
            var classDeviation = (short)2;

            //Get new ranking form table OfficialStatsHeaderNpb.
            int leagueID = mlb.TeamInfo.Where(m => m.TeamID == teamID).Select(m => m.LeagueID).FirstOrDefault().Value;
            //var matchDay = mlb.OfficialStatsHeaderMlb.Where(m => m.GameAssortment == leagueID).Max(m => m.Matchday == null ? 0 : m.Matchday);

            //Get record from Mlb context.
            var queryFirst = from ti in mlb.TeamInfo    //チーム情報
                             join ticon in mlb.TeamIconMlb on ti.TeamID equals ticon.TeamCD into ticont
                             from tic in ticont.DefaultIfEmpty()
                             join os in mlb.OfficialStatsMlb on ti.TeamID equals os.TeamID  //順位表_チーム情報
                             join dg in mlb.DivGroupMlb on os.DivGroupMlbId equals dg.DivGroupMlbId
                             join lg in mlb.LeagueGroupMlb on dg.LeagueGroupMlbId equals lg.LeagueGroupMlbId
                             join osh in mlb.OfficialStatsHeaderMlb on lg.OfficialStatsHeaderMlbId equals osh.OfficialStatsHeaderMlbId
                             // where (ti.TeamID == teamID && osh.Year == dateNow.Year)
                             where (ti.TeamID == teamID)
                             select new MlbTeamInfoTeamTopViewModel
                             {
                                 LeagueID = ti.LeagueID.Value,
                                 TeamID = ti.TeamID,
                                 ShortNameLeague = ti.LeagueName,
                                 Team = ti.TeamName,
                                 Ranking = os.Ranking,
                                 TeamIcon = tic.TeamIcon,


                             };

            //Get record from member context.
            var querySecond = com.Deviation.Where(m => m.UniqueID == teamID && m.ClassClass == classDeviation && m.SportsID == Constants.MLB_SPORT_ID && m.StartYear == dateNow.Year).ToList();

            //Join 2 list to get all infos.
            if (querySecond.Count != 0)
            {
                var lstFirst = queryFirst.ToList();
                result = (from q1 in lstFirst
                          join q2 in querySecond on q1.TeamID equals q2.UniqueID into tmp
                          from q3 in tmp.DefaultIfEmpty()
                          select new MlbTeamInfoTeamTopViewModel
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