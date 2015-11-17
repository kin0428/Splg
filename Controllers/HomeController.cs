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
 * Class		: HomeController
 * Developer	: Sy Huynh
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Splg;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Npb.Models;
using Splg.Models;
using Splg.Models.News.ViewModel;
using Splg.Models.ViewModel;
using Splg.Services.Game;
using Splg.Services.Members;
using Splg.Services.Point;
using Splg.Services.System;

namespace Splg.Controllers
{
    public class HomeController : Controller
    {
        #region Global Properties

        //Create context to get value from db.      
        ComEntities com = new ComEntities();
        NpbEntities npb = new NpbEntities();
        JlgEntities jlg = new JlgEntities();
        #endregion

        #region actions

        //[Authorize]
        public ActionResult Index()
        {
            string debug_mode = Request["debug_mode"];

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.HomeTopNews = GetHomeNewsTopViews().ToList();

            //QA#329 TOPページ pages改善版をリリース
            debug_mode = "1";

            if (debug_mode == null)
            {
                //以前と同じコード
                homeViewModel.HomeGameInfoViewModels = GetHomeTopPointGame();

                List<HomeContentLeagueViewModel> homeContentLeagueViewModel = new List<HomeContentLeagueViewModel>();
                homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.NPB_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.NPB_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.NPB_SPORT_ID), HomeRecentPost = PostedController.GetRecentPosts(0, Constants.NPB_SPORT_ID, null, null), GameID = GetHomeTopPointGameOfSport(Constants.NPB_SPORT_ID) });
                homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.JLG_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.JLEAGUE_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.JLG_SPORT_ID), HomeRecentPost = PostedController.GetRecentPosts(0, Constants.JLG_SPORT_ID, null, null), GameID = GetHomeTopPointGameOfSport(Constants.JLG_SPORT_ID) });
                homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.MLB_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.MLB_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.MLB_SPORT_ID), GameID = GetHomeTopPointGameOfSport(Constants.MLB_SPORT_ID), HomeRecentPost = PostedController.GetRecentPosts(0, Constants.MLB_SPORT_ID, null, null) });
                //homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.WS_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.OVERSEA_SOCCER_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.WS_SPORT_ID), GameID = GetHomeTopPointGameOfSport(Constants.WS_SPORT_ID), HomeRecentPost = default(IEnumerable<PostedInfoViewModel>) }); //PostedController.GetRecentPosts(0, Constants.WS_SPORT_ID, null, null) });
                homeViewModel.HomeContentLeagueViewModel = homeContentLeagueViewModel;
            }
            else
            {
                //最適化したコード
                homeViewModel.HomeGameInfoViewModels = GetHomeTopPointGame2().ToList();

                List<HomeContentLeagueViewModel> homeContentLeagueViewModel = new List<HomeContentLeagueViewModel>();
                homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.NPB_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.NPB_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.NPB_SPORT_ID), HomeRecentPost = Posted2Controller.GetRecentPosts(0, Constants.NPB_SPORT_ID, null, null), GameID = GetHomeTopPointGameOfSport(Constants.NPB_SPORT_ID) });
                homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.JLG_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.JLEAGUE_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.JLG_SPORT_ID), HomeRecentPost = Posted2Controller.GetRecentPosts(0, Constants.JLG_SPORT_ID, null, null), GameID = GetHomeTopPointGameOfSport(Constants.JLG_SPORT_ID) });
                homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.MLB_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.MLB_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.MLB_SPORT_ID), GameID = GetHomeTopPointGameOfSport(Constants.MLB_SPORT_ID), HomeRecentPost = Posted2Controller.GetRecentPosts(0, Constants.MLB_SPORT_ID, null, null) });
                //homeContentLeagueViewModel.Add(new HomeContentLeagueViewModel { SportID = Constants.WS_SPORT_ID, HomeRecentNews = GetHomeRecentNews(Constants.OVERSEA_SOCCER_ITPCSUBJECTCODE), HomeRecentMatch = GetRecentMatch(Constants.WS_SPORT_ID), GameID = GetHomeTopPointGameOfSport(Constants.WS_SPORT_ID), HomeRecentPost = default(IEnumerable<PostedInfoViewModel>) }); //PostedController.GetRecentPosts(0, Constants.WS_SPORT_ID, null, null) });
                homeViewModel.HomeContentLeagueViewModel = homeContentLeagueViewModel;
            }


            return View(homeViewModel);
        }

        public ActionResult GoogleCse()
        {
            return View();
        }

        /// <summary>
        /// Get: /Home/ShowPointAlert
        /// </summary>
        /// <returns></returns> 
        public ActionResult ShowPointAlert()
        {
            var memberId = (Convert.ToInt64(Session["CurrentUser"]));

            var pointInfoService = new PointInfoService(com);

            var memberModel = pointInfoService.GetMemberWithOnlinePoints(memberId, (short)DateTime.Now.Year, (short)DateTime.Now.Month);

            return PartialView("_PointAlert", memberModel);
        }
        #endregion

        #region Utilities

        #region Get News Top Views
        /// <summary>
        /// Get top 4(Max View) or 6(Newest news) jlg news that have 
        /// </summary>
        /// <returns>List of news.</returns>
        private IEnumerable<NewsInfoViewModel> GetHomeNewsTopViews()
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            try
            {
                var viewsCount = (from r in com.NewsReading
                                  select r).GroupBy(x => x.NewsItemID).Select(
                                        l => new
                                        {
                                            NewsItemId = l.Key,
                                            ReadingSum = l.Count()
                                        });
                query = (from brief in com.BriefNews
                         join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                         join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                         from tmp in br_photo.DefaultIfEmpty()
                         join rds in com.NewsReadingSumView on brief.NewsItemID equals rds.NewsItemID
                         join v in viewsCount on brief.NewsItemID equals v.NewsItemId
                         where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                         (tmp.Duid == Constants.IMAGE_DUID || tmp.Duid == null)
                         && (itpc.IptcSubjectCode == Constants.NPB_ITPCSUBJECTCODE || itpc.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
                            || itpc.IptcSubjectCode == Constants.MLB_ITPCSUBJECTCODE)// || itpc.IptcSubjectCode == Constants.OVERSEA_SOCCER_ITPCSUBJECTCODE)
                         select new NewsInfoViewModel
                         {
                             NewsItemID = brief.NewsItemID,
                             DeliveryDate = brief.DeliveryDate,
                             Headline = brief.Headline,
                             newstext = brief.newstext,
                             Content = tmp.Content,
                             Duid = tmp.Duid,
                             SentFrom = brief.SentFrom,
                             TotalViews = v.ReadingSum,
                             TotalViewsInOneHour = rds.View ?? 0,
                             ItpcSubjectCode = itpc.IptcSubjectCode,
                             SubHeadline = brief.SubHeadline
                         } into news_photo
                         //orderby news_photo.TotalViews descending
                         select news_photo).GroupBy(n => n.NewsItemID).Select(i => i.OrderBy(s => s.ItpcSubjectCode).FirstOrDefault());
                if (query != null)
                    return query.OrderByDescending(x => x.TotalViewsInOneHour).Take(4);
            }
            catch
            {
                return query;
            }
            return query;

        }
        #endregion

        #region Get Recent News Views based on type of game
        /// <summary>
        /// Get top 4(Max View) or 6(Newest news) jlg news that have 
        /// </summary>
        /// <returns>List of news.</returns>
        private IEnumerable<NewsInfoViewModel> GetHomeRecentNews(int itpcSubject)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            query = (from brief in com.BriefNews
                     join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                     join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                     from tmp in br_photo.DefaultIfEmpty()
                     where brief.Status == Constants.NEWS_VALID_STATUS
                     && brief.CarryLimitDate >= DateTime.Now
                     && (tmp.Duid == Constants.IMAGE_THUMNAIL_DUID || tmp.Duid == null)
                     && itpc.IptcSubjectCode == itpcSubject
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
                     orderby news_photo.DeliveryDate descending
                     select news_photo).Take(6);
            return query;
        }
        #endregion

        #region Get Top Point of Games for any sport
        /// <summary>
        /// Get Top 5 points that point of any sport.
        /// </summary>
        /// <returns>List top 5 points.</returns>
        private IEnumerable<HomeGameInfoViewModel> GetHomeTopPointGame()
        {
            var sDate = DateTime.Now.Date;
            var expectPointGroup = (from ep in com.ExpectPoint
                                    join et in com.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                                    where DbFunctions.TruncateTime(et.StartScheduleDate) >= sDate && et.ClassClass == 4
                                            && ep.SituationStatus == 1 //TODO SituationStatus 要確認（これは終了前の試合だけ対象で良い？）
                                    select ep).GroupBy(x => x.ExpectTargetID).Select(
                                        l => new
                                        {
                                            ExpectTargetID = l.Key,
                                            TotalPoint = l.Sum(p => p.ExpectPoint1)
                                        }).ToList();

            var query = from ep in expectPointGroup
                        join et in com.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                        orderby ep.TotalPoint descending
                        select new HomeGameInfoViewModel
                        {
                            SportID = et.SportsID,
                            GameID = et.UniqueID,
                        } into game_info
                        select game_info;

            return query.Take(5);
        }


        private IEnumerable<HomeGameInfoViewModel> GetHomeTopPointGame2()
        {

            string queryStr = "";
            queryStr += "SELECT ep.ExpectTargetID, ";
            queryStr += "       et.SportsID as SportID, ";
            queryStr += "       et.UniqueID         AS GameID, ";
            queryStr += "       Sum(ep.expectpoint) AS ExpectPoint ";
            queryStr += "FROM   com.expectpoint AS ep ";
            queryStr += "       INNER JOIN com.expecttarget AS et ON ep.expecttargetid = et.expecttargetid ";
            queryStr += "WHERE  et.StartScheduleDate >= GETDATE() AND ";
            queryStr += "       4 = et.classclass AND ";
            queryStr += "       1 = ep.situationstatus ";
            queryStr += "GROUP  BY ep.expecttargetid,et.sportsid,et.uniqueid ";
            queryStr += "ORDER  BY expectpoint DESC ";

            var query = com.Database.SqlQuery<HomeGameInfoViewModel>(queryStr);

            return query.Take(5);
        }
        #endregion

        #region Get Top Point of Games of a sport
        /// <summary>
        /// Get max point that bet by sportID.
        /// </summary>
        /// <param name="sportID">SportID.</param>
        /// <returns>GameID that bet max point.</returns>
        private int GetHomeTopPointGameOfSport(int sportID)
        {
            var sDate = DateTime.Now.Date;
            var expectPointGroup = (from ep in com.ExpectPoint
                                    join et in com.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                                    where DbFunctions.TruncateTime(et.StartScheduleDate) >= sDate && et.ClassClass == 4
                                            && ep.SituationStatus == 1 && et.SportsID == sportID //TODO SituationStatus 要確認（これは終了前の試合だけ対象で良い？
                                    select ep).GroupBy(x => x.ExpectTargetID).Select(
                                        l => new
                                        {
                                            ExpectTargetID = l.Key,
                                            TotalPoint = l.Sum(p => p.ExpectPoint1)
                                        }).ToList();

            int result = 0;
            var query = from ep in expectPointGroup
                        join et in com.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                        orderby ep.TotalPoint descending
                        select new HomeGameInfoViewModel
                        {
                            SportID = et.SportsID,
                            GameID = et.UniqueID,
                        } into game_info
                        select game_info;
            if (query != null && query.Any())
                result = query.FirstOrDefault().GameID;

            return result;
        }
        #endregion

        #region Get Recent game
        private IEnumerable<HomeRecentMatchViewModel> GetRecentMatch(int sportID)
        {
            IEnumerable<HomeRecentMatchViewModel> result = default(IEnumerable<HomeRecentMatchViewModel>);
            switch (sportID)
            {
                case 1:
                    result = GetNbpRecentMatch().ToList();
                    break;
                case 2:
                    result = GetJleagueRecentMatch();
                    break;
                case 3:
                    result = GetMlbRecentMatch();
                    break;
                //case 4:
                //    result = GetOverseaSoccerRecentMatch();
                //    break;
            }
            return result;

        }
        private IEnumerable<HomeRecentMatchViewModel> GetNbpRecentMatch()
        {
            IEnumerable<HomeRecentMatchViewModel> result = default(IEnumerable<HomeRecentMatchViewModel>);
            result = (from t1 in npb.RealGameInfoRootRGI
                      join t2 in npb.GameInfoRGI on t1.RealGameInfoRootRGIId equals t2.RealGameInfoRootRGIId
                      join home in npb.ScoreRGI on new { a = t1.RealGameInfoRootRGIId, b = t2.HomeTeam } equals new { a = home.RealGameInfoRootRGIId, b = home.TeamCD }
                      join visiter in npb.ScoreRGI on new { a = t1.RealGameInfoRootRGIId, b = t2.VisitorTeam.Value } equals new { a = visiter.RealGameInfoRootRGIId, b = visiter.TeamCD }
                      join t4 in npb.GameLiveGL on t2.GameID equals t4.GameID
                      join t5 in npb.GameInfoGL on t4.GameLiveGLId equals t5.GameLiveGLId
                      select new HomeRecentMatchViewModel
                      {
                          GameID = t2.GameID,
                          MatchDay = t1.Matchday,
                          ShortNameHomeTeam = t2.ShortNameHomeTeam,
                          ShortNameVisitorTeam = t2.ShortNameVisitorTeam,
                          TotalHomeTeamScore = home.TotalScore,
                          TotalVisitorTeamScore = visiter.TotalScore,
                          GameStateName = t5.GameStateName,
                          HomeTeamID = home.TeamCD,
                          VisitorTeamID = visiter.TeamCD,
                          GameSetSituationCD = t2.GameSetSituationCD,
                          Inning = t2.Inning
                      } into match
                      orderby match.MatchDay descending
                      select match).Take(3);
            return result;
        }
        private IEnumerable<HomeRecentMatchViewModel> GetJleagueRecentMatch()
        {
            // pending...
            return default(IEnumerable<HomeRecentMatchViewModel>);

        }
        private IEnumerable<HomeRecentMatchViewModel> GetMlbRecentMatch()
        {
            // pending...
            return default(IEnumerable<HomeRecentMatchViewModel>);

        }
        #endregion

        #region User Bet The First Time Or Not
        /// <summary>
        /// Define user bet the first time or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DefineBetFirstTimeOrNot()
        {
            var result = string.Empty;
            if (Session["CurrentUser"] != null)
            {
                var memberID = Session["CurrentUser"].ToString();
                var expect = com.ExpectPoint.Where(m => m.CreatedAccountID == memberID && m.SituationStatus == 4).OrderBy(m => m.ExpectPointID).FirstOrDefault();
                if (expect != null)
                {
                    result = expect.ExpectPointID.ToString();
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 予想した試合が終了しているかどうかを返す
        /// </summary>
        /// <returns>true:終了した試合有</returns>
        [HttpGet]
        public JsonResult IsExistsFinishedGame()
        {
            bool result = false;
            var memberId = this.GetMemberId();
            var systemDatetimeService = new SystemDatetimeService();
            var noticeService = new NoticeService(this.com, systemDatetimeService);
            var fromDate = Utils.GetStartOfDay(systemDatetimeService.SystemDate.AddDays(-1));
            var toDate = Utils.GetEndOfDay(systemDatetimeService.SystemDate);

            result = noticeService.NoticeExpectedGameInfo(memberId, fromDate, toDate, memberId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private long GetMemberId()
        {
            long memberId = -1;
            object currentUser = Session["CurrentUser"];

            if (currentUser != null)
            {
                memberId = Convert.ToInt64(currentUser.ToString());
            }

            return memberId;
        }

        #endregion
    }
}