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
 * Namespace	: Splg.Areas.Jlg.Controllers
 * Class		: JlgTeamInfoController
 * Developer	: Nguyen Ho Long Hai
 * Updated date : 2015-03-31
 */
#endregion

using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Services.Game;
using Splg.Areas.Jleague.Models.Dto;

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgTeamInfoController : Controller
    {
        #region Menu config description
        /// <summary>
        /// KietNM / 2015-04-11
        ///                  Menu main jleague 
        /// ViewBag.JleagueMenu : Using choice JleagueMenu
        /// ViewBag.JleagueMenu = 1 => Jleague Home 画面設計書 (JリーグTOP画面）: 10-1
        /// ViewBag.JleagueMenu = 2 => Jleague J1Top 画面設計書 (J1TOP画面）: 10-2
        /// ViewBag.JleagueMenu = 3 => Jleague J2Top 画面設計書 (J2TOP画面）: 10-3
        /// ViewBag.JleagueMenu = 4 => Jleague NabiscoTop 画面設計書 (ナビスコCTOP画面）: 10-4
        /// ViewBag.JleagueMenu = 5 => Jleague JlgNews 画面設計書 (ニュースリスト(Jリーグ)画面）: 10-6

        ///                  Menu sub jleague 
        /// ViewBag.JleagueSubMenu : Using choice sub Jleague
        /// ViewBag.JleagueSubMenu = 1 => Jleague Home JリーグTOP : 10_2; 10_3
        /// ViewBag.JleagueSubMenu = 2 => Jleague 日程・結果 10_2_1 ; 10_3_1
        /// ViewBag.JleagueSubMenu = 3 => Jleague 順位表: 10-2_2 ; 10_3_2
        /// ViewBag.JleagueSubMenu = 4 => Jleague 戦績表: 10_2_3 ; 10_3_3
        /// ViewBag.JleagueSubMenu = 5 => Jleague 個人成績: 10_2_4 ; 10_3_4
        /// ViewBag.JleagueSubMenu = 6 => Jleague チーム情報: 10-2_5 ; 10_3_5
        /// </summary>
        #endregion

        #region Global Properties
        //Create context to get value from db.         
        JlgEntities jlg = new JlgEntities();
        #endregion

        /// <summary>
        /// Get Data Team info by gameKind
        /// gameKind=2 Jleague 1
        /// gameKind=6 Jleague 2
        /// </summary>
        /// <param name="gameKind"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
           
            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
            ViewBag.JleagueSubMenu = 6;

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
            var query = (from gameKindTeamGT in jlg.GameKindTeamGT
                         join categoryGT in jlg.CategoryGT on gameKindTeamGT.GameKindTeamGTId equals categoryGT.GameKindTeamGTId
                         join teamInfoGT in jlg.TeamInfoGT on categoryGT.CategoryGTId equals teamInfoGT.CategoryGTId
                         where categoryGT.GameKindID == gameKindID
                         select new JlgTeamInfoViewModel
                         {
                             TeamID = teamInfoGT.TeamID,
                             TeamName = teamInfoGT.TeamName,
                             TeamIcon = (from teamIconJlg in jlg.TeamIconJlg where teamIconJlg.TeamCD == teamInfoGT.TeamID select teamIconJlg.TeamIcon).FirstOrDefault()

                         }).Distinct().OrderByDescending(p => p.TeamID);

            var result = query.ToList();

            return View(query);
        } 

        /// <summary>
        /// 直近の試合結果取得
        /// </summary>
        [ChildActionOnly()]
        public ActionResult GetRecentGameResults(int homeTeamId,int awayTeamId,int gameId)
        {
            var jlgRecentGameResultsViewModel = new JlgRecentGameResultsViewModel();

            var jlgService = new JlgService();

            jlgRecentGameResultsViewModel.TargetHomeTeamId = homeTeamId;

            jlgRecentGameResultsViewModel.TargetAwayTeamId = awayTeamId;

            jlgRecentGameResultsViewModel.HomeTeamSpec = jlgService.GetTeamSpecByTeamId(homeTeamId);

            jlgRecentGameResultsViewModel.HomeRecentGameResults = jlgService.GetRecentGameResults(homeTeamId, gameId);

            jlgRecentGameResultsViewModel.HomeRecentGameResultCounts = jlgService.CalculateJlgRecentGameResultCounts(homeTeamId, jlgRecentGameResultsViewModel.HomeRecentGameResults);

            jlgRecentGameResultsViewModel.AwayTeamSpec = jlgService.GetTeamSpecByTeamId(awayTeamId);

            jlgRecentGameResultsViewModel.AwayRecentGameResults = jlgService.GetRecentGameResults(awayTeamId, gameId);

            jlgRecentGameResultsViewModel.AwayRecentGameResultCounts = jlgService.CalculateJlgRecentGameResultCounts(awayTeamId, jlgRecentGameResultsViewModel.AwayRecentGameResults);

            return PartialView("_JlgRecentGameResults", jlgRecentGameResultsViewModel);
        }

        /// <summary>
        /// 直近の直接対決結果取得
        /// </summary>
        [ChildActionOnly()]
        public ActionResult GetJlgRecentMatches(int homeTeamId, int awayTeamId,int gameDate)
        {
            var jlgRecentMatchesViewModel = new JlgRecentMatchesViewModel();

            var jlgService = new JlgService();

            jlgRecentMatchesViewModel.TargetHomeTeamId = homeTeamId;

            jlgRecentMatchesViewModel.TargetAwayTeamId = awayTeamId;

            jlgRecentMatchesViewModel.HomeTeamSpec = jlgService.GetTeamSpecByTeamId(homeTeamId);

            jlgRecentMatchesViewModel.AwayTeamSpec = jlgService.GetTeamSpecByTeamId(awayTeamId);

            jlgRecentMatchesViewModel.RecentMatches = jlgService.GetRecentMatches(homeTeamId, awayTeamId, gameDate);

            jlgRecentMatchesViewModel.RecentMatchesCounts = jlgService.CalculateJlgRecentGameResultCounts(homeTeamId, jlgRecentMatchesViewModel.RecentMatches);

            return PartialView("_JlgRecentMatches", jlgRecentMatchesViewModel);
        }

        /// <summary>
        /// チームの傾向（棒グラフ）取得
        /// </summary>
        [ChildActionOnly()]
        public ActionResult GetTeamTrendsAtBar(int homeTeamId, int awayTeamId, int gameDate)
        {
            var jlgService = new JlgService();

            var jlgTeamTrendsAtBarViewModel = new JlgTeamTrendsAtBarViewModel()
            {
                TargetHomeTeamId = homeTeamId,
                TargetAwayTeamId = awayTeamId,
                HomeTeamSpec = jlgService.GetTeamSpecByTeamId(homeTeamId),
                AwayTeamSpec = jlgService.GetTeamSpecByTeamId(awayTeamId),
                HomeTeamTrendsAtBar = jlgService.GetTeamTrendsAtBar(homeTeamId, gameDate),
                AwayTeamTrendsAtBar = jlgService.GetTeamTrendsAtBar(awayTeamId, gameDate),
            };

            return PartialView("_JlgTeamTrendsAtBar", jlgTeamTrendsAtBarViewModel);
        }

        /// <summary>
        /// チームの傾向（円グラフ）取得
        /// </summary>
        [ChildActionOnly()]
        public ActionResult GetGetTeamTrendsAtPie(int homeTeamId, int awayTeamId)
        {
            var jlgChartService = new JlgChartService();

            return PartialView("_JlgTeamTrendsAtPie", jlgChartService.GetjlgTeamTrendsAtPieViewModel(homeTeamId,awayTeamId));
        }

        /// <summary>
        /// 関連記事取得
        /// </summary>
        [ChildActionOnly()] 
        public ActionResult GetRelatedArticles(int homeTeamId, int awayTeamId)
        {
            var jlgService = new JlgService();

            var jlgRelatedArticlesViewModel = new JlgRelatedArticlesViewModel()
            {
                TargetHomeTeamId = homeTeamId,
                TargetAwayTeamId = awayTeamId,
                HomeTeamSpec = jlgService.GetTeamSpecByTeamId(homeTeamId),
                AwayTeamSpec = jlgService.GetTeamSpecByTeamId(awayTeamId),
                HomeRelatedArticles = new RelatedArticles() { Items = jlgService.GetRelatedArticles(homeTeamId), JLeagueType = jlgService.GetJlgType(Request.Url.AbsoluteUri) },
                AwayRelatedArticles = new RelatedArticles() { Items = jlgService.GetRelatedArticles(awayTeamId), JLeagueType = jlgService.GetJlgType(Request.Url.AbsoluteUri) },
            };

            return PartialView("_JlgRelatedArticles", jlgRelatedArticlesViewModel);
        }
    }
}