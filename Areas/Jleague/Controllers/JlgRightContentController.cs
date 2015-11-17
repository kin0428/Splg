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
 * Class		: JlgRightContentController
 * Developer	: Nojima
 * 
 */
#endregion


#region Using
using System;

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Splg.Models;
using System.Web;
using System.Web.Mvc;
using Splg.Models.Game.ViewModel;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using System.Web.UI;
using Splg.Areas.Jleague;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Models.News.ViewModel;
#endregion

namespace Splg.Areas.Jleague.Controllers
{
    /// <summary>
    /// Controller for all Jlg right content : top3 ranking, my point,...
    /// Use for each control in PartialView : JlgRightContent 
    /// </summary>
    public class JlgRightContentController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Jlg to get db.
        /// </summary>
        JlgEntities Jlg = new JlgEntities();
        /// <summary>
        /// Declare context News to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        #endregion

        #region Partial View

        #region Show TopN Ranking
        /// <summary>
        /// Show Jlg top N ranking in partial view.
        /// </summary>
        /// <returns>PartialView : _JlgTop3Ranking</returns>        
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 5)]
        public ActionResult ShowTopNRanking(int? teamID)
        {
            JlgTop3RankingViewModel ranking = new JlgTop3RankingViewModel();
            if (teamID == null || teamID == 0)
            {
                List<int> gameKindIDs = GetGameKindIDs();
                foreach (var gameKindID in gameKindIDs)
                {
                    if (gameKindID == JlgConstants.JLG_GAMEKIND_J1)
                    {
                        //J1
                        ranking.J1League = GetTopNRankingByType(teamID, gameKindID, 3);
                    }
                    else if (gameKindID == JlgConstants.JLG_GAMEKIND_J2)
                    {
                        //J2
                        ranking.J2League = GetTopNRankingByType(teamID, gameKindID, 3);
                    }
                    else if (gameKindID == JlgConstants.JLG_GAMEKIND_NABISCO)
                    {
                        //ナビスコ
                        ranking.Nabisco = GetTopNRankingByType(teamID, gameKindID, 3);
                    }
                }
            }
            else
            {
                var gameKindID = Jlg.TeamCardReportTC.Where(m => m.TeamID == teamID).Select(m => m.GameKindID).FirstOrDefault();
                if (gameKindID == JlgConstants.JLG_GAMEKIND_J1)
                {
                    //J1
                    ranking.J1League = GetTopNRankingByType(teamID, gameKindID, 6);
                }
                else if (gameKindID == JlgConstants.JLG_GAMEKIND_J2)
                {
                    //J2
                    ranking.J2League = GetTopNRankingByType(teamID, gameKindID, 6);
                }
                else if (gameKindID == JlgConstants.JLG_GAMEKIND_NABISCO)
                {
                    //ナビスコ
                    ranking.Nabisco = GetTopNRankingByType(teamID, gameKindID, 6);
                }
            }
            return PartialView("~/Areas/Jleague/Views/Shared/_JlgTop3Ranking.cshtml", ranking);
        }
        #endregion

        #region Show Jlg RecentNews
        /// <summary>
        /// Show top 5 Jlg recent news in day. 
        /// </summary>
        /// <returns>PartialView : _JlgRecentNews</returns>
        [ChildActionOnly]
        public ActionResult ShowJlgRecentNews(string type, int? teamID, int? gameID, string homeVisitor)
        {
            if (teamID != null)
            {
                ViewBag.TeamID = teamID.Value;
                ViewBag.TeamName = (from tc in Jlg.TeamCardReportTC
                                    join tm in Jlg.TeamInfoTE on tc.TeamID equals tm.TeamID
                                    where tc.Year == DateTime.Now.Year
                                    select tm.TeamName).FirstOrDefault();
            }
            if (gameID != null)
            {
                ViewBag.GameID = gameID;
            }
            if (!string.IsNullOrEmpty(homeVisitor))
            {
                ViewBag.HomeVisitor = homeVisitor;
            }
            IEnumerable<NewsInfoViewModel> result = GetJlgNews(type, teamID);
            ViewBag.Name = type;
            return PartialView("_JleagueRecentNews", result);
        }
        #endregion

        #region Show Jlg RecentNews at the right of page
        /// <summary>
        /// Show top 5 Jlg recent news in day. 
        /// </summary>
        /// <returns>PartialView : _JlgRecentNews</returns>
        [ChildActionOnly]
        public ActionResult ShowJlgRightRecentNews(string type)
        {
            IEnumerable<NewsInfoViewModel> result = GetJlgNews(Constants.NPBTOP, null);
            ViewBag.Name = type;
            return PartialView("_JleagueRightRecentNews", result);
        }
        #endregion

        #region Show PersonInfo
        /// <summary>
        /// Show person info of 1 player.
        /// </summary>
        /// <param name="playerID">PlayerID</param>
        /// <returns>PartialView : _JlgPersonInfo</returns>
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client)] // every 3 sec
        public ActionResult ShowPersonInfo(int? teamID, int? playerID)
        {
            JlgPersonInfoViewModel personInfo = GetPersonInfoByPlayerID(teamID, playerID);
            return PartialView("_JlgPersonInfo", personInfo);
        }
        #endregion

        //#region Show HittingStats                             サッカーの他の成績取得を考慮
        ///// <summary>
        ///// Show hitting stats by teamID.
        ///// </summary>
        ///// <param name="teamID">TeamID</param>
        ///// <returns>Hittingstats info of player.</returns>
        //[OutputCache(NoStore = true, Location = OutputCacheLocation.Client)] // every 3 sec
        //public ActionResult ShowHittingStats(int? teamID, int? playerID)
        //{
        //    HittingStats hittingStats = null;
        //    if (teamID!= null && playerID != null)
        //    {
        //        hittingStats = GetHittingStatsByPlayerID(teamID.Value, playerID.Value);
        //    }
        //    return PartialView("_JlgBattingResult", hittingStats);
        //}
        //#endregion       

        #region Show Jlg News Top Views
        /// <summary>
        /// Show top 4 Jlg news that have max views. 
        /// </summary>
        /// <returns>PartialView : _JlgRecentNews</returns>
        //[ChildActionOnly]
        public ActionResult ShowJlgNewsTopViews()
        {
            IEnumerable<NewsInfoViewModel> result = GetJlgNewTopViews();
            return PartialView("_JleagueNewsTopView", result);
        }
        #endregion

        #endregion
        #region Utilities

        #region Get GameAssortment In OfficialStats Header
        /// <summary>
        /// Get gameassortment (gametype) in table OfficialStatsHeader
        /// </summary>
        /// <returns>List game type need get.</returns>
        public List<int> GetGameKindIDs()
        {
            var query = Jlg.GameKindInfo.Select(m => m.GameKindID).Distinct().ToList();
            return query;
        }
        #endregion

        #region Get TopN Ranking By Type
        /// <summary>
        /// Get top N ranking by type of Jlg.
        /// </summary>
        /// <param name="leagueID">GameType</param>
        /// <returns>List top N team have win highest.</returns>
        public IEnumerable<JlgOfficialStatsModel> GetTopNRankingByType(int? teamID, int? leagueID, int topN)
        {
            var query = default(IEnumerable<JlgOfficialStatsModel>);
            if (teamID == null && leagueID != null)
            {
                query = (from rf in Jlg.RankInfoRT
                         join tc in Jlg.TeamCardReportTC
                         on rf.TeamID equals tc.TeamID
                         where tc.GameKindID == leagueID && tc.Year == DateTime.Now.Year
                         orderby rf.Ranking
                         select new JlgOfficialStatsModel
                         {
                             Draw = rf.Draw,
                             Lose = rf.Lose,
                             Win = rf.Win,
                             Game = rf.Game
                         }).Take(topN);
            }
            else
            {
                query = (from rf in Jlg.RankInfoRT
                         join tc in Jlg.TeamCardReportTC
                         on rf.TeamID equals tc.TeamID
                         where tc.GameKindID == leagueID && tc.Year == DateTime.Now.Year
                         orderby rf.Ranking
                         select new JlgOfficialStatsModel
                         {
                             Draw = rf.Draw,
                             Lose = rf.Lose,
                             Win = rf.Win,
                             Game = rf.Game
                         }).Take(topN);
            }

            return query;
        }
        #endregion

        #region Get Jlg Recent News
        /// <summary>
        /// Get top 5 Jlg recent news.
        /// </summary>
        /// <returns>List news.</returns>
        public IEnumerable<NewsInfoViewModel> GetJlgNews(string type, int? teamID)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            var imageContent = Constants.IMAGE_THUMNAIL_DUID;
            if (!string.IsNullOrEmpty(type))
            {
                if (type == Constants.JLG_TOP_INDEX || type == Constants.JLG_TOP_J1 || type == Constants.JLG_TOP_J2)
                {
                    imageContent = Constants.IMAGE_DUID;
                    query = (from brief in com.BriefNews
                             join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                             join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                             join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                             from tmp in br_photo.DefaultIfEmpty()
                             where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                                (tmp.Duid == imageContent || tmp.Duid == null) && itpcsm.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
                             orderby brief.DeliveryDate descending
                             select new NewsInfoViewModel
                             {
                                 NewsItemID = brief.NewsItemID,
                                 DeliveryDate = brief.DeliveryDate,
                                 Headline = brief.Headline,
                                 newstext = brief.newstext,
                                 SubHeadline = brief.SubHeadline,
                                 Content = tmp.Content,
                                 Duid = tmp.Duid,
                                 SentFrom = brief.SentFrom,
                                 ItpcSubjectCode = itpc.IptcSubjectCode
                             } into news_photo
                             select news_photo).Take(5);
                }
                else if (type == Constants.JLG_J1_T_TOP || type == Constants.JLG_J1_T_PYR || type == Constants.JLG_J1_T_PYR_DTL || type == Constants.JLG_GAME_INFO || type == Constants.JLG_J1_RESULT)
                {
                    if (type == Constants.JLG_J1_T_TOP)
                    {
                        imageContent = Constants.IMAGE_DUID;
                    }
                    var classificationType = 0;
                    var uniqueID = 0;
                    if (teamID != null)
                    {
                        classificationType = Constants.TEAM_TOPIC_CLASSIFICATION;
                        uniqueID = teamID.Value;
                    }
                    query = (from brief in com.BriefNews
                             join nt in com.NewsTopic on brief.NewsItemID equals nt.NewsItemID
                             join tm in com.TopicMaster on nt.TopicID equals tm.TopicID
                             join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                             from tmp in br_photo.DefaultIfEmpty()
                             where (tmp.Duid == imageContent || tmp.Duid == null) && tm.ClassificationType == classificationType && tm.UniqueID == uniqueID
                                    && brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now
                             orderby brief.DeliveryDate descending
                             select new NewsInfoViewModel
                             {
                                 NewsItemID = brief.NewsItemID,
                                 DeliveryDate = brief.DeliveryDate,
                                 Headline = brief.Headline,
                                 SubHeadline = brief.SubHeadline,
                                 newstext = brief.newstext,
                                 Content = tmp.Content,
                                 Duid = tmp.Duid,
                                 SentFrom = brief.SentFrom
                             }).Take(5);
                }
            }
            return query;
        }
        #endregion

        #region Get PersonInfo By PlayerID
        /// <summary>
        /// Get player info and team info (icon) by playerID.
        /// </summary>
        /// <param name="playerID">ID need get info.</param>
        /// <returns>Play info from db.</returns>
        public JlgPersonInfoViewModel GetPersonInfoByPlayerID(int? teamID, int? playerID)
        {
            var query = default(JlgPersonInfoViewModel);
            if (playerID != null && teamID != null)
            {
                ViewBag.TeamID = teamID;
                //query = (from pi in Jlg.PlayerInfoRE
                //         join d in Jlg.TeamInfoRE on pi.TeamInfoREId equals d.TeamInfoREId
                //         join ti in Jlg.TeamIconJlg on d.TeamID equals ti.TeamCD
                //         where pi.PlayerID == playerID && d.TeamID == teamID
                //         select new JlgPersonInfoViewModel
                //         {
                //             PersonInfo = pi,
                //             TeamID = d.TeamID,
                //             TeamIcon = ti.TeamIcon,
                //             TeamName = d.TeamNameS
                //         }).FirstOrDefault();

                query = (from ddi in Jlg.DirectoryDI
                         join dda in Jlg.DirectoryDA on ddi.TeamID equals dda.TeamID
                         join pid in Jlg.PlayerInfoDI on ddi.DirectoryDIId equals pid.DirectoryDIId
                         join pia in Jlg.PlayerInfoDA on new { dda.DirectoryDAId, pid.PlayerID } equals new { pia.DirectoryDAId, pia.PlayerID }
                         where ddi.TeamID == teamID && pid.PlayerID == playerID
                         select new JlgPersonInfoViewModel
                         {
                             PersonInfoDI = pid,
                             PersonInfoDA = pia,
                             ImagePath = ""
                         }).FirstOrDefault();
            }
            return query;
        }
        #endregion

        #region Get JlgNews Top Views
        /// <summary>
        /// Get top 4 Jlg news that have max views.
        /// </summary>
        /// <returns>List 4 Jlg news.</returns>
        public IEnumerable<NewsInfoViewModel> GetJlgNewTopViews()
        {

            var query = default(IEnumerable<NewsInfoViewModel>);

            var viewsCount = (from r in com.NewsReading
                              select r).GroupBy(x => x.NewsItemID).Select(
                                    l => new
                                    {
                                        NewsItemId = l.Key,
                                        ReadingSum = l.Count()
                                    });
            query = (from brief in com.BriefNews
                     join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                     join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                     join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                     from tmp in br_photo.DefaultIfEmpty()
                     join rds in com.NewsReadingSumView on brief.NewsItemID equals rds.NewsItemID
                     join v in viewsCount on brief.NewsItemID equals v.NewsItemId
                     where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                        (tmp.Duid == Constants.IMAGE_DUID || tmp.Duid == null) && itpcsm.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
                     select new NewsInfoViewModel
                     {
                         NewsItemID = brief.NewsItemID,
                         DeliveryDate = brief.DeliveryDate,
                         Headline = brief.Headline,
                         newstext = brief.newstext,
                         Content = tmp.Content,
                         Duid = tmp.Duid,
                         SentFrom = brief.SentFrom,
                         ItpcSubjectCode = itpc.IptcSubjectCode,
                         TotalViews = v.ReadingSum,
                         TotalViewsInOneHour = rds.View ?? 0,
                         SubHeadline = brief.SubHeadline
                     } into news_photo
                     orderby news_photo.TotalViewsInOneHour descending
                     select news_photo).Take(4);

            return query;
        }
        #endregion


        #endregion
        #region Get NextDate
        /// <summary>
        /// Get next date if have game.
        /// </summary>
        /// <param name="gameDate">GameDate.</param>
        /// <returns>NextDate.</returns>
        [HttpPost]
        public JsonResult GetNextDate(int? gameDate)
        {
            int? nxtDate = 0;
            nxtDate = Jlg.ScheduleInfo.Where(m => m.GameDate > gameDate).Min(m => m.GameDate);
            return Json(nxtDate, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}