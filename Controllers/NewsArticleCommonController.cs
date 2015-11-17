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
 * Namespace	: Splg.Controllers
 * Class		: NewsArticleCommonController
 * Developer	: Huynh Thi Phuong Cuc
 * 
 * Updated      : Sy Huynh
 */
#endregion

#region Using directives
using Splg.Models.News.ViewModel;
using Splg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
#endregion

namespace Splg.Controllers
{
    public class NewsArticleCommonController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        ComEntities news = new ComEntities();
        #endregion        

        #region Index
        /// <summary>
        /// Action for news detail
        /// </summary>
        /// <param name="topic">topic name: may "npb", "mlb", "jleague"</param>
        /// <param name="newsItemID">id of news which is displaied</param>
        /// <param name="sportID">id of sport: 1 - npb, 2-jleague, ...</param>
        /// <param name="uniqueID">gameID/leagueID when news item was clicked from /npb/game/{gameID} or /jleague/j1/ or ...</param>
        /// <param name="teamID"> when news item was clicked from /npb/teams/{teamID}/xxx/ or /jleague/j1/teams/{teamid}/news/ or ... </param>
        public ActionResult Index(string topic, long newsItemID, int sportID, string uniqueID, string teamID)
        {
            ViewBag.topic = topic;
            ViewBag.sportID = sportID;
            ViewBag.newsItemID = newsItemID;
            ViewBag.pageNO = "7-1";
            int teamLeagueFlag = 0;
            int teamId = -1;
            int leagueId = -1;

            if (!string.IsNullOrEmpty(uniqueID) && Int32.TryParse(uniqueID, out leagueId))
                ViewBag.uniqueID = uniqueID;
            if (!string.IsNullOrEmpty(teamID) && Int32.TryParse(teamID, out teamId))
                ViewBag.teamID = teamID;
            // for every sportID, if teamID exists then  do 12th proccessing
            if (teamId != -1 && ViewBag.teamID != null)
            {
                //for every sportID, do 12th proccessing
                teamLeagueFlag = Constants.TEAM_TOPIC_CLASSIFICATION;
            }
            else
            {
                // do 13th proccessing
                if (leagueId != -1 && ViewBag.uniqueID != null)
                {
                    if (sportID == 2 || sportID == 4)
                        teamLeagueFlag = Constants.LEAGUE_TOPIC_CLASSIFICATION;
                }
            }
            var queryNewsTop = new NewsInfoViewModel();
            switch(teamLeagueFlag)
            {
                case Constants.TEAM_TOPIC_CLASSIFICATION:
                    queryNewsTop = GetBriefNewsById(newsItemID, teamLeagueFlag, teamId);
                    break;
                case Constants.LEAGUE_TOPIC_CLASSIFICATION:
                    queryNewsTop = GetBriefNewsById(newsItemID, teamLeagueFlag, leagueId);
                    break;
                default:
                    queryNewsTop = GetBriefNewsById(newsItemID);
                    break;
            }
            NewsTopicViewModel newsTopic = new NewsTopicViewModel();
            if (queryNewsTop != null)
            {
                // Add a record to NewsReading
                SaveNewRecordToNewsReading(queryNewsTop.NewsItemID);

                // get Caption for main photo 
                var query = news.PhotoNews.Find(queryNewsTop.NewsItemID, Constants.IMAGE_CAPTION_DUID);
                if (query != null)
                {
                    queryNewsTop.Caption = query.Content;
                }
                newsTopic.NewsDisplayed = queryNewsTop;

                // get number of news will display on newsTopic.RelatedNews
                var spara = news.SystemParamater.Find(3);
                int relatedNewsSize = 5;
                if (spara != null)
                    relatedNewsSize = Convert.ToInt32(spara.Spara);
                // depend on teamLeagueFlag do the corresponding function for RelatedNews
                switch (teamLeagueFlag)
                {
                    case Constants.TEAM_TOPIC_CLASSIFICATION:
                        // team related news
                        newsTopic.RelatedNews = GetRelatedNews(queryNewsTop.NewsItemID, queryNewsTop.ItpcSubjectCode, teamLeagueFlag, teamId);
                        ViewBag.relatedNewsSize = relatedNewsSize;
                        break;
                    case Constants.LEAGUE_TOPIC_CLASSIFICATION:
                        // league related news
                        newsTopic.RelatedNews = GetRelatedNews(queryNewsTop.NewsItemID, queryNewsTop.ItpcSubjectCode, teamLeagueFlag, leagueId);
                        ViewBag.relatedNewsSize = relatedNewsSize;
                        break;
                    default:
                        // orther (no teamID, no uniqueID)
                        newsTopic.RelatedNews = GetRelatedNews(queryNewsTop.NewsItemID, queryNewsTop.ItpcSubjectCode);
                        break;
                }
                newsTopic.RelatedTopics = GetRelatedTopics(queryNewsTop.NewsItemID, sportID);
                newsTopic.RelatedPosts = PostedController.GetRecentPosts(Constants.NPB_POST_SPORT_TYPE, sportID, null);
                return View(newsTopic);
            }
            return View("Error410");
        }
        #endregion      

