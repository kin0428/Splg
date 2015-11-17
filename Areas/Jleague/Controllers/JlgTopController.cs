#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.MyPage;
using Splg.Controllers;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using Splg.Models.News.ViewModel;
using Splg.Services.Game;
using Splg.Services.System;
using WebGrease.Css.Extensions;

#endregion

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgTopController : Controller
    {
        #region Global Properties 
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        JlgEntities jlg = new JlgEntities();

        private SystemDatetimeService systemDatetimeService;
        #endregion

        public JlgTopController()
        {
            this.systemDatetimeService = new SystemDatetimeService();
        }

        #region Actions
        // GET: Jlg/JlgTop
        /// <summary>
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

        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.JleagueMenu = 1;
            JlgTopViewModel topViewModel = new JlgTopViewModel();
            topViewModel.TopMostViewNews = GetJlgNewTopViews();
            topViewModel.TopRecentNews = GetJlgNewTopViews(false);
            topViewModel.TopRecentPost = PostedController.GetRecentPosts(0, JlgConstants.JLG_SPORT_ID, null, null);
            topViewModel.TopExpectationsDeviation = GetTeamExpectedRanking(1);
            topViewModel.TopBetrayalDeviation = GetTeamExpectedRanking(2);
            DateTime dateNow = DateTime.Now;
            int year = dateNow.Year;
            int month = dateNow.Month;
            int day = dateNow.Day;
            int inputDate = year * 10000 + month * 100 + day;
            int gameDate = GetGameDate(inputDate, 0, JlgConstants.JLG_DATETYPE_NEXT);
            if (gameDate == 0)
            {
                gameDate = inputDate;
            } 
            topViewModel.JlgGameDate = gameDate;
            topViewModel.JlgDispGameDate = DateTime.ParseExact(gameDate.ToString(), "yyyyMMdd", null);
            topViewModel.JlgFirstGameDate = GetFirstGameDate(0);
            topViewModel.JlgLastGameDate = GetLastGameDate(0);
            return View(topViewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult JTop()
        {
            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JType = jType;
            JlgTopViewModel topViewModel = new JlgTopViewModel();
            int gameKind = JlgConstants.JLG_GAMEKIND_J1;
            ViewBag.JleagueSubMenu = 1;
            ViewBag.JleagueMenu = 2;
            if(jType == 2)
            {
                ViewBag.JleagueMenu = 3;
                gameKind = JlgConstants.JLG_GAMEKIND_J2;
            }
            topViewModel.JlgGameKind = gameKind;
            topViewModel.TopMostViewNews = GetJlgNewTopViewsInLeague(Constants.LEAGUE_TOPIC_CLASSIFICATION, gameKind);
            topViewModel.TopRecentNews = GetJlgNewTopViewsInLeague(Constants.LEAGUE_TOPIC_CLASSIFICATION, gameKind, false);
            topViewModel.TopExpectationsDeviation = GetTeamExpectedRanking(1, Constants.LEAGUE_TOPIC_CLASSIFICATION, gameKind);
            topViewModel.TopBetrayalDeviation = GetTeamExpectedRanking(2, Constants.LEAGUE_TOPIC_CLASSIFICATION, gameKind);
            topViewModel.TopRecentPost = PostedController.GetRecentPosts(3, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION, gameKind);
            DateTime dateNow = DateTime.Now;
            int year = dateNow.Year;
            int month = dateNow.Month;
            int day = dateNow.Day;
            int inputDate = year * 10000 + month * 100 + day;
            int gameDate = GetGameDate(inputDate, gameKind, JlgConstants.JLG_DATETYPE_NEXT);
            if (gameDate == 0)
            {
                gameDate = inputDate;
            }
            topViewModel.JlgGameDate = gameDate;
            topViewModel.JlgDispGameDate = DateTime.ParseExact(gameDate.ToString(), "yyyyMMdd", null);
            topViewModel.JlgFirstGameDate = GetFirstGameDate(gameKind);
            topViewModel.JlgLastGameDate = GetLastGameDate(gameKind);
            return View(topViewModel);
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NabiscoTop()
        {
            ViewBag.JleagueMenu = 4;
            ViewBag.JleagueSubMenu = 1;
            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JType = jType;
            JlgTopViewModel topViewModel = new JlgTopViewModel();
            topViewModel.JlgGameKind = JlgConstants.JLG_GAMEKIND_NABISCO;
            topViewModel.TopRecentPost = PostedController.GetRecentPosts(3, JlgConstants.JLG_SPORT_ID, Constants.LEAGUE_TOPIC_CLASSIFICATION, JlgConstants.JLG_GAMEKIND_NABISCO);
            DateTime dateNow = DateTime.Now;
            int year = dateNow.Year;
            int month = dateNow.Month;
            int day = dateNow.Day;
            int inputDate = year * 10000 + month * 100 + day;
            int gameDate = GetGameDate(inputDate, JlgConstants.JLG_GAMEKIND_NABISCO, JlgConstants.JLG_DATETYPE_NEXT);
            if (gameDate == 0)
            {
                gameDate = inputDate;
            }
            topViewModel.JlgGameDate = gameDate;
            topViewModel.JlgDispGameDate = DateTime.ParseExact(gameDate.ToString(), "yyyyMMdd", null);
            topViewModel.JlgFirstGameDate = GetFirstGameDate(JlgConstants.JLG_GAMEKIND_NABISCO);
            topViewModel.JlgLastGameDate = GetLastGameDate(JlgConstants.JLG_GAMEKIND_NABISCO);
            return View(topViewModel);
        }
        #endregion

        #region

        private IEnumerable<JlgTeamExpectedRankingViewModel> GetTeamExpectedRanking(int type, short classification = 0, int uniqueID = 0)
        {
            var result = new List<JlgTeamExpectedRankingViewModel>();

            DateTime dateNow = DateTime.Now;
            var queryFirst = default(IEnumerable<Deviation>);
            if(classification == 0 && uniqueID == 0)
            {
                queryFirst = from d in com.Deviation
                             where d.StartYear == dateNow.Year
                             && d.SportsID == JlgConstants.JLG_SPORT_ID
                             && d.ClassClass == (short)2
                             select d;
            }
            else
            {
                queryFirst = from d in com.Deviation
                             where d.StartYear == dateNow.Year
                             && d.SportsID == JlgConstants.JLG_SPORT_ID
                             && d.ClassClass == classification
                             //&& d.UniqueID == uniqueID
                             select d;
            }

            if (queryFirst != null)
            {
                //var lstQuery = (type == 1) ? queryFirst.OrderByDescending(m => m.ExpectationsDeviation).Take(3).ToList() : queryFirst.OrderByDescending(m => m.BetrayalDeviation).Take(3).ToList();

                //リーグ（GameKind)をまたがったランクなので、同一チームに対してRankInfoRTで複数の結果が取得できるので、Take(3)は実施しない。
                var lstQuery = (type == 1) ? queryFirst.OrderByDescending(m => m.ExpectationsDeviation).ToList() : queryFirst.OrderByDescending(m => m.BetrayalDeviation).ToList();

                var querySecond = (from q1 in lstQuery
                                  join q2 in jlg.TeamInfoTE on q1.UniqueID equals q2.TeamID
                                  join q3 in jlg.RankInfoRT on q1.UniqueID equals q3.TeamID
                                  join q4 in jlg.RankReportRT on q3.RankReportRTId equals q4.RankReportRTId
                                  join q6 in jlg.LeagueInfo on q4.GameKindID equals q6.LeagueID
                                  join q5 in jlg.TeamIconJlg on q1.UniqueID equals q5.TeamCD
                                  select new JlgTeamExpectedRankingViewModel
                                  {
                                      TeamIcon = q5.TeamIcon,
                                      TeamName = q2.TeamName,
                                      DeviationValue = (type == 1) ? q1.ExpectationsDeviation : q1.BetrayalDeviation,
                                      Ranking = q3.Ranking,
                                      LeagueNameS = q6.LeagueNameS,
                                      GameKindID = q4.GameKindID,
                                      TeamID = q1.UniqueID
                                  }).ToList();

                var usedTeamIds = new List<int>();
                if (querySecond != null)
                {
                    foreach (JlgTeamExpectedRankingViewModel rank in querySecond)
                    {
                        if(!usedTeamIds.Contains(rank.TeamID)){
                            result.Add(rank);
                            usedTeamIds.Add(rank.TeamID);
                        }

                        if (usedTeamIds.Count >= 3)
                            break;
                    }
                }
            }
            return result;
        }
        #endregion

        #region Get JlgNews Top Views
        /// <summary>
        /// Get top 4(Max View) or 6(Newest news) jlg news that have 
        /// </summary>
        /// <returns>List of news.</returns>
        private IEnumerable<NewsInfoViewModel> GetJlgNewTopViews(bool isTopView = true)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            if(isTopView)
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

                //query = (from brief in com.BriefNews
                //         join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                //         join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                //         join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                //         from tmp in br_photo.DefaultIfEmpty()
                //         join rds in com.NewsReadingSumView on brief.NewsItemID equals rds.NewsItemID
                //         where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                //            (tmp.Duid == Constants.IMAGE_DUID || tmp.Duid == null) && itpcsm.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
                //         orderby rds.View descending
                //         select new NewsInfoViewModel
                //         {
                //             NewsItemID = brief.NewsItemID,
                //             DeliveryDate = brief.DeliveryDate,
                //             Headline = brief.Headline,
                //             newstext = brief.newstext,
                //             Content = tmp.Content,
                //             Duid = tmp.Duid,
                //             SentFrom = brief.SentFrom,
                //             ItpcSubjectCode = itpc.IptcSubjectCode,
                //             TotalViews = rds.View.Value,
                //             SubHeadline = brief.SubHeadline
                //         } into news_photo
                //         select news_photo).Take(4);
            }
            else
            {
                query = (from brief in com.BriefNews
                         join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                         join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                         from tmp in br_photo.DefaultIfEmpty()
                         where brief.Status == Constants.NEWS_VALID_STATUS 
                         && brief.CarryLimitDate >= DateTime.Now 
                         && (tmp.Duid == Constants.IMAGE_THUMNAIL_DUID || tmp.Duid == null) 
                         && itpc.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
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
                         orderby news_photo.DeliveryDate descending
                         select news_photo).Take(6);

            }
            return query;
        }
        #endregion

        #region Get JlgNews Top Views
        /// <summary>
        /// Get top 4(Max View) or 6(Newest news) jlg news that have 
        /// </summary>
        /// <returns>List of news.</returns>
        private IEnumerable<NewsInfoViewModel> GetJlgNewTopViewsInLeague(int classification, int uniqueID, bool isTopView = true)
        {
            var query = default(IEnumerable<NewsInfoViewModel>);
            if (isTopView)
            {

                var viewsCount = (from r in com.NewsReading
                                  select r).GroupBy(x => x.NewsItemID).Select(
                                        l => new
                                        {
                                            NewsItemId = l.Key,
                                            ReadingSum = l.Count()
                                        });
                query = (from brief in com.BriefNews
                         join nt in com.NewsTopic on brief.NewsItemID equals nt.NewsItemID
                         join tm in com.TopicMaster on nt.TopicID equals tm.TopicID
                         join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                         join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                         join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                         from tmp in br_photo.DefaultIfEmpty()
                         join rds in com.NewsReadingSumView on brief.NewsItemID equals rds.NewsItemID
                         join v in viewsCount on brief.NewsItemID equals v.NewsItemId
                         where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                            (tmp.Duid == Constants.IMAGE_DUID || tmp.Duid == null) 
                            && tm.ClassificationType == classification
                            && tm.UniqueID == uniqueID
                            && itpcsm.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
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


                //query = (from brief in com.BriefNews
                //         join nt in com.NewsTopic on brief.NewsItemID equals nt.NewsItemID
                //         join tm in com.TopicMaster on nt.TopicID equals tm.TopicID
                //         join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                //         from tmp in br_photo.DefaultIfEmpty()
                //         join rds in com.NewsReadingSumView on brief.NewsItemID equals rds.NewsItemID
                //         where brief.Status == Constants.NEWS_VALID_STATUS 
                //            && brief.CarryLimitDate >= DateTime.Now 
                //            && (tmp.Duid == Constants.IMAGE_DUID || tmp.Duid == null) 
                //            && tm.ClassificationType == classification
                //            && tm.UniqueID == uniqueID
                //         orderby rds.View descending
                //         select new NewsInfoViewModel
                //         {
                //             NewsItemID = brief.NewsItemID,
                //             DeliveryDate = brief.DeliveryDate,
                //             Headline = brief.Headline,
                //             newstext = brief.newstext,
                //             Content = tmp.Content,
                //             Duid = tmp.Duid,
                //             SentFrom = brief.SentFrom,
                //             TotalViews = rds.View.Value,
                //             SubHeadline = brief.SubHeadline
                //         } into news_photo
                //         select news_photo).Take(4);
            }
            else
            {
                query = (from brief in com.BriefNews
                         join nt in com.NewsTopic on brief.NewsItemID equals nt.NewsItemID
                         join tm in com.TopicMaster on nt.TopicID equals tm.TopicID
                         join photo in com.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                         from tmp in br_photo.DefaultIfEmpty()
                         where brief.Status == Constants.NEWS_VALID_STATUS
                         && brief.CarryLimitDate >= DateTime.Now
                         && (tmp.Duid == Constants.IMAGE_THUMNAIL_DUID || tmp.Duid == null)
                        && tm.ClassificationType == classification
                        && tm.UniqueID == uniqueID
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
                         orderby news_photo descending
                         select news_photo).Take(6);

            }
            return query;
        }
        #endregion

        #region Show game info Show game info in partial view.

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">
        /// 0 JlgTeamInfoScheduleResult チーム情報　日程・結果
        /// 1 JlgTop リーグTOP
        /// 2 JlgScheduleResult 日程・結果
        /// 3 JlgTeamInfoTop チーム情報　日程・結果 次の試合を見る
        /// 4 _JlgGameInfoPlayerInfo.cshtml　試合情報　　or HOME 注目度の高い試合  or UserArticle 
        /// 5 JlgGameInformation 次の試合を予想してみよう
        /// 6 Home サッカーTOP　　or HOME 予想締切間近の試合
        /// </param>
        /// <param name="link"></param>
        /// <param name="gameDate"></param>
        /// <param name="occasionNo"></param>
        /// <param name="teamID"></param>
        /// <param name="gameID"></param>
        /// <param name="lstGameID"></param>
        /// <param name="leagueType"></param>
        /// <param name="round"></param>
        /// <param name="withScoreDetails"></param>
        /// <param name="seasonId"></param>
        /// <returns></returns>
        [NoCache]
        public ActionResult ShowGameInfo(int? type, int? link, int? gameDate, int? occasionNo, int? teamID, int? gameID, string lstGameID, int? leagueType = 0, int? round = 0, bool withScoreDetails = false, int seasonId = 0)
        {
            IEnumerable<JlgGameInfos> lstGame = default(IEnumerable<JlgGameInfos>);
            ViewBag.Type = type;
            ViewBag.Link = link;
            var lstgId = new List<int>();

            long memberId = this.GetMemberId();

            if (type == 5)
            {
                int? gDate = jlg.ScheduleInfo.Where(m => m.GameDate > gameDate.Value).Min(m => m.GameDate);
                lstGame = GetGameInfo(gDate, occasionNo, teamID, null, null, 0, 0, withScoreDetails, memberId, seasonId);
            }
            else
            {
                //Convert string to list
                if (type == 6 && !string.IsNullOrEmpty(lstGameID))
                {
                    List<string> listofIDs = lstGameID.Split(',').ToList();
                    lstgId = listofIDs.Select(int.Parse).ToList();
                    lstGame = GetGameInfo(gameDate, occasionNo, teamID, gameID, lstgId, 0, 0, withScoreDetails, memberId, seasonId);
                }
                else
                {
                    lstGame = GetGameInfo(gameDate, occasionNo, teamID, gameID, null, leagueType, round, withScoreDetails, memberId, seasonId);
                }
            }

            if (lstGame != null && lstGame.Any())
            {
                lstGame.ForEach(x => x.ParameterInfo = new JlgGameInfos.ParameterInfoModel
                {
                    ParameterType = type,
                    Link = link,
                    GameDate = gameDate,
                    OccasionNo = occasionNo,
                    TeamId = teamID,
                    GameId = gameID,
                    LstGameId = lstGameID,
                    LeagueType = leagueType,
                    Round = round,
                    WithScoreDetail = withScoreDetails
                });

                var jlsService = new JlgService();
                foreach (var game in lstGame)
                {
                    occasionNo = jlsService.GetOccasionNo(game.GameDate, game.GameKindID);
                    game.OccasionNo = occasionNo ?? 0;
                    
                    //Phase3#2000~2002 試合情報パネル対応
                    //スコア情報を読み込む
                    if (withScoreDetails)
                        game.ScoreDetails = this.GetScoreDetails(game.GameID);
                }
            }

            if (type == 5)  // 次節の試合情報
            {
                return PartialView("_JlgNextGames", lstGame);
            }

            if (withScoreDetails)
                return PartialView("_JlgGameCard", lstGame);
            else
                return PartialView("_JlgGameInfo", lstGame);
        }

        #endregion

        private long GetMemberId()
        {
            long memberId = -1;
            object currentUser = Session["CurrentUser"];

            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());
            return memberId;
        }


        #region Get Game Info

        ///  <summary>
        ///  Get game info use for many page : 8-1, 8-2, 8-5-1, 8-6 Top and Bottom, Home,...
        ///  </summary>
        /// <param name="gameDate">GameDate.</param>
        ///  <param name="occasionNo">occasionNo.(節)</param>
        ///  <param name="teamId">TeamID.</param>
        ///  <param name="gameId">GameID.</param>
        ///  <param name="lstGameId">List GameID.</param>
        /// <param name="leagueType">leagueType.(1:JリーグTOP / 2:J1TOP / 3:J2TOP)</param>
        /// <param name="round"></param>
        /// <param name="withScoreDetails"></param>
        /// <param name="memberId"></param>
        /// <returns>List game for each condition.</returns>
        public IEnumerable<JlgGameInfos> GetGameInfo(int? gameDate, int? occasionNo, int? teamId, int? gameId,
                                                     List<int> lstGameId, int? leagueType = 0, int? round = 0,
                                                     bool withScoreDetails = false, long memberId = -1, int seasonId = 0)
        {

            var result = new List<JlgGameInfos>();
            var query = default(IEnumerable<JlgGameInfos>);
            var queryFirst = default(IQueryable<JlgGameInfoByCondidtion>);
            bool isValue = true;

            if (lstGameId == null)
            {
                isValue = false;
                lstGameId = new List<int>();
            }

            switch (leagueType)
            {
                case 0:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate)
                                 && (occasionNo == null || si.OccasionNo == occasionNo)
                                 && (round == 0 || si.Round == round)
                                 && (seasonId == 0 || gcat.SeasonID == seasonId)
                                 && (teamId == null || si.HomeTeamID == teamId || si.AwayTeamID == teamId)
                                 && (si.GameID == gameId || gameId == null)
                                 && (!isValue || lstGameId.Contains(si.GameID))
                                 where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_J1 || gs.GameKindID == JlgConstants.JLG_GAMEKIND_J2 || gs.GameKindID == JlgConstants.JLG_GAMEKIND_NABISCO)
                                 select new JlgGameInfoByCondidtion()
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID   
                                 };
                    break;
                case 1:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate) 
                                 && (occasionNo == null || si.OccasionNo == occasionNo)
                                 && (round == 0 || si.Round == round)
                                 && (seasonId == 0 || gcat.SeasonID == seasonId)
                                 && (teamId == null || si.HomeTeamID == teamId || si.AwayTeamID == teamId)
                                 && (si.GameID == gameId || gameId == null)
                                 && (!isValue || lstGameId.Contains(si.GameID))
                                 where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_J1)
                                 select new JlgGameInfoByCondidtion
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID   
                                 };

                    break;
                case 2:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate) 
                                 && (occasionNo == null || si.OccasionNo == occasionNo)
                                 && (round == 0 || si.Round == round)
                                 && (seasonId == 0 || gcat.SeasonID == seasonId)
                                 && (teamId == null || si.HomeTeamID == teamId || si.AwayTeamID == teamId)
                                 && (si.GameID == gameId || gameId == null)
                                 && (!isValue || lstGameId.Contains(si.GameID))
                                 where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_J2)
                                 select new JlgGameInfoByCondidtion
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID   
                                 };
                    break;
                case 3:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate)
                                 && (occasionNo == null || si.OccasionNo == occasionNo)
                                 && (round == 0 || si.Round == round)
                                 && (seasonId == 0 || gcat.SeasonID == seasonId)
                                 && (teamId == null || si.HomeTeamID == teamId || si.AwayTeamID == teamId)
                                 && (si.GameID == gameId || gameId == null)
                                 && (!isValue || lstGameId.Contains(si.GameID))
                                 where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_NABISCO)
                                 select new JlgGameInfoByCondidtion
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID   
                                 };
                    break;
                        

            }
            
            //Continue get extra info for game.
            if (queryFirst == null || !queryFirst.Any())
            {
                query = null;
            }
            else
            {
                query = (from q in queryFirst
                         join ti in jlg.TeamIconJlg on q.ScheduleInfoJlg.HomeTeamID equals ti.TeamCD into htic
                         from hti in htic.DefaultIfEmpty()
                         join ti1 in jlg.TeamIconJlg on q.ScheduleInfoJlg.AwayTeamID equals ti1.TeamCD into atic
                         from ati in atic.DefaultIfEmpty()
                         join osh1 in jlg.RankReportRT on new { q1 = q.GameKindID, q2 = q.SeasonId } equals new { q1 = osh1.GameKindID, q2 = osh1.SeasonID } into hrr
                         from hrpt in hrr.DefaultIfEmpty()
                         join os1 in jlg.RankInfoRT on new { q1 = (int)q.ScheduleInfoJlg.HomeTeamID.Value, q2 = hrpt.RankReportRTId } equals new { q1 = (int)os1.TeamID, q2 = os1.RankReportRTId } into rir1
                         from hrir in rir1.DefaultIfEmpty()
                         join grl in jlg.GameReportLG on new { q1 = q.ScheduleInfoJlg.GameID, q2 = q.SeasonId } equals new { q1 = grl.GameID, q2 = grl.SeasonID } into grl1
                         from grpl in grl1.DefaultIfEmpty()
                         join ht1 in jlg.TeamInfoLG on new { k1 = grpl.GameReportLGId, k2 = (int)q.ScheduleInfoJlg.HomeTeamID.Value } equals new { k1 = ht1.GameReportLGId, k2 = ht1.ID } into htl1
                         from htil in htl1.DefaultIfEmpty()
                         join at1 in jlg.TeamInfoLG on new { k1 = grpl.GameReportLGId, k2 = (int)q.ScheduleInfoJlg.AwayTeamID.Value } equals new { k1 = at1.GameReportLGId, k2 = at1.ID } into atl1
                         from atil in atl1.DefaultIfEmpty()
                         join osh2 in jlg.RankReportRT on new { q1 = q.GameKindID, q2 = q.SeasonId } equals new { q1 = osh2.GameKindID, q2 = osh2.SeasonID } into arr
                         from arpt in arr.DefaultIfEmpty()
                         join os2 in jlg.RankInfoRT on new { k1 = (int)q.ScheduleInfoJlg.AwayTeamID.Value, k2 = arpt.RankReportRTId } equals new { k1 = (int)os2.TeamID, k2 = os2.RankReportRTId } into rir2
                         from arir in rir2.DefaultIfEmpty()
                         join sehe in jlg.ScheduleInfoHE on q.ScheduleInfoJlg.GameID equals sehe.GameID into sehec
                         from sehei in sehec.DefaultIfEmpty()
                         select new JlgGameInfos
                         {
                             GameID = q.ScheduleInfoJlg.GameID,
                             GameDate = (int)q.ScheduleInfoJlg.GameDate,
                             Time = q.ScheduleInfoJlg.GameTime.ToString(),
                             StadiumName = q.ScheduleInfoJlg.StadiumName,
                             GameKindID = q.GameKindID,
                             GameKindName = q.GameKindName,
                             HomeTeamID = q.ScheduleInfoJlg.HomeTeamID.Value,
                             HomeTeamName = q.ScheduleInfoJlg.HomeTeamName ?? string.Empty,
                             HomeTeamNameS = q.ScheduleInfoJlg.HomeTeamNameS ?? string.Empty,
                             HomeTeamIcon = hti.TeamIcon ?? string.Empty,
                             HomeTeamRanking = hrir.Ranking != null ? hrir.Ranking : 0,
                             HomeTeamWin = sehei.HomeWin ?? 0,
                             HomeTeamR = htil.Score,
                             HomeTeamScore = htil.Score.ToString(),
                             AwayTeamID = q.ScheduleInfoJlg.AwayTeamID.Value,
                             AwayTeamName = q.ScheduleInfoJlg.AwayTeamName ?? string.Empty,
                             AwayTeamNameS = q.ScheduleInfoJlg.AwayTeamNameS ?? string.Empty,
                             AwayTeamIcon = ati.TeamIcon ?? string.Empty,
                             AwayTeamRanking = arir.Ranking != null ? arir.Ranking : 0,
                             AwayTeamWin = sehei.AwayWin ?? 0,
                             AwayTeamR = atil.Score,
                             AwayTeamScore = atil.Score.ToString(),
                             Round = q.ScheduleInfoJlg.Round.Value
                         });
            }

            if (query != null)
            {
                // 予想情報を取得
                // CHSTMLとGetGameInfoForTopで同じ値を取得してしまっている。
                // TDDO IsMobileDevice廃止
                // （PC版のDBアクセス箇所をコントローラ以下に移してGetGameInfoForTopを呼ぶようにする）
                IEnumerable<GameInfoModel> expectedInfo = null;
                if (memberId > 0 && this.HttpContext.Request.Browser.IsMobileDevice)
                {
                    expectedInfo = MyPageCommon.GetGameInfoForTop(memberId, this.systemDatetimeService.TargetYear, this.systemDatetimeService.TargetMonth, 0, Constants.JLG_SPORT_ID);
                }

                var oddsService = new OddsService();

                var newQuery = query.ToList();

                foreach (var q in newQuery)
                {
                    q.GameOddsInfoModel = oddsService.GetOddsInfoByGameID(Constants.JLG_SPORT_ID, q.GameID, memberId);

                    q.GameStatus = JlgCommon.GetStatusMatch(q.GameID, memberId.ToString());

                    if (expectedInfo != null)
                    {
                        q.GameInfoModel = expectedInfo.FirstOrDefault(e => e.GameID == q.GameID && e.SportsID == Constants.JLG_SPORT_ID);

                        // todo 本来ならば、MyPageCommon.GetGameInfoForTopにて設定するべき
                        if (q.GameInfoModel != null)
                        {
                            q.GameInfoModel.GameStatus = q.GameStatus;
                        }
                    }

                    //予想している会員を取得する（ログイン会員以外）
                    ExpectInfoService expectInfoService = new ExpectInfoService(com);
                    q.BetMembers = expectInfoService.GetBetMembers(Constants.JLG_SPORT_ID, q.GameID, memberId, true);
                }
            
                query = newQuery.AsQueryable();
            }

            if (withScoreDetails)
            {
                //試合速報_ヘッダー情報
                if (query != null)
                {
                    var queryList = query.ToList();

                    foreach (JlgGameInfos g in queryList)
                    {
                        var report = (from rp in jlg.GameReportLG where rp.GameID == g.GameID select rp).FirstOrDefault();
                        if (report != null)
                        {
                            g.StateID = report.StateID;
                            g.StateName = report.StateName;
                            g.Half = report.Half;
                            g.HalfEndF = report.HalfEndF;
                            g.ScoreDetails = this.GetScoreDetails(g.GameID);
                        }
                        result.Add(g);
                    }

                    return result;
                }
            } 
            else
            {
                return query != null ? query.ToList() : null;
            }

            return null;
        }
        #endregion


        #region Get Game Info original interface
        /// <summary>
        /// Get game info use for many page : 8-1, 8-2, 8-5-1, 8-6 Top and Bottom, Home,...
        /// </summary>
        ///<param name="gameDate">GameDate.</param>
        /// <param name="startDate">StartDate.</param>
        /// <param name="endDate">EndDate.</param>
        /// <param name="teamID">TeamID.</param>
        /// <param name="gameID">GameID.</param>
        /// <param name="lstGameID">List GameID.</param>
        /// <param name="lstGameID">leagueType.(1:JリーグTOP / 2:J1TOP / 3:J2TOP)</param>
        /// <returns>List game for each condition.</returns>
        public IEnumerable<JlgGameInfos> GetGameInfoByDate(int? gameDate, int? startDate, int? endDate, int? teamID, int? gameID, List<int> lstGameID, int? leagueType = 0)
        {
            var query = default(IEnumerable<JlgGameInfos>);
            var queryFirst = default(IQueryable<JlgGameInfoByCondidtion>);
            bool isValue = true;
            if (lstGameID == null)
            {
                isValue = false;
                lstGameID = new List<int>();
            }
            switch (leagueType)
            {
                case 0:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate) && (startDate == null || si.GameDate >= startDate)
                                 && (endDate == null || si.GameDate <= endDate)
                                 && (teamID == null || si.HomeTeamID == teamID || si.AwayTeamID == teamID)
                                 && (si.GameID == gameID || gameID == null)
                                 && (!isValue || lstGameID.Contains(si.GameID))
                                 select new JlgGameInfoByCondidtion
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID
                                 };
                    break;
                case 1:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate) && (startDate == null || si.GameDate >= startDate)
                                 && (endDate == null || si.GameDate <= endDate)
                                 && (teamID == null || si.HomeTeamID == teamID || si.AwayTeamID == teamID)
                                 && (si.GameID == gameID || gameID == null)
                                 && (!isValue || lstGameID.Contains(si.GameID))
                                 where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_J1 || gs.GameKindID == JlgConstants.JLG_GAMEKIND_J2)
                                 select new JlgGameInfoByCondidtion
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID
                                 };
                    break;
                case 2:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate) && (startDate == null || si.GameDate >= startDate)
                                 && (endDate == null || si.GameDate <= endDate)
                                 && (teamID == null || si.HomeTeamID == teamID || si.AwayTeamID == teamID)
                                 && (si.GameID == gameID || gameID == null)
                                 && (!isValue || lstGameID.Contains(si.GameID))
                                 where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_J1)
                                 select new JlgGameInfoByCondidtion
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID
                                 };

                    break;
                case 3:
                    queryFirst = from si in jlg.ScheduleInfo
                                 join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                 join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                 where (gameDate == null || si.GameDate == gameDate) && (startDate == null || si.GameDate >= startDate)
                                 && (endDate == null || si.GameDate <= endDate)
                                 && (teamID == null || si.HomeTeamID == teamID || si.AwayTeamID == teamID)
                                 && (si.GameID == gameID || gameID == null)
                                 && (!isValue || lstGameID.Contains(si.GameID))
                                 where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_J2)
                                 select new JlgGameInfoByCondidtion
                                 {
                                     ScheduleInfoJlg = si,
                                     GameKindID = gs.GameKindID,
                                     GameKindName = gs.GameKindName,
                                     SeasonId = gcat.SeasonID
                                 };
                    break;


            }

            //Continue get extra info for game.
            if (queryFirst == null || queryFirst.Count() == 0)
            {
                query = null;
            }
            else
            {
                query = (from q in queryFirst
                         join ti in jlg.TeamIconJlg on q.ScheduleInfoJlg.HomeTeamID equals ti.TeamCD into htic
                         from hti in htic.DefaultIfEmpty()
                         join ti1 in jlg.TeamIconJlg on q.ScheduleInfoJlg.AwayTeamID equals ti1.TeamCD into atic
                         from ati in atic.DefaultIfEmpty()
                         join grl in jlg.GameReportLG on q.ScheduleInfoJlg.GameID equals grl.GameID into grl1
                         from grpl in grl1.DefaultIfEmpty()
                         join ht1 in jlg.TeamInfoLG on new { k1 = grpl.GameReportLGId, k2 = (int)q.ScheduleInfoJlg.HomeTeamID.Value } equals new { k1 = ht1.GameReportLGId, k2 = ht1.ID } into htl1
                         from htil in htl1.DefaultIfEmpty()
                         join at1 in jlg.TeamInfoLG on new { k1 = grpl.GameReportLGId, k2 = (int)q.ScheduleInfoJlg.AwayTeamID.Value } equals new { k1 = at1.GameReportLGId, k2 = at1.ID } into atl1
                         from atil in atl1.DefaultIfEmpty()
                         join osh1 in jlg.RankReportRT on new { k1 = q.GameKindID, k2 = q.SeasonId } equals new { k1 = osh1.GameKindID, k2 = osh1.SeasonID } into hrr
                         from hrpt in hrr.DefaultIfEmpty()
                         join os1 in jlg.RankInfoRT on new { q1 = (int)q.ScheduleInfoJlg.HomeTeamID.Value, q2 = hrpt.RankReportRTId } equals new { q1 = (int)os1.TeamID, q2 = os1.RankReportRTId } into rir1
                         from hrir in rir1.DefaultIfEmpty()
                         join osh2 in jlg.RankReportRT on new { k1 = q.GameKindID, k2 = q.SeasonId } equals new { k1 = osh2.GameKindID, k2 = osh2.SeasonID } into arr
                         from arpt in arr.DefaultIfEmpty()
                         join os2 in jlg.RankInfoRT on new { k1 = (int)q.ScheduleInfoJlg.AwayTeamID.Value, k2 = arpt.RankReportRTId } equals new { k1 = (int)os2.TeamID, k2 = os2.RankReportRTId } into rir2
                         from arir in rir2.DefaultIfEmpty()
                         join htcr in jlg.TeamCardReportTC on new { p1 = q.GameKindID, p2 = hti.TeamCD } equals new { p1 = htcr.GameKindID, p2 = htcr.TeamID }
                         join hti in jlg.TeamInfoTC on htcr.TeamCardReportTCId equals hti.TeamCardReportTCId into tit1
                         from htit in tit1.DefaultIfEmpty()
                         join hri in jlg.ResultInfoTC on new { p1 = htit.TeamInfoTCId, p2 = hti.TeamCD } equals new { p1 = hri.TeamInfoTCId, p2 = hri.ID } into hritc
                         from hrit in hritc.DefaultIfEmpty()
                         join atcr in jlg.TeamCardReportTC on new { p1 = q.GameKindID, p2 = ati.TeamCD } equals new { p1 = atcr.GameKindID, p2 = atcr.TeamID }
                         join ati in jlg.TeamInfoTC on atcr.TeamCardReportTCId equals ati.TeamCardReportTCId into tit2
                         from atit in tit2.DefaultIfEmpty()
                         join ari in jlg.ResultInfoTC on new { p1 = atit.TeamInfoTCId, p2 = ati.TeamCD } equals new { p1 = ari.TeamInfoTCId, p2 = ari.ID } into aritc
                         from arit in aritc.DefaultIfEmpty()
                         select new JlgGameInfos
                         {
                             GameID = q.ScheduleInfoJlg.GameID,
                             GameDate = (int)q.ScheduleInfoJlg.GameDate,
                             Time = q.ScheduleInfoJlg.GameTime.ToString(),
                             StadiumName = q.ScheduleInfoJlg.StadiumName,
                             GameKindID = q.GameKindID,
                             GameKindName = q.GameKindName,
                             HomeTeamID = q.ScheduleInfoJlg.HomeTeamID.Value,
                             HomeTeamName = q.ScheduleInfoJlg.HomeTeamName != null ? q.ScheduleInfoJlg.HomeTeamName : string.Empty,
                             HomeTeamNameS = q.ScheduleInfoJlg.HomeTeamNameS != null ? q.ScheduleInfoJlg.HomeTeamNameS : string.Empty,
                             HomeTeamIcon = hti.TeamIcon != null ? hti.TeamIcon : string.Empty,
                             HomeTeamRanking = hrir.Ranking != null ? hrir.Ranking : 0,
                             HomeTeamWin = hrit.Win.Value != null ? hrit.Win.Value : 0,
                             HomeTeamR = htil.Score,
                             HomeTeamScore = htil.Score.ToString(),
                             AwayTeamID = q.ScheduleInfoJlg.AwayTeamID.Value,
                             AwayTeamName = q.ScheduleInfoJlg.AwayTeamName != null ? q.ScheduleInfoJlg.AwayTeamName : string.Empty,
                             AwayTeamNameS = q.ScheduleInfoJlg.AwayTeamNameS != null ? q.ScheduleInfoJlg.AwayTeamNameS : string.Empty,
                             AwayTeamIcon = ati.TeamIcon != null ? ati.TeamIcon : string.Empty,
                             AwayTeamRanking = arir.Ranking != null ? arir.Ranking : 0,
                             AwayTeamWin = arit.Win.Value != null ? arit.Win.Value : 0,
                             AwayTeamR = atil.Score,
                             AwayTeamScore = atil.Score.ToString(),
                             Round = q.ScheduleInfoJlg.Round.Value
                         });
            }
            return query;
        }
        #endregion

        /// <summary>
        ///　ゴール毎のレコードを複数取得する。
        ///　HVでどちらの得点か判断できる。
        ///　また、StateIDでどちらのハーフ化判定できる（ 1 or 3 or 5（前半の場合）、2 or 4 or 6（後半の場合））
        /// </summary>
        /// <param name="gameID"></param>C:\Users\noji\Documents\splog_solution\Splg\Areas\Jleague\
        /// <param name="half"></param>
        /// <returns></returns>
        public List<JlgScoreDetailsModel> GetScoreDetails(int? gameID)
        {
            List<JlgScoreDetailsModel> result = new List<JlgScoreDetailsModel>();

            var query = (from grlg in jlg.GameReportLG                                                  //一試合速報_ヘッダー情報
                     join tilg in jlg.TeamInfoLG on grlg.GameReportLGId equals tilg.GameReportLGId     //一試合速報_チーム情報
                     join gilg in jlg.GoalInfoLG on new{ p1 = grlg.GameReportLGId, p2 = tilg.ID } equals new { p1 = gilg.GameReportLGId, p2 = (int)gilg.ScoreTeamID }  //一試合速報_得点情報
                     where grlg.GameID == gameID
                     orderby gilg.CreatedDate
                     select new JlgScoreDetailsModel
                     {
                        Hv = tilg.HV,
                        No = gilg.No,
                        StateID = gilg.StateID,
                        StateName = gilg.StateName,
                        Time = gilg.Time,
                        Half = gilg.Half,
                        TeamID = gilg.TeamID,
                        TeamNameS = gilg.TeamNameS,
                        GPlayerID = gilg.GPlayerID,
                        GPlayerName = gilg.GPlayerName,
                        GPlayerNameS = gilg.GPlayerNameS,
                        GPlayerUni = gilg.GPlayerUni,
                        Body = gilg.Body,
                        Operation = gilg.Operation,
                        APlayerID = gilg.APlayerID,
                        APlayerName = gilg.APlayerName,
                        APlayerNameS = gilg.APlayerNameS,
                        APlayerUni = gilg.APlayerUni,
                        AssistKind = gilg.AssistKind,
                        OwnGoalF = gilg.OwnGoalF,
                        ScoreTeamID = gilg.ScoreTeamID,
                        ScoreTeamNameS = gilg.ScoreTeamNameS
                          
                     });

            //試合スケジュール_試合情報 ScheduleInfo
            return result = query.ToList();

        }



        #region Format GameDate
        /// <summary>
        /// Get game date from server side in session.
        /// </summary>
        /// <returns>String date formatted.</returns>
        [HttpPost]
        public JsonResult FormatGameDate(string date)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(date))
            {
                DateTime tmpDate = DateTime.ParseExact(date, "yyyyMMdd", null);
                //var tmpDate = Convert.ToDateTime(date);
                result = Utils.GetMonthAndDayOfWeek(tmpDate) + "の試合";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Format OccasionNo
        /// <summary>
        /// Get game date from server side in session.
        /// </summary>
        /// <returns>String OccasionNo formatted.</returns>
        [HttpPost]
        public JsonResult FormatOccasionNo(string date, int? occasionNo)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(date) && occasionNo != null)
            {
                DateTime tmpDate = DateTime.ParseExact(date, "yyyyMMdd", null);
                result = "第" + occasionNo + "節" + Utils.GetMonthAndDayOfWeek(tmpDate);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Define User Login Or Not
        /// <summary>
        /// Define user login or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DefineLoginOrNot()
        {
            string result = string.Empty;
            if (Session["CurrentUser"] != null)
            {
                result = Session["CurrentUser"].ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 現在日付以降（以前）の試合日を取得する
        /// <summary>
        /// 現在日付以降の節を取得する
        /// </summary>
        /// <param name="dateInput">基準日</param>
         /// <returns></returns>
        [HttpPost]
        public int GetGameDate(int dateInput, int gameKindId, int dateType)
        {
            // 初期値をセット
            int? query = 0;

            // 引数によって場合分け
            switch(gameKindId){
                // JLGTOPの場合（ゲームKindに指定なし）
                case 0:
                    if(dateType == JlgConstants.JLG_DATETYPE_NEXT){
                            query = (from si in jlg.ScheduleInfo
                                         join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                         join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                         where (si.GameDate >= dateInput)
                                         orderby si.GameDate
                                         select si.GameDate).FirstOrDefault();
                    } else if(dateType == JlgConstants.JLG_DATETYPE_PREV){
                            query = (from si in jlg.ScheduleInfo
                                         join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                         join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                         where (si.GameDate <= dateInput)
                                     orderby si.GameDate descending
                                         select si.GameDate).FirstOrDefault();
                    }
                    break;
                // その他の場合（J1,J2,ナビスコTOP)
                default:
                    if(dateType == JlgConstants.JLG_DATETYPE_NEXT){
                            query = (from si in jlg.ScheduleInfo
                                         join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                         join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                         where (si.GameDate >= dateInput)
                                         where gs.GameKindID == gameKindId
                                         orderby si.GameDate
                                         select si.GameDate).FirstOrDefault();
                    } else if(dateType == JlgConstants.JLG_DATETYPE_PREV){
                            query = (from si in jlg.ScheduleInfo
                                         join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                         join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                         where (si.GameDate <= dateInput)
                                         where gs.GameKindID == gameKindId
                                     orderby si.GameDate descending
                                         select si.GameDate).FirstOrDefault();
                    }
                    break;
            }

            if (query != null)
            {
                return (int)query;
            }
            else
            {
                return 0;
            }
        }
        #endregion
        #region シーズン最初の試合日と最後の試合日を取得する
        /// <summary>
        /// シーズン最初の試合日を取得する
        /// </summary>
        /// <param name="dateInput">基準日</param>
         /// <returns></returns>
        [HttpPost]
        public int GetFirstGameDate(int gameKindId)
        {
            // 初期値をセット
            int? query = 0;

            // 引数によって場合分け
            switch(gameKindId){
                // JLGTOPの場合（ゲームKindに指定なし）
                case 0:
                    query = (from si in jlg.ScheduleInfo
                                    join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                    join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                    orderby si.GameDate
                                    select si.GameDate).FirstOrDefault();
                    break;
                // その他の場合（J1,J2,ナビスコTOP)
                default:
                    query = (from si in jlg.ScheduleInfo
                                    join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                    join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                    where gs.GameKindID == gameKindId
                                    orderby si.GameDate
                                    select si.GameDate).FirstOrDefault();
                    break;
            }

            if (query != null)
            {
                return (int)query;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// シーズン最後試合日を取得する
        /// </summary>
        /// <param name="dateInput">基準日</param>
         /// <returns></returns>
        [HttpPost]
        public int GetLastGameDate(int gameKindId)
        {
            // 初期値をセット
            int? query = 0;

            // 引数によって場合分け
            switch(gameKindId){
                // JLGTOPの場合（ゲームKindに指定なし）
                case 0:
                    query = (from si in jlg.ScheduleInfo
                                    join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                    join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                    orderby si.GameDate descending
                                    select si.GameDate).FirstOrDefault();
                    break;
                // その他の場合（J1,J2,ナビスコTOP)
                default:
                    query = (from si in jlg.ScheduleInfo
                                    join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                                    join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                                    where gs.GameKindID == gameKindId
                                    orderby si.GameDate descending
                                    select si.GameDate).FirstOrDefault();
                    break;
            }

            if (query != null)
            {
                return (int)query;
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}