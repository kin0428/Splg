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
 * Namespace	: Splg.Areas.Mlb.Controllers
 * Class		: MlbTeamInfoNewsController
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directives
using Splg.Models.News.ViewModel;
using Splg.Models;
using Splg.Areas.Mlb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
#endregion

namespace Splg.Areas.Mlb.Controllers
{
    public class MlbTeamInfoNewsController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        ComEntities news = new ComEntities();
        MlbEntities mlb = new MlbEntities();

        #endregion
        // GET: Mlb/MlbTeamNews
        public ActionResult Index(int teamId, int? page)
        {
            ViewBag.teamId = teamId;
            ViewBag.TeamName = Utils.GetTeamNameById(Constants.MLB_SPORT_ID, teamId);
            ViewBag.TeamInfoMenuTabActive = (int)MlbConstants.TeamInfoMenu.TabActive_6;
            var teamNewsList = GetTeamNewsList(teamId);
            var spara = news.SystemParamater.Find(1);
            int pageSize = 10;
            if (spara != null)
                pageSize = Convert.ToInt32(spara.Spara);
            int pageNumber = (page ?? 1);
            return View(teamNewsList.ToPagedList(pageNumber, pageSize));
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
        public IEnumerable<NewsInfoViewModel> GetTeamNewsList(int? teamId)
        {
            var query = from brief in news.BriefNews
                        join topic in news.NewsTopic on brief.NewsItemID equals topic.NewsItemID
                        join tm in news.TopicMaster on topic.TopicID equals tm.TopicID
                        join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                        join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                        from tmp in br_photo.DefaultIfEmpty()
                        where (tm.ClassificationType == Constants.TEAM_TOPIC_CLASSIFICATION
                        && tm.UniqueID == teamId
                        && itpc.IptcSubjectCode == Constants.MLB_ITPCSUBJECTCODE
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
                            SentFrom = brief.SentFrom
                        } into news_photo
                        where (news_photo.Duid == Constants.IMAGE_THUMNAIL_DUID || news_photo.Duid == null)
                        orderby news_photo.DeliveryDate descending
                        select news_photo;

            return query;
        }
        #endregion
    }
}