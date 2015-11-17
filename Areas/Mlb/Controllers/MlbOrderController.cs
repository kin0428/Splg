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
 * Class		: MlbOrderController
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Areas.Mlb.Models;
using Splg.Areas.Mlb.Models.ViewModels;
#endregion

namespace Splg.Areas.Mlb.Controllers
{
    public class MlbOrderController : Controller
    {
        MlbEntities Mlb = new MlbEntities();
        // GET: Mlb/MlbOrder
        public ActionResult Index()
        {
            List<MlbOrderViewModel> orderViewModelList = new List<MlbOrderViewModel>();
            // ア・リーグ　東地区
            MlbOrderViewModel MlbOrder = GetOfficialStats(MlbConstants.OFFICIALSTATS_LEAGUE_AMERICAN, MlbConstants.OFFICIALSTATS_DIVGROUP_EAST);
            if (MlbOrder != null)
                orderViewModelList.Add(MlbOrder);
            // ア・リーグ　中地区
            MlbOrder = GetOfficialStats(MlbConstants.OFFICIALSTATS_LEAGUE_AMERICAN, MlbConstants.OFFICIALSTATS_DIVGROUP_CENTRAL);
            if (MlbOrder != null)
                orderViewModelList.Add(MlbOrder);
            // ア・リーグ　西地区
            MlbOrder = GetOfficialStats(MlbConstants.OFFICIALSTATS_LEAGUE_AMERICAN, MlbConstants.OFFICIALSTATS_DIVGROUP_WEST);
            if (MlbOrder != null)
                orderViewModelList.Add(MlbOrder);
            // ナ・リーグ　東地区
            MlbOrder = GetOfficialStats(MlbConstants.OFFICIALSTATS_LEAGUE_NATIONAL, MlbConstants.OFFICIALSTATS_DIVGROUP_EAST);
            if (MlbOrder != null)
                orderViewModelList.Add(MlbOrder);
            // ナ・リーグ　中地区
            MlbOrder = GetOfficialStats(MlbConstants.OFFICIALSTATS_LEAGUE_NATIONAL, MlbConstants.OFFICIALSTATS_DIVGROUP_CENTRAL);
            if (MlbOrder != null)
                orderViewModelList.Add(MlbOrder);
            // ナ・リーグ　西地区
            MlbOrder = GetOfficialStats(MlbConstants.OFFICIALSTATS_LEAGUE_NATIONAL, MlbConstants.OFFICIALSTATS_DIVGROUP_WEST);
            if (MlbOrder != null)
                orderViewModelList.Add(MlbOrder);
           IEnumerable<MlbOrderViewModel> viewList = orderViewModelList;

            //MlbOrderViewModel viewList = null;
            return View(viewList);
        }

        #region Get ranking for table ア・リーグ, ナ・リーグ
        /// <summary>
        /// Join 3 table OfficialStatsHeaders, OfficialStatsHeaders,TeamIconMSTs
        /// </summary>
        /// <param name="gameType">Game type (8-ア・リーグ/9-ナ・リーグ : 1-東地区/2-中地区/3-西地区)</param>
        /// <returns>List of ranking information</returns>
        private MlbOrderViewModel GetOfficialStats(int leagueId,int divId)
        {
            MlbEntities Mlb = new MlbEntities();
            var firstHeader = from header in Mlb.OfficialStatsHeaderMlb
                              join lg in Mlb.LeagueGroupMlb on header.OfficialStatsHeaderMlbId equals lg.OfficialStatsHeaderMlbId
                              join grp in Mlb.DivGroupMlb on lg.LeagueGroupMlbId equals grp.LeagueGroupMlbId 
                              where (lg.LeagueID == leagueId) 
                              where (grp.DivID == divId)
                              select header;
            if (firstHeader == null || !firstHeader.Any())
                return null;
            var query = from os in Mlb.OfficialStatsMlb
                        join ti in Mlb.TeamIconMlb on os.TeamID equals ti.TeamCD into icon
                        from tic in icon.DefaultIfEmpty()
                        join dgrp in Mlb.DivGroupMlb on os.DivGroupMlbId equals dgrp.DivGroupMlbId
                        join lgrp in Mlb.LeagueGroupMlb on dgrp.LeagueGroupMlbId equals lgrp.LeagueGroupMlbId
                        select new MlbOfficialStatsViewModel
                        {
                            TeamID = os.TeamID,
                            TeamIcon = tic.TeamIcon,
                            Ranking = os.Ranking,
                            TeamName = os.TeamName,
                            Win = os.Win,
                            Lose = os.Lose,
                            WinningPercentage = os.WinningPercentage,
                            GameBehind = os.GameBehind,
                            StatHome = os.StatHome,
                            StatVisitor = os.StatVisitor,
                            Streak = os.Streak,
                            LeagueID = lgrp.LeagueID,
                            DivID = dgrp.DivID,
                            CreatedDate = os.CreatedDate,
                        } into officialStats
                        where (officialStats.LeagueID == leagueId) 
                        where (officialStats.DivID == divId)
                        orderby officialStats.Ranking
                        select officialStats;
			MlbOrderViewModel orderViewModel = new MlbOrderViewModel();
            orderViewModel.GameDate = firstHeader.First().GameDateJPN;
            orderViewModel.LeagueID = leagueId;
            orderViewModel.DivID = divId;
            orderViewModel.officialStatsViewModels = query;
            return orderViewModel;
        }
        #endregion

    }
}