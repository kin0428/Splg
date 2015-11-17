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
 * Class		: MlbRightContentController
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Mlb.Models;
using Splg.Areas.Mlb.Models.ViewModels;
using Splg.Areas.Mlb.Models.ViewModels.InfosModel;
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
#endregion

namespace Splg.Areas.Mlb.Controllers
{
    /// <summary>
    /// Controller for all mlb right content : top3 ranking, my point,...
    /// Use for each control in PartialView : MlbRightContent 
    /// </summary>
    public class MlbRightContentController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Mlb to get db.
        /// </summary>
        MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context News to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        #endregion

        #region Partial View

        #region Show TopN Ranking
        /// <summary>
        /// Show mlb top N ranking in partial view.
        /// </summary>
        /// <returns>PartialView : _MlbTop3Ranking</returns>        
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 5)]
        public ActionResult ShowTopNRanking(int? teamID)
        {
            MlbTop3RankingViewModel ranking = new MlbTop3RankingViewModel();
            if (teamID == null || teamID == 0)
            {
                List<MlbDivisionGroupModel> lstGameType = GetGameAssortmentInOfficialStats();
                foreach (var item in lstGameType)
                {
                    if (item.DivID == (int)MlbConstants.DivGroup.E && item.LeagueID == (int)MlbConstants.GameAssortment.A)
                    {
                        //アメリカン・リーグ 東
                        ranking.AmericanEast = GetTopNRankingByType(item.DivGroupMlbId,3);
                    }
                    else if (item.DivID == (int)MlbConstants.DivGroup.C && item.LeagueID == (int)MlbConstants.GameAssortment.A)
                    {
                        //アメリカン・リーグ　中
                        ranking.AmericanCenteral = GetTopNRankingByType(item.DivGroupMlbId, 3);
                    }
                    else if (item.DivID == (int)MlbConstants.DivGroup.W && item.LeagueID == (int)MlbConstants.GameAssortment.A)
                    {
                        //アメリカン・リーグ　西
                        ranking.AmericanWest = GetTopNRankingByType(item.DivGroupMlbId, 3);
                    }
                    else if (item.DivID == (int)MlbConstants.DivGroup.E && item.LeagueID == (int)MlbConstants.GameAssortment.Na)
                    {
                        //ナショナル・リーグ　東
                        ranking.NationalEast = GetTopNRankingByType(item.DivGroupMlbId, 3);
                    }
                    else if (item.DivID == (int)MlbConstants.DivGroup.C && item.LeagueID == (int)MlbConstants.GameAssortment.Na)
                    {
                        //ナショナル・リーグ　中
                        ranking.NationalCenteral = GetTopNRankingByType(item.DivGroupMlbId, 3);
                    }
                    else if (item.DivID == (int)MlbConstants.DivGroup.W && item.LeagueID == (int)MlbConstants.GameAssortment.Na)
                    {
                        //ナショナル・リーグ　西
                        ranking.NationalWest = GetTopNRankingByType(item.DivGroupMlbId, 3);
                    }
                }
            }
            else
            {
                MlbDivisionGroupModel item = GetGameAssortmentByTeamId(teamID);

                if (item.DivID == (int)MlbConstants.DivGroup.E && item.LeagueID == (int)MlbConstants.GameAssortment.A)
                {
                    //アメリカン・リーグ 東
                    ranking.AmericanEast = GetTopNRankingByType(item.DivGroupMlbId, 6);
                }
                else if (item.DivID == (int)MlbConstants.DivGroup.C && item.LeagueID == (int)MlbConstants.GameAssortment.A)
                {
                    //ナショナル・リーグ　中
                    ranking.AmericanCenteral = GetTopNRankingByType(item.DivGroupMlbId, 6);
                }
                else if (item.DivID == (int)MlbConstants.DivGroup.W && item.LeagueID == (int)MlbConstants.GameAssortment.A)
                {
                    //ナショナル・リーグ　西
                    ranking.AmericanWest = GetTopNRankingByType(item.DivGroupMlbId, 6);
                }
                else if (item.DivID == (int)MlbConstants.DivGroup.E && item.LeagueID == (int)MlbConstants.GameAssortment.Na)
                {
                    //ナショナル・リーグ　東
                    ranking.NationalEast = GetTopNRankingByType(item.DivGroupMlbId, 6);
                }
                else if (item.DivID == (int)MlbConstants.DivGroup.C && item.LeagueID == (int)MlbConstants.GameAssortment.Na)
                {
                    //ナショナル・リーグ　中
                    ranking.NationalCenteral = GetTopNRankingByType(item.DivGroupMlbId, 6);
                }
                else if (item.DivID == (int)MlbConstants.DivGroup.W && item.LeagueID == (int)MlbConstants.GameAssortment.Na)
                {
                    //ナショナル・リーグ　西
                    ranking.NationalWest = GetTopNRankingByType(item.DivGroupMlbId, 6);
                }
            }
            return PartialView("~/Areas/Mlb/Views/Shared/_MlbTop3Ranking.cshtml", ranking);
        }
        #endregion

        #region Show Mlb RecentNews
        /// <summary>
        /// Show top 5 mlb recent news in day. 
        /// </summary>
        /// <returns>PartialView : _MlbRecentNews</returns>
        [ChildActionOnly]
        public ActionResult ShowMlbRecentNews(string type, int? teamID, int? gameID, string homeVisitor)
        {
            if (teamID != null)
            {
                ViewBag.TeamID = teamID.Value;
                ViewBag.TeamName = mlb.TeamInfo.Where(x => x.TeamID == teamID).Select(x => x.TeamName).FirstOrDefault();
            }
            if (gameID != null)
            {
                ViewBag.GameID = gameID;
            }
            if (!string.IsNullOrEmpty(homeVisitor))
            {
                ViewBag.HomeVisitor = homeVisitor;
            }
            IEnumerable<NewsInfoViewModel> result = GetMlbNews(type, teamID);
            ViewBag.Name = type;
            return PartialView("_MlbRecentNews", result);
        }
        #endregion

        #region Show Mlb RecentNews at the right of page
        /// <summary>
        /// Show top 5 mlb recent news in day. 
        /// </summary>
        /// <returns>PartialView : _MlbRecentNews</returns>
        [ChildActionOnly]
        public ActionResult ShowMlbRightRecentNews(string type)
        {
            IEnumerable<NewsInfoViewModel> result = GetMlbNews(Constants.MLB_TOP_INDEX, null);
            ViewBag.Name = type;
            return PartialView("_MlbRightRecentNews", result);
        }
        #endregion

        #region Show PersonInfo
        /// <summary>
        /// Show person info of 1 player.
        /// </summary>
        /// <param name="playerID">PlayerID</param>
        /// <returns>PartialView : _MlbPersonInfo</returns>
        //[OutputCache(NoStore = true, Location = OutputCacheLocation.Client)] // every 3 sec
        //public ActionResult ShowPersonInfo(int? teamID, int? playerID)
        //{
        //    MlbPersonInfoViewModel personInfo = GetPersonInfoByPlayerID(teamID, playerID);
        //    return PartialView("_MlbPersonInfo", personInfo);
        //}
        #endregion

        #region Show HittingStats
        /// <summary>
        /// Show hitting stats by teamID.
        /// </summary>
        /// <param name="teamID">TeamID</param>
        /// <returns>Hittingstats info of player.</returns>
        //[OutputCache(NoStore = true, Location = OutputCacheLocation.Client)] // every 3 sec
        //public ActionResult ShowHittingStats(int? teamID, int? playerID)
        //{
        //    HittingStats hittingStats = null;
        //    if (teamID != null && playerID != null)
        //    {
        //        hittingStats = GetHittingStatsByPlayerID(teamID.Value, playerID.Value);
        //    }
        //    return PartialView("_MlbBattingResult", hittingStats);
        //}
        #endregion

        #region Show Mlb News Top Views
        /// <summary>
        /// Show top 4 mlb news that have max views. 
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult ShowMlbNewsTopViews()
        {
            IEnumerable<NewsInfoViewModel> result = GetMlbNewTopViews();
            return PartialView("_MlbNewsTopView", result);
        }
        #endregion

        #endregion

        #region Utilities

        #region Get GameAssortment In OfficialStats Header
        /// <summary>
        /// Get gameassortment (gametype) in table OfficialStatsHeader
        /// </summary>
        /// <returns>List game type need get.</returns>
        public List<MlbDivisionGroupModel> GetGameAssortmentInOfficialStats()
        {
            var query = from d in  mlb.DivGroupMlb
                        join l in mlb.LeagueGroupMlb
                        on d.LeagueGroupMlbId equals l.LeagueGroupMlbId
                  orderby d.DivID, d.DivGroupMlbId
                  select new MlbDivisionGroupModel
                  {
                       LeagueID = l.LeagueID,
                       DivID =  d.DivID,
                       LeagueGroupMlbId = d.LeagueGroupMlbId,
                        DivGroupMlbId = d.DivGroupMlbId

                   };

            return query.Distinct().ToList();
        }


        public MlbDivisionGroupModel GetGameAssortmentByTeamId( int? teamId)
        {
            var query = (from d in mlb.DivGroupMlb
                        join t in mlb.TeamInfo
                        on d.DivID equals t.DivID
                         join l in mlb.LeagueGroupMlb
                         on d.LeagueGroupMlbId equals l.LeagueGroupMlbId
                        where t.TeamID == teamId
                        select new MlbDivisionGroupModel
                        {
                            LeagueID = l.LeagueID,
                            DivID = d.DivID,
                            LeagueGroupMlbId = d.LeagueGroupMlbId,
                            DivGroupMlbId = d.DivGroupMlbId

                        }).First();

            return query;
        }
        #endregion

        #region Get TopN Ranking By Type
        /// <summary>
        /// Get top N ranking by type of mlb.
        /// </summary>
        /// <param name="leagueID">GameType</param>
        /// <returns>List top N team have win highest.</returns>
        public IEnumerable<OfficialStatsMlb> GetTopNRankingByType(long LeagueGroupMlbId, int topN)
        {
            var query = default(IEnumerable<OfficialStatsMlb>);
                query = (mlb.OfficialStatsMlb.Where(m => m.DivGroupMlbId == LeagueGroupMlbId).OrderBy(p => p.Ranking)).Take(topN);

                if (query == null)
                    query = new List<OfficialStatsMlb>();
           
            return query;
        }
        #endregion

        #region Get Mlb Recent News
        /// <summary>
        /// Get top 5 mlb recent news.
        /// </summary>
        /// <returns>List news.</returns>
        public IEnumerable<NewsInfoViewModel> GetMlbNews(string type, int? teamID)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            var imageContent = Constants.IMAGE_THUMNAIL_DUID;
            if (!string.IsNullOrEmpty(type))
            {
                if (type == Constants.MLB_TOP_INDEX || type == Constants.MLB_ORDER_INDEX)
                {
                    imageContent = Constants.IMAGE_DUID;
                    query = (from brief in com.BriefNews
                             join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                             join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                             join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                             from tmp in br_photo.DefaultIfEmpty()
                             where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                                (tmp.Duid == imageContent || tmp.Duid == null) && itpcsm.IptcSubjectCode == Constants.MLB_ITPCSUBJECTCODE
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
                else if (type == Constants.MLB_TEAMINFO_TEAMTOP  || type == Constants.MLB_GAME_INFORMATION) //|| type == Constants.MLBTEAMINFOPITCHER || type == Constants.MLBTEAMINFOBATCHER)
                {
                    if (type == Constants.MLB_TEAMINFO_TEAMTOP)
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
        //public MlbPersonInfoViewModel GetPersonInfoByPlayerID(int? teamID, int? playerID)
        //{
        //    var query = default(MlbPersonInfoViewModel);
        //    if (playerID != null && teamID != null)
        //    {
        //        query = (from pi in mlb.PersonInfo
        //                 join d in mlb.Directory on pi.DirectoryId equals d.DirectoryId
        //                 join ti in mlb.TeamIconMlb on d.TeamID equals ti.TeamCD
        //                 where pi.ID == playerID && d.TeamID == teamID
        //                 select new MlbPersonInfoViewModel
        //                 {
        //                     PersonInfo = pi,
        //                     TeamID = d.TeamID,
        //                     TeamIcon = ti.TeamIcon,
        //                     TeamName = d.TeamName
        //                 }).FirstOrDefault();
        //    }
        //    return query;
        //}
        #endregion

        #region Get HittingStats By PlayerID
        /// <summary>
        /// Get hittingstats by PlayerID
        /// </summary>
        /// <param name="TeamID">PlayerID.</param>
        /// <returns>HittingStats Info.</returns>
        //public HittingStats GetHittingStatsByPlayerID(int teamID, int playerID)
        //{
        //    var query = mlb.HittingStats.Where(m => m.PlayerCD == playerID && m.TeamCD == teamID).FirstOrDefault();
        //    return query;
        //}
        #endregion

        #region Get MlbNews Top Views
        /// <summary>
        /// Get top 4 mlb news that have max views.
        /// </summary>
        /// <returns>List 4 mlb news.</returns>
        public IEnumerable<NewsInfoViewModel> GetMlbNewTopViews()
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
                        (tmp.Duid == Constants.IMAGE_DUID || tmp.Duid == null) && itpcsm.IptcSubjectCode == Constants.MLB_ITPCSUBJECTCODE
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

        #region Get NextDate
        /// <summary>
        /// Get next date if have game.
        /// </summary>
        /// <param name="gameDate">GameDate.</param>
        /// <returns>NextDate.</returns>
        [HttpPost]
        public JsonResult GetNextDate(int gameDate)
        {
            int? nxtDate = 0;
            nxtDate = mlb.DayGroup.Where(m => m.GameDateJPN > gameDate).Min(m => m.GameDateJPN);
            return Json(nxtDate, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}