        #region Get Specific Brief news
        /// <summary>
        /// Get 1 newest brief news with image in current day or by NewsItemId.
        /// Join 2 table BriefNews, Photonews to get :
        /// 1. BriefNews : NewsItemID, Headline, SubHeadline, newstext, DeliveryDate, SentFrom.
        /// 2. PhotoNews : Content, Duid.
        /// </summary>
        /// <param name="newsItemId">id of news</param>
        /// <param name="isByDate">decide condition function</param>
        /// <returns>List of brief news</returns>
        public NewsInfoViewModel GetBriefNewsById(long? newsItemId = null, int teamLeagueFlag = 0, int uniqueID = -1)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            if (teamLeagueFlag == 0 || uniqueID == -1)
            {
                query = from brief in news.BriefNews
                        join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                        join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                        from tmp in br_photo.DefaultIfEmpty()
                        where (brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now)
                        select new NewsInfoViewModel
                        {
                            NewsItemID = brief.NewsItemID,
                            DeliveryDate = brief.DeliveryDate,
                            Headline = brief.Headline,
                            newstext = brief.newstext,
                            SentFrom = brief.SentFrom,
                            Duid = tmp.Duid,
                            Content = tmp.Content,
                            SubHeadline = brief.SubHeadline,
                            ItpcSubjectCode = itpc.IptcSubjectCode
                        } into news_photo
                        where (news_photo.NewsItemID == newsItemId
                        && (news_photo.Duid == Constants.IMAGE_DUID || news_photo.Duid == null))
                        orderby news_photo.DeliveryDate descending
                        select news_photo;
            }
            else
            {
                query = from brief in news.BriefNews
                        join topic in news.NewsTopic on brief.NewsItemID equals topic.NewsItemID
                        join tm in news.TopicMaster on topic.TopicID equals tm.TopicID
                        join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                        from tmp in br_photo.DefaultIfEmpty()
                        where (brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now
                            && tm.ClassificationType == teamLeagueFlag && tm.UniqueID == uniqueID)
                        select new NewsInfoViewModel
                        {
                            NewsItemID = brief.NewsItemID,
                            DeliveryDate = brief.DeliveryDate,
                            Headline = brief.Headline,
                            newstext = brief.newstext,
                            SentFrom = brief.SentFrom,
                            Duid = tmp.Duid,
                            Content = tmp.Content,
                            SubHeadline = brief.SubHeadline,
                        } into news_photo
                        where (news_photo.NewsItemID == newsItemId
                        && (news_photo.Duid == Constants.IMAGE_DUID || news_photo.Duid == null))
                        orderby news_photo.DeliveryDate descending
                        select news_photo;
            }
            return query.FirstOrDefault();
        }
        #endregion

        #region Get Related news
        /// <summary>
        /// Get all brief news in db with more infomation from another tables : image name, group,...
        /// Join columns in 4 tables to get 
        ///     1. BriefNews : NewsItemID, DeliveryDate, Headline, newstext
        ///     2. PhotoNews : NewsItemID, Classification, Content
        ///     3. NewsGenre : NewsItemID, LargeCategory, SmallCategory
        /// </summary>
        /// <param name="newsItemId">id of news</param>
        /// <param name="largeCategoryEn">condition input</param>
        /// <returns>New table NewsInfoViewModel that related with id input</returns>
        public IEnumerable<NewsInfoViewModel> GetRelatedNews(long newsItemId, int itpcSubjectCode, int teamLeagueFlag = 0, int uniqueID = -1)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            if (teamLeagueFlag == 0 || uniqueID == -1)
            {
                var queryTmp = (from brief in news.BriefNews
                        join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                        join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                        from tmp in br_photo.DefaultIfEmpty()
                        where (brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now)
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
                            SubHeadline = brief.SubHeadline
                        } into news_photo
                        where ((news_photo.Duid == Constants.IMAGE_THUMNAIL_DUID || news_photo.Duid == null)
                        && (news_photo.NewsItemID != newsItemId)
                        && (news_photo.ItpcSubjectCode == itpcSubjectCode))
                        orderby news_photo.DeliveryDate descending
                        select news_photo).Distinct().ToList();
                query = queryTmp.OrderByDescending(d => d.DeliveryDate);
            }
            else
            {
                var queryTmp = (from brief in news.BriefNews
                        join topic in news.NewsTopic on brief.NewsItemID equals topic.NewsItemID
                        join tm in news.TopicMaster on topic.TopicID equals tm.TopicID
                        //join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                        join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                        from tmp in br_photo.DefaultIfEmpty()
                        where (brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now
                            && tm.ClassificationType == teamLeagueFlag && tm.UniqueID == uniqueID)
                        select new NewsInfoViewModel
                        {
                            NewsItemID = brief.NewsItemID,
                            DeliveryDate = brief.DeliveryDate,
                            Headline = brief.Headline,
                            newstext = brief.newstext,
                            Content = tmp.Content,
                            Duid = tmp.Duid,
                            SentFrom = brief.SentFrom,
                            SubHeadline = brief.SubHeadline
                            //ItpcSubjectCode = itpc.IptcSubjectCode
                        } into news_photo
                        where ((news_photo.Duid == Constants.IMAGE_THUMNAIL_DUID || news_photo.Duid == null)
                        && (news_photo.NewsItemID != newsItemId)
                        //&& (news_photo.ItpcSubjectCode == itpcSubjectCode)
                        )
                        orderby news_photo.DeliveryDate descending
                        select news_photo).Distinct().ToList();
                query = queryTmp.OrderByDescending(d => d.DeliveryDate);
            }
            return query;
        }
        #endregion

        #region Get Related Topics
        /// <summary>
        /// Get all topics that realted to a brief news
        /// Join columns in 3 tables to get 
        ///     1. BriefNews : NewsItemID, DeliveryDate, Headline, newstext
        ///     2. NewsTopics : NewsItemID, TopicID
        ///     3. TopicMasters : TopicID, SportID, ClassificationType,...
        /// </summary>
        /// <param name="newsItemId">id of news</param>
        /// <returns>List of topicmaster that has the same id of news</returns>
        public IEnumerable<TopicMaster> GetRelatedTopics(long newsItemId, int sportID = 0)
        {
            var query = from brief in news.BriefNews
                        join topic in news.NewsTopic on brief.NewsItemID equals topic.NewsItemID
                        join tm in news.TopicMaster on topic.TopicID equals tm.TopicID
                        where (brief.NewsItemID == newsItemId)// && ((sportID == 0) ? true : (tm.SportID == sportID)))
                        select tm;

            return query;
        }
        #endregion

        #region Save NewsReading to Database
        /// <summary>
        /// When a news is viewed, a record is added to database for compute Top view of news
        /// </summary>
        /// <param name="newsItemId">id of news</param>
        /// <returns>void</returns>
        private void SaveNewRecordToNewsReading(long newsItemId)
        {
            NewsReading newsReading = new NewsReading();
            newsReading.NewsItemID = newsItemId;
            newsReading.CreatedDate = DateTime.Now;
            news.NewsReading.Add(newsReading);
            news.SaveChanges();
        }
        #endregion
    }
}