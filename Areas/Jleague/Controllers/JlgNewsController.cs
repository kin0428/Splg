#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Jleague;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Models;
using Splg.Models.News.ViewModel;
using PagedList;

namespace Splg.Areas.Jleague.Controllers
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
    /// ViewBag.JleagueSubMenu = 7 => Jleague チーム情報: 10-2_6 ; 10_3_6
    /// </summary>
    #endregion

    public class JlgNewsController : Controller
    {
        #region Global variables
        JlgEntities jlg = new JlgEntities();
        ComEntities news = new ComEntities();
        #endregion

        #region Actions
        // GET: Jleague/JlgNews
        [HttpGet]
        public ActionResult Index(string jType, int? page)
        {

            ViewBag.jType = jType;
            var teamNewsList = default(IEnumerable<NewsInfoViewModel>);
            string[] jList = GetJNameList();
            if (!string.IsNullOrEmpty(jType))
            {
                switch (Convert.ToInt32(jType))
                {
                    case 0:
                        teamNewsList = GetTeamNewsList(JlgConstants.JLG_GAMEKIND_J1, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION);                     
                        break;
                    case 1:                         
                        teamNewsList = GetTeamNewsList(JlgConstants.JLG_GAMEKIND_J2, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION);
                        break;
                    case 2:                        
                        teamNewsList = GetTeamNewsList(JlgConstants.JLG_GAMEKIND_NABISCO, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION);
                        break;
                }
            }
            else
            {
                ViewBag.JleagueMenu = 5;
                ViewBag.jType = string.Empty;
                teamNewsList = GetJNewsList();
            }

            int pageNumber = (page ?? 1);
            JlgJNewsViewModel npbTeamNews = new JlgJNewsViewModel();
            npbTeamNews.JList = jList;
            npbTeamNews.TeamNewsList = teamNewsList.ToPagedList(pageNumber, GetPageSize(1));
            return View(npbTeamNews);
        }

        public ActionResult TeamInfoNews(int teamId, int? page)
        {
            int  jType=JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JType = jType;
            ViewBag.JleagueMenu = jType == 1 ? 2 : jType == 2 ? 3 : 4;
            ViewBag.JleagueSubMenu = 6;
            ViewBag.JleagueTeamMenu = 5;
            ViewBag.teamId = teamId;
            ViewBag.TeamInfoMenuTabActive = (int)JlgConstants.TeamInfoMenu.TabActive_5;
            var teamNames = (from t in jlg.TeamInfoTE
                                    where t.TeamID == teamId
                                select t).FirstOrDefault();
            if(teamNames != null)
                ViewBag.TeamName = teamNames.TeamName;

            var teamNewsList = GetTeamNewsList(teamId, JlgConstants.JLG_SPORT_ID, Constants.TEAM_TOPIC_CLASSIFICATION);
            int pageNumber = (page ?? 1);
            return View(teamNewsList.ToPagedList(pageNumber, GetPageSize(1)));
        }

        [HttpGet]
        public ActionResult JlgNews(string strTeamId, int? page)
        {
          
            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JType = jType;
            ViewBag.teamId = strTeamId;
            var teamNewsList = default(IEnumerable<NewsInfoViewModel>);
            if (!string.IsNullOrEmpty(strTeamId))
            {
                teamNewsList = GetTeamNewsList(Convert.ToInt32(strTeamId), JlgConstants.JLG_SPORT_ID, Constants.TEAM_TOPIC_CLASSIFICATION);
            }
            else
            {
                ViewBag.teamId = string.Empty;
                if (jType == 1)
                    teamNewsList = GetTeamNewsList(JlgConstants.JLG_GAMEKIND_J1, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION);
                else if(jType == 2)
                    teamNewsList = GetTeamNewsList(JlgConstants.JLG_GAMEKIND_J2, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION);
            }

            int pageNumber = (page ?? 1);
            JlgTeamNewsViewModel npbTeamNews = new JlgTeamNewsViewModel();
            if (jType == 1)
            {
                ViewBag.JleagueMenu = 2;
                npbTeamNews.TeamList = GetTeamNameList(2);
            }
            else if (jType == 2)
            {
                ViewBag.JleagueMenu = 3;
                npbTeamNews.TeamList = GetTeamNameList(6);
            }        
            ViewBag.JleagueSubMenu = 7;
            npbTeamNews.TeamNewsList = teamNewsList.ToPagedList(pageNumber, GetPageSize(1));
            return View(npbTeamNews);
        }

        public ActionResult JlgNabiscoNews(int? page)
        {
            //ViewBag.teamId = teamId;
            ViewBag.JleagueMenu = 4;
            ViewBag.JleagueSubMenu = 5;
            var teamNewsList = GetTeamNewsList(JlgConstants.JLG_GAMEKIND_NABISCO, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION);
            int pageNumber = (page ?? 1);
            return View(teamNewsList.ToPagedList(pageNumber, GetPageSize(1)));
        }
        #endregion

        #region Function: Get Page size from Com.SystemParameter
        private int GetPageSize(int scope)
        {
            int pageSize = 10;
            var spara = news.SystemParamater.Find(scope);
            if (spara != null)
                pageSize = Convert.ToInt32(spara.Spara);
            return pageSize;
        }
        #endregion

        #region Function: Get Team Name List
        private IEnumerable<TeamInfoGT> GetTeamNameList(int gameKindID)
        {
            var query = (from tgt in jlg.TeamInfoGT
                         join crt in jlg.CategoryGT on tgt.CategoryGTId equals crt.CategoryGTId
                        where crt.GameKindID == gameKindID
                         select tgt).Distinct().GroupBy(x => x.TeamID).Select(g => g.FirstOrDefault()).ToList();

            return query.OrderBy(x => x.TeamID);
       }
        #endregion

        #region Function: Get League Name List
        private string[] GetJNameList()
        {
            string[] jl= new string[3];
            jl[0] = "Ｊ１リーグ";
            jl[1] = "Ｊ２リーグ";
            jl[2] = "ナビスコＣ";
            return jl;
        }
        #endregion

        #region Get News List based on a TeamId
        /// <summary>
        /// Get all brief news in db with more infomation from another tables : image name, group,...
        /// Join columns in 4 tables to get 
        ///     1. BriefNews : NewsItemID, DeliveryDate, Headline, newstext
        ///     2. NewsTopics : NewsItemID, TopicID
        ///     3. TopicMasters : TopicID, SportID, ClassificationType,...
        /// </summary>
        /// <param name="teamId">id of news</param>
        /// <returns>List of news</returns>
        private IEnumerable<NewsInfoViewModel> GetTeamNewsList(int teamId, int sportID, int classification)
        {
            IEnumerable<NewsInfoViewModel> result = default(IEnumerable<NewsInfoViewModel>);
            var query = (from brief in news.BriefNews
                        join topic in news.NewsTopic on brief.NewsItemID equals topic.NewsItemID
                        join tm in news.TopicMaster on topic.TopicID equals tm.TopicID
                        join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                        //join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                        from tmp in br_photo.DefaultIfEmpty()
                        where (tm.ClassificationType == classification
                        && tm.SportID == sportID
                        && tm.UniqueID == teamId
                        //&& itpc.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
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
            result = query;
            return result.OrderByDescending(x => x.DeliveryDate);
        }
        #endregion
        #region Get News List based on a TeamId
        /// <summary>
        /// Get all brief news in db with more infomation from another tables : image name, group,...
        /// Join columns in 4 tables to get 
        ///     1. BriefNews : NewsItemID, DeliveryDate, Headline, newstext
        ///     2. NewsTopics : NewsItemID, TopicID
        ///     3. TopicMasters : TopicID, SportID, ClassificationType,...
        /// </summary>
        /// <param name="teamId">id of news</param>
        /// <returns>List of news</returns>
        private IEnumerable<NewsInfoViewModel> GetJNewsList()
        {
            IEnumerable<NewsInfoViewModel> result = default(IEnumerable<NewsInfoViewModel>);
            var query = (from brief in news.BriefNews
                         join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                         join photo in news.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                         from tmp in br_photo.DefaultIfEmpty()
                         where ( itpc.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
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
            result = query;
            return result.OrderByDescending(x => x.DeliveryDate);
        }
        #endregion
    }
}