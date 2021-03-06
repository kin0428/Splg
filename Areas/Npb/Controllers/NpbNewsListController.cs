﻿#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Areas.Npb.Controllers
 * Class		: NpbNewsListController
 * Developer	: Sy Huynh
 * 
 */
#endregion

#region Using directives
using Splg.Models.News.ViewModel;
using Splg.Models;
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbNewsListController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        ComEntities news = new ComEntities();
        NpbEntities npb = new NpbEntities();

        #endregion
        // GET: Npb/NpbNews
        [HttpGet]
        public ActionResult Index(string strTeamId, int? page)
        {
            ViewBag.teamId = strTeamId;
            var teamNewsList = default(IEnumerable<NewsInfoViewModel>);
            if (!string.IsNullOrEmpty(strTeamId))
            {
                teamNewsList = GetTeamNewsList(Convert.ToInt32(strTeamId));
            }
            else
            {
                ViewBag.teamId = string.Empty;
                teamNewsList = GetTeamNewsList();
            }
            var spara = news.SystemParamater.Find(1);
            int pageSize = 10;
            if (spara != null)
                pageSize = Convert.ToInt32(spara.Spara);

            int pageNumber = (page ?? 1);
            NpbTeamNewsViewModel npbTeamNews = new NpbTeamNewsViewModel();
            npbTeamNews.TeamList = (from t in npb.TeamInfoMST
                                   where t.LeagueID == 1 || t.LeagueID == 2
                                   select t).OrderBy(i => i.TeamCD);
            npbTeamNews.TeamNewsList = teamNewsList.ToPagedList(pageNumber, pageSize);
            return View(npbTeamNews);
        }

        #region Get News List based on a TeamId
        /// <summary>
        /// Get all brief news in db with more infomation from another tables : image name, group,...
        /// Join columns in 4 tables to get 
        ///     1. BriefNews : NewsItemID, DeliveryDate, Headline, newstext
        ///     2. NewsTopics : NewsItemID, TopicID
        ///     3. TopicMasters : TopicID, SportID, ClassificationType,...
        /// </summary>
        /// <param name="teamId">id of news</param>
        /// <returns>List of topicmaster that has the same id of news</returns>
        public IEnumerable<NewsInfoViewModel> GetTeamNewsList(int? teamId = null)
        {
            IEnumerable<NewsInfoViewModel> query = default(IEnumerable<NewsInfoViewModel>);
            if(teamId != null)
            {
                query = from brief in news.BriefNews
                            join topic in news.NewsTopic on brief.NewsItemID equals topic.NewsItemID
                            join tm in news.TopicMaster on topic.TopicID equals tm.TopicID
                            join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                            //join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                            from tmp in br_photo.DefaultIfEmpty()
                            where (tm.ClassificationType == Constants.TEAM_TOPIC_CLASSIFICATION
                            && tm.UniqueID == teamId
                            //&& itpc.IptcSubjectCode == Constants.NPB_ITPCSUBJECTCODE
                            && brief.Status == Constants.NEWS_VALID_STATUS
                            && brief.CarryLimitDate >= DateTime.Now)
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
                                } into news_photo
                                where (news_photo.Duid == Constants.IMAGE_THUMNAIL_DUID || news_photo.Duid == null)
                                orderby news_photo.DeliveryDate descending
                                select news_photo;

            }
            else
            {
                var queryTemperary = (from brief in news.BriefNews
                        join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                        join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                        from tmp in br_photo.DefaultIfEmpty()
                        where (itpc.IptcSubjectCode == Constants.NPB_ITPCSUBJECTCODE
                        && brief.Status == Constants.NEWS_VALID_STATUS
                        && brief.CarryLimitDate >= DateTime.Now)
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
                        } into news_photo
                        where (news_photo.Duid == Constants.IMAGE_THUMNAIL_DUID || news_photo.Duid == null)
                        select news_photo).Distinct().ToList();
                query = queryTemperary;
            }

            return query.OrderByDescending(n => n.DeliveryDate);
        }
        #endregion
    }
}