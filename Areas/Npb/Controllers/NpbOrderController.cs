#region Author Information
/*
 * Namespace	: Splg.Areas.Npb.Controllers
 * Class		: NpbOrderController
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;

namespace Splg.Areas.Npb.Controllers
{
    public class NpbOrderController : Controller
    {
        NpbEntities npb = new NpbEntities();
        // GET: Npb/NpbOrder
        public ActionResult Index()
        {
            List<NpbOrderViewModel> orderViewModelList = new List<NpbOrderViewModel>();
			NpbOrderViewModel npbOrder = GetOfficialStats(Constants.OFFICIALSTATS_GAMEASSORTMENT_CENTRAL_LEAGUE);
			if(npbOrder != null)
				orderViewModelList.Add(npbOrder);
			npbOrder = GetOfficialStats(Constants.OFFICIALSTATS_GAMEASSORTMENT_PACIFFC_LEAGUE);
			if(npbOrder != null)
				orderViewModelList.Add(npbOrder);
			npbOrder = GetOfficialStats(Constants.OFFICIALSTATS_GAMEASSORTMENT_EXCHANGE_GAME);
			if(npbOrder != null)
				orderViewModelList.Add(npbOrder);
			npbOrder = GetExhibitionGameStats(Constants.EXHIBITIONGAMESTATS_GAMEASSORTMENT_EXHIBITION_GAME);
			if(npbOrder != null)
				orderViewModelList.Add(npbOrder);
            IEnumerable<NpbOrderViewModel> viewList = orderViewModelList;
            return View(viewList);
        }

        #region Get ranking for table セ・リーグ, パ・リーグ, 交流戦
        /// <summary>
        /// Join 3 table OfficialStatsHeaders, OfficialStatsHeaders,TeamIconMSTs
        /// </summary>
        /// <param name="gameType">Game type (1-セ・リーグ/2-パ・リーグ/26-交流戦)</param>
        /// <returns>List of ranking information</returns>
        private NpbOrderViewModel GetOfficialStats(int gameType)
        {
            NpbEntities npb = new NpbEntities();
            var firstHeader = from header in npb.OfficialStatsHeaderNpb
                              where(header.GameAssortment == gameType)
                              select header;
            if (firstHeader == null || !firstHeader.Any())
                return null;
            var query = from os in npb.OfficialStatsNpb
                        join osHeader in npb.OfficialStatsHeaderNpb on os.OfficialStatsHeaderNpbId equals osHeader.OfficialStatsHeaderNpbId
                        join ti in npb.TeamIconNpb on os.TeamCD equals ti.TeamCD
                        select new NpbOfficialStatsViewModel
                        {
                            TeamID = os.TeamCD,
                            GameAssortment = osHeader.GameAssortment,
                            TeamIcon = ti.TeamIcon,
                            Ranking = os.Ranking,
                            ShortNameTeam = os.ShortNameTeam,
                            Game = os.Game,
                            Win = os.Win,
                            Lose = os.Lose,
                            Draw = os.Draw,
                            WinningPercentage = os.WinningPercentage,
                            GameBehind = os.GameBehind,
                            CreatedDate = os.CreatedDate,
                            RestGame = os.RestGame
                        } into officialStats
                        where (officialStats.GameAssortment == gameType)
                        orderby officialStats.Ranking
                        select officialStats;
			NpbOrderViewModel orderViewModel = new NpbOrderViewModel();
			orderViewModel.GameAssortment = gameType;
			orderViewModel.Matchday = firstHeader.First().Matchday;
			orderViewModel.officialStatsViewModels = query;
			return orderViewModel;
        }
        #endregion

        #region Get ranking for table オープン戦
        /// <summary>
        /// Join 3 table ExhibitionGameStats, ExhibitionGameStatsHeaders,TeamIconMSTs
        /// </summary>
        /// <param name="gameType">Game type </param>
        /// <returns>List of ranking information</returns>
        private NpbOrderViewModel GetExhibitionGameStats(int gameType)
        {
            NpbEntities npb = new NpbEntities();
            var firstHeader = from header in npb.ExhibitionGameStatsHeader
                               select header;
            if (firstHeader == null || !firstHeader.Any())
                return null;
            var query = from egs in npb.ExhibitionGameStats
                        join egsHeader in npb.ExhibitionGameStatsHeader on egs.ExhibitionGameStatsHeaderId equals egsHeader.ExhibitionGameStatsHeaderId
                        join ti in npb.TeamIconNpb on egs.TeamCD equals ti.TeamCD
                        select new NpbOfficialStatsViewModel
                        {
                            TeamID = egs.TeamCD,
                            GameAssortment = egsHeader.GameAssortment,
                            TeamIcon = ti.TeamIcon,
                            Ranking = egs.Ranking,
                            ShortNameTeam = egs.ShortNameTeam,
                            Game = egs.Game,
                            Win = egs.Win,
                            Lose = egs.Lose,
                            Draw = egs.Draw,
                            WinningPercentage = egs.WinningPercentage,
                            CreatedDate = egs.CreatedDate,
                            GameBehind = egs.GameBehind
                        } into exhibitionGameStats
                        where (exhibitionGameStats.GameAssortment == gameType)
                        orderby exhibitionGameStats.Ranking
                        select exhibitionGameStats;
			NpbOrderViewModel orderViewModel = new NpbOrderViewModel();
			orderViewModel.GameAssortment = 0;
			orderViewModel.Matchday = firstHeader.First().Matchday;
			orderViewModel.officialStatsViewModels = query;
			return orderViewModel;
        }
        #endregion
    }
}