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
 * Namespace	: Splg.Areas.MyPage.Controllers
 * Class		: MyPageRightContentController
 * Developer	: Nojima
 * 
 */
#endregion

using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using Splg.Models.News;
using Splg.Models.News.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Services.Point;

namespace Splg.Areas.MyPage.Controllers
{
    /// <summary>
    /// Controller for all npb right content : top3 ranking, my point,...
    /// Use for each control in PartialView : NpbRightContent 
    /// </summary>
    public class MyPageRightContentController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Npb to get db.
        /// </summary>
        NpbEntities npb = new NpbEntities();
        /// <summary>
        /// Declare context News to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        #endregion

        #region Partial View

        #region Show Group List
        [ChildActionOnly]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 5)]
        public ActionResult ShowGroupList(long memberId)
        {
            IEnumerable<MyPageGroupListViewModel.GroupListInfo> model = MyPageCommon.GetGroupLists(com, memberId);


            return PartialView("~/Areas/MyPage/Views/Shared/_MyPageRightGroupRanking.cshtml", model);
        }
        #endregion




        #region Show Follow Ranking List
        [ChildActionOnly]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 5)]
        public ActionResult ShowFollowPointRankings(long memberId)
        {
            var pointRankingService = new PointRankingService(com);

            var pointRankingViewModels = pointRankingService.GetFollowPointRankingsByMemberId(memberId,5);
            
            return PartialView("_FollowRankings", pointRankingViewModels.ToList());
        }


        #endregion

        #region Show TopN Ranking
        /// <summary>
        /// Show npb top N ranking in partial view.
        /// </summary>
        /// <returns>PartialView : _NpbTop3Ranking</returns>        
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 5)]
        public ActionResult ShowTopNRanking(int? teamID)
        {
            Top3RankingViewModel ranking = new Top3RankingViewModel();
            if (teamID == null || teamID == 0)
            {
                List<int> lstGameType = GetGameAssortmentInOfficialStats();
                foreach (var item in lstGameType)
                {
                    if (item == 1)
                    {
                        //セリーグ　
                        ranking.SeLeague = GetTopNRankingByType(teamID, item, 3);
                    }
                    else if (item == 2)
                    {
                        //パリーグ
                        ranking.PaLeague = GetTopNRankingByType(teamID, item, 3);
                    }
                }
            }
            else
            {
                var leagueID = npb.TeamInfoMST.Where(m => m.TeamCD == teamID).Select(m => m.LeagueID).FirstOrDefault();
                if (leagueID == 1)
                {
                    //セリーグ　
                    ranking.SeLeague = GetTopNRankingByType(teamID, leagueID.Value, 6);
                }
                else
                {
                    //パリーグ
                    ranking.PaLeague = GetTopNRankingByType(teamID, leagueID.Value, 6);
                }
            }
            return PartialView("~/Areas/Npb/Views/Shared/_NpbTop3Ranking.cshtml", ranking);
        }
        #endregion

        #region Show Npb RecentNews
        /// <summary>
        /// Show top 5 npb recent news in day. 
        /// </summary>
        /// <returns>PartialView : _NpbRecentNews</returns>
        [ChildActionOnly]
        public ActionResult ShowNpbRecentNews(string type, int? teamID, int? gameID, string homeVisitor)
        {
            if (teamID != null)
            {
                ViewBag.TeamID = teamID.Value;
                ViewBag.TeamName = npb.TeamInfoMST.Where(x => x.TeamCD == teamID).Select(x => x.Team).FirstOrDefault();
            }
            if (type == Constants.NPBTEAMINFOPITCHER || type == Constants.NPBTEAMINFOBATCHER)
            {
                teamID = null;
            }
            if (gameID != null)
            {
                ViewBag.GameID = gameID;
            }
            if (!string.IsNullOrEmpty(homeVisitor))
            {
                ViewBag.HomeVisitor = homeVisitor;
            }
            IEnumerable<NewsInfoViewModel> result = GetNpbNews(type, teamID);
            ViewBag.Name = type;
            return PartialView("_NpbRecentNews", result);
        }
        #endregion

        #region Show Npb RecentNews at the right of page
        /// <summary>
        /// Show top 5 npb recent news in day. 
        /// </summary>
        /// <returns>PartialView : _NpbRecentNews</returns>
        [ChildActionOnly]
        public ActionResult ShowNpbRightRecentNews(string type)
        {
            IEnumerable<NewsInfoViewModel> result = GetNpbNews(Constants.NPBTOP, null);
            ViewBag.Name = type;
            return PartialView("_NpbRightRecentNews", result);
        }
        #endregion

        #region Show PersonInfo
        /// <summary>
        /// Show person info of 1 player.
        /// </summary>
        /// <param name="playerID">PlayerID</param>
        /// <returns>PartialView : _NpbPersonInfo</returns>
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client)] // every 3 sec
        public ActionResult ShowPersonInfo(int? teamID, int? playerID)
        {
            NpbPersonInfoViewModel personInfo = GetPersonInfoByPlayerID(teamID, playerID);
            return PartialView("_NpbPersonInfo", personInfo);
        }
        #endregion

        #region Show HittingStats
        /// <summary>
        /// Show hitting stats by teamID.
        /// </summary>
        /// <param name="teamID">TeamID</param>
        /// <returns>Hittingstats info of player.</returns>
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client)] // every 3 sec
        public ActionResult ShowHittingStats(int? teamID, int? playerID)
        {
            HittingStats hittingStats = null;
            if (teamID != null && playerID != null)
            {
                hittingStats = GetHittingStatsByPlayerID(teamID.Value, playerID.Value);
            }
            return PartialView("_NpbBattingResult", hittingStats);
        }
        #endregion

        #region Show Npb News Top Views
        /// <summary>
        /// Show top 4 npb news that have max views. 
        /// </summary>
        /// <returns>PartialView : _NpbRecentNews</returns>
        [ChildActionOnly]
        public ActionResult ShowNpbNewsTopViews()
        {
            IEnumerable<NewsInfoViewModel> result = GetNpbNewTopViews();
            return PartialView("_NpbNewsTopView", result);
        }
        #endregion


        #endregion

        #region Utilities

        #region Get GameAssortment In OfficialStats Header
        /// <summary>
        /// Get gameassortment (gametype) in table OfficialStatsHeader
        /// </summary>
        /// <returns>List game type need get.</returns>
        public List<int> GetGameAssortmentInOfficialStats()
        {
            var query = npb.OfficialStatsHeaderNpb.Select(m => m.GameAssortment).Distinct().ToList();
            return query;
        }
        #endregion

        #region Get TopN Ranking By Type
        /// <summary>
        /// Get top N ranking by type of npb.
        /// </summary>
        /// <param name="leagueID">GameType</param>
        /// <returns>List top N team have win highest.</returns>
        public IEnumerable<OfficialStatsNpb> GetTopNRankingByType(int? teamID, int? leagueID, int topN)
        {
            var query = default(IEnumerable<OfficialStatsNpb>);
            if (teamID == null && leagueID != null)
            {
                var matchDay = npb.OfficialStatsHeaderNpb.Where(m => m.GameAssortment == leagueID).Max(m => m.Matchday == null ? 0 : m.Matchday);
                long headerId = (from offheader in npb.OfficialStatsHeaderNpb
                                 where offheader.Matchday == matchDay && offheader.GameAssortment == leagueID
                                 select offheader.OfficialStatsHeaderNpbId).SingleOrDefault();

                query = npb.OfficialStatsNpb.Where(p => p.OfficialStatsHeaderNpbId == headerId).OrderBy(p => p.Ranking).Take(topN);
            }
            else
            {
                var matchDay = npb.OfficialStatsHeaderNpb.Where(m => m.GameAssortment == leagueID).Max(m => m.Matchday == null ? 0 : m.Matchday);
                query = (from offheader in npb.OfficialStatsHeaderNpb
                         join offs in npb.OfficialStatsNpb on offheader.OfficialStatsHeaderNpbId equals offs.OfficialStatsHeaderNpbId
                         where offheader.GameAssortment == leagueID && offheader.Matchday == matchDay
                         select offs).OrderBy(p => p.Ranking).Take(topN);
            }

            return query;
        }
        #endregion

        #region Get Npb Recent News
        /// <summary>
        /// Get top 5 npb recent news.
        /// </summary>
        /// <returns>List news.</returns>
        public IEnumerable<NewsInfoViewModel> GetNpbNews(string type, int? teamID)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            var imageContent = Constants.IMAGE_THUMNAIL_DUID;
            if (!string.IsNullOrEmpty(type))
            {
                if (type == Constants.NPBTOP)
                {
                    imageContent = Constants.IMAGE_DUID;
                    query = (from brief in com.BriefNews
                             join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                             join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                             join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                             from tmp in br_photo.DefaultIfEmpty()
                             where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                                (tmp.Duid == imageContent || tmp.Duid == null) && itpcsm.IptcSubjectCode == Constants.NPB_ITPCSUBJECTCODE
                             orderby brief.DeliveryDate descending
                             select new NewsInfoViewModel
                             {
                                 NewsItemID = brief.NewsItemID,
                                 DeliveryDate = brief.DeliveryDate,
                                 Headline = brief.Headline,
                                 newstext = brief.newstext,
                                 Content = tmp.Content,
                                 Duid = tmp.Duid,
                                 SentFrom = brief.SentFrom,
                                 ItpcSubjectCode = itpc.IptcSubjectCode
                             } into news_photo
                             select news_photo).Take(5);
                }
                else if (type == Constants.NPBTEAMINFOTOP || type == Constants.NPBTEAMINFOPITCHER || type == Constants.NPBGAMEINFO)
                {
                    if (type == Constants.NPBTEAMINFOTOP)
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
        public NpbPersonInfoViewModel GetPersonInfoByPlayerID(int? teamID, int? playerID)
        {
            var query = default(NpbPersonInfoViewModel);
            if (playerID != null && teamID != null)
            {
                query = (from pi in npb.PersonInfo
                         join d in npb.Directory on pi.DirectoryId equals d.DirectoryId
                         join ti in npb.TeamIconNpb on d.TeamID equals ti.TeamCD
                         join pt in npb.DefenceLocationMST on pi.PositionType.ToString() equals pt.DefenceLocationID.ToString() into tmp
                         from tmp1 in tmp.DefaultIfEmpty()
                         where pi.ID == playerID && d.TeamID == teamID
                         select new NpbPersonInfoViewModel
                         {
                             PersonInfo = pi,
                             TeamID = d.TeamID,
                             TeamIcon = ti.TeamIcon,
                             TeamName = d.TeamName,
                             PositionName = tmp1.Name
                         }).FirstOrDefault();
            }
            return query;
        }
        #endregion

        #region Get HittingStats By PlayerID
        /// <summary>
        /// Get hittingstats by PlayerID
        /// </summary>
        /// <param name="TeamID">PlayerID.</param>
        /// <returns>HittingStats Info.</returns>
        public HittingStats GetHittingStatsByPlayerID(int teamID, int playerID)
        {
            var query = npb.HittingStats.Where(m => m.PlayerCD == playerID && m.TeamCD == teamID).FirstOrDefault();
            return query;
        }
        #endregion

        #region Get NpbNews Top Views
        /// <summary>
        /// Get top 4 npb news that have max views.
        /// </summary>
        /// <returns>List 4 npb news.</returns>
        public IEnumerable<NewsInfoViewModel> GetNpbNewTopViews()
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            query = (from brief in com.BriefNews
                     join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                     join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                     join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                     from tmp in br_photo.DefaultIfEmpty()
                     join rds in com.NewsReadingSumView on brief.NewsItemID equals rds.NewsItemID
                     where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                        (tmp.Duid == Constants.IMAGE_DUID || tmp.Duid == null) && itpcsm.IptcSubjectCode == Constants.NPB_ITPCSUBJECTCODE
                     orderby rds.View descending
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
                         TotalViews = rds.View.Value,
                         SubHeadline = brief.SubHeadline
                     } into news_photo
                     select news_photo).Take(4);
            return query;
        }
        #endregion


        #endregion

        #region Json Result

        #region Update Prediction
        /// <summary>
        /// Update point prediction :
        ///     1. Update expect point in table ExpectPoint.
        ///     2. Insert 2 records to table PointHistory.
        ///     3. Update possession point in table Point.
        /// </summary>
        /// <param name="expectPointID">ExpectPointID</param>
        /// <param name="pointID">PointID</param>
        /// <param name="newPoint">NewPoint</param>
        /// <param name="oldPoint">OldPoint</param>
        /// <returns>True : Update success 3 tables : Point, PointHistory, ExpectPoint.</returns>
        [HttpPost]
        public JsonResult UpdatePrediction(string expectPointID, string pointID, int newPoint, string oldPoint)
        {
            string strNewTotalPoint = string.Empty;
            using (var dbContextTransaction = com.Database.BeginTransaction())
            {
                try
                {
                    if (Session["CurrentUser"] != null)
                    {
                        var memberID = Session["CurrentUser"].ToString();
                        int oPoint = Convert.ToInt32(oldPoint);

                        //Decrypt expectPointID and pointID.
                        var expPointID = Convert.ToInt64(StringProtector.Unprotect(expectPointID));
                        var pID = Convert.ToInt64(StringProtector.Unprotect(pointID));

                        //1.Update expectPoint in table ExpectPoint
                        var expectRecord = com.ExpectPoint.Where(m => m.ExpectPointID == expPointID).FirstOrDefault();
                        if (expectRecord != null)
                        {
                            expectRecord.ExpectPoint1 = newPoint;
                            expectRecord.ModifiedAccountID = memberID;
                            expectRecord.ModifiedDate = DateTime.Now;
                        }

                        //2. Insert 2 records to table PointHistory.  
                        //2.1 Calculate range from old point to new point.
                        int range = 0;
                        int possessionPoint = com.Point.Where(m => m.PointID == pID).Select(m => m.PossesionPoint).FirstOrDefault();
                        int newPossPoint = 0;
                        bool isIncrease = false;
                        if (newPoint > oPoint)
                        {
                            range = newPoint - oPoint;
                            newPossPoint = possessionPoint - range;
                            isIncrease = true;
                        }
                        else
                        {
                            range = oPoint - newPoint;
                            newPossPoint = possessionPoint + range;
                        }

                        //2.2 Start insert 2 records  to table PointHistory.
                        //First record.
                        PointHistory his = new PointHistory();
                        his.PointId = pID;
                        his.ExpectPointId = expPointID;
                        his.CampaignId = null;
                        his.PointClass = 2;
                        his.Points = range;
                        his.HistoryClass = 2;
                        his.AdjustmentClass = (short)(isIncrease ? 2 : 1);
                        his.OperationClass = 1;
                        his.Status = true;
                        his.CreatedAccountID = memberID;
                        his.CreatedDate = DateTime.Now;
                        com.PointHistory.Add(his);

                        //Second record.
                        PointHistory his1 = new PointHistory();
                        his1.PointId = pID;
                        his1.ExpectPointId = expPointID;
                        his1.CampaignId = null;
                        his1.PointClass = 3;
                        his1.Points = range;
                        his1.HistoryClass = 2;
                        his1.AdjustmentClass = (short)(isIncrease ? 1 : 2);
                        his1.OperationClass = 1;
                        his1.Status = true;
                        his1.CreatedAccountID = memberID;
                        his1.CreatedDate = DateTime.Now;
                        com.PointHistory.Add(his1);

                        //3. Update possession point in table Point.
                        var newTablePoint = com.Point.Where(m => m.PointID == pID).FirstOrDefault();
                        newTablePoint.PossesionPoint = newPossPoint;
                        newTablePoint.ModifiedAccountID = memberID;
                        newTablePoint.ModifiedDate = DateTime.Now;

                        //Commit transaction.
                        com.SaveChanges();
                        dbContextTransaction.Commit();
                        strNewTotalPoint = newPossPoint.ToString();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }

            return Json(strNewTotalPoint, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cancel Prediction
        /// <summary>
        /// Cancel prediction : 3 step
        ///     1. Update record in table ExpectPoint.
        ///     2. Insert 2 records in table PointHistory.
        ///     3. Update possession point in table Point
        /// </summary>
        /// <param name="expectPointID">ExpectPointID</param>
        /// <param name="pointID">PointID</param>
        /// <returns>True : Cancel success.</returns>
        /// <returns>False : Cancel fail.</returns>
        public JsonResult CancelPrediction(string expectPointID, string pointID, int deletedPoint)
        {
            string strNewTotalPoint = string.Empty;
            using (var dbContextTransaction = com.Database.BeginTransaction())
            {
                try
                {
                    if (Session["CurrentUser"] != null)
                    {
                        var memberID = Session["CurrentUser"].ToString();
                        var expPointID = Convert.ToInt64(StringProtector.Unprotect(expectPointID));
                        var pID = Convert.ToInt64(StringProtector.Unprotect(pointID));

                        //1. Update record in table ExpectPoint.
                        var expectRecord = com.ExpectPoint.Where(m => m.ExpectPointID == expPointID).FirstOrDefault();
                        if (expectRecord != null)
                        {
                            expectRecord.SituationStatus = 2;
                            expectRecord.ModifiedAccountID = memberID;
                            expectRecord.ModifiedDate = DateTime.Now;
                        }

                        //2. Insert 2 records in table PointHistory.
                        //First record.
                        PointHistory his = new PointHistory();
                        his.PointId = pID;
                        his.ExpectPointId = expPointID;
                        his.CampaignId = null;
                        his.PointClass = 3;
                        his.Points = deletedPoint;
                        his.HistoryClass = 6;
                        his.AdjustmentClass = 2;
                        his.OperationClass = 1;
                        his.Status = true;
                        his.CreatedAccountID = memberID;
                        his.CreatedDate = DateTime.Now;
                        com.PointHistory.Add(his);

                        //Second record.
                        PointHistory his1 = new PointHistory();
                        his1.PointId = pID;
                        his1.ExpectPointId = expPointID;
                        his1.CampaignId = null;
                        his1.PointClass = 2;
                        his1.Points = deletedPoint;
                        his1.HistoryClass = 6;
                        his1.AdjustmentClass = 1;
                        his1.OperationClass = 1;
                        his1.Status = true;
                        his1.CreatedAccountID = memberID;
                        his1.CreatedDate = DateTime.Now;
                        com.PointHistory.Add(his1);

                        //3. Update possession point in table Point : add deleted point to possession point.
                        var newTablePoint = com.Point.Where(m => m.PointID == pID).FirstOrDefault();
                        var newPossPoint = newTablePoint.PossesionPoint + deletedPoint;
                        newTablePoint.PossesionPoint = newPossPoint;
                        newTablePoint.ModifiedAccountID = memberID;
                        newTablePoint.ModifiedDate = DateTime.Now;

                        com.SaveChanges();
                        dbContextTransaction.Commit();
                        strNewTotalPoint = newPossPoint.ToString();
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return Json(strNewTotalPoint, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}