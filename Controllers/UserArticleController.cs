using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Splg.Areas.Jleague.Controllers;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb.Controllers;
using Splg.Areas.Mlb.Models;
using Splg.Areas.Npb.Controllers;
using Splg.Areas.Npb.Models;
using Splg.Areas.User.Models.InfoModel;
using Splg.Areas.User.Models.ViewModel;
using Splg.Models;
using Splg.Models.News.ViewModel;
using Splg.Models.UserArticle.ViewModel;
using Splg.Core.Constant;

namespace Splg.Controllers
{

    public class UserArticleController : Controller
    {
        private static ComEntities memberContext = new ComEntities();
        private static MlbEntities mlbContext = new MlbEntities();
        private static JlgEntities jlgContext = new JlgEntities();
        private static NpbEntities npbContext = new NpbEntities();

        // GET: UserArticle
        #region 5-1 投稿記事トップ | 5-2 投稿記事_検索結果 | 5-3 投稿記事_記事詳細
        public ActionResult Index(int? page, int? year, int? month)
        {
            var articleListDefault = default(IEnumerable<PostedInfoViewModel>);
            if (Session["CurrentUser"] != null)
            {
                var memberId = Convert.ToInt64(Session["CurrentUser"].ToString());
                var email = memberContext.Member.SingleOrDefault(m => m.MemberId == memberId).Mail;
                var username = memberContext.Member.SingleOrDefault(m => m.MemberId == memberId).Nickname;

                ViewBag.UserName = username;
                ViewBag.CurrentPage = "投稿記事トップ";

                articleListDefault = PostedController.GetRecentPosts(5);
            }
            else
            {
                articleListDefault = PostedController.GetRecentPosts(4);
            }
            if (articleListDefault != null)
            {
                int pageNumber = (page ?? 1);
                var postYear = (year == null) ? DateTime.Now.Year : year.Value;
                var postMonth = DateTime.Now.Month;
                var spara = memberContext.SystemParamater.Find(1);
                int pageSize = 10;
                if (spara != null)
                    pageSize = Convert.ToInt32(spara.Spara);

                var postMothList = new UserArticleMonthListViewModel();
                postMothList.Year = postYear;
                postMothList.YearList = (from a in articleListDefault
                                         select a).GroupBy(m => m.PostYear).Select(l => l.Key).ToList();
                if (postMothList.YearList != null)
                    postMothList.YearList.Sort();
                postMothList.YearList.Sort();
                postMothList.MonthList = (from a in articleListDefault
                                          where a.PostYear == postYear
                                          select a).GroupBy(m => m.PostMonth).Select(l => l.Key).ToList();
                if (postMothList.MonthList != null && postMothList.MonthList.Any())
                {
                    postMothList.MonthList.Sort();
                    if (month != null)
                        postMonth = month.Value;
                    else
                        postMonth = postMothList.MonthList.LastOrDefault();
                }
                ViewBag.PostMonth = postMonth;

                // get main list of articles
                postMothList.PostList = (from a in articleListDefault
                                         where a.PostYear == postYear && a.PostMonth == postMonth
                                         select a).ToPagedList(pageNumber, pageSize);

                var yesterday = DateTime.Now.AddDays(-1).Date;
                var yesterdayEnd = DateTime.Today.Date;
                postMothList.MostViewTopicList = from p in memberContext.PopularTopicsAggregate
                                                 join t in memberContext.TopicMaster on p.TopicID equals t.TopicID
                                                 where p.TargetDate >= yesterday && p.TargetDate < yesterdayEnd
                                                 orderby p.Views descending
                                                 select t;
                return View(postMothList);
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsClassID"> wrong paramesster name : should be topicId</param>
        /// <param name="contributeID"></param>
        /// <param name="page"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult SearchByCategory(int? newsClassID, long? contributeID, int? page, int? year, int? month)
        {
            //#1295対応 Route5_3で呼ばれたとき(contributeIDあり)は新規のURLに301リダイレクト
            //return RedirectToActionPermanent("Show", "Splg.Controllers.UserArticle", new { area = "", contributeID = contributeID });

            ViewBag.pageNO = "5-3";
            long memberID = 0;
            string email = string.Empty;
            string username = string.Empty;
            if (Session["CurrentUser"] != null)
            {
                memberID = Convert.ToInt64(Session["CurrentUser"].ToString());
                email = memberContext.Member.SingleOrDefault(m => m.MemberId == memberID).Mail;
                username = memberContext.Member.SingleOrDefault(m => m.MemberId == memberID).Nickname;
                ViewBag.UserName = username;
            }
            string topicName = memberContext.TopicMaster.SingleOrDefault(t => t.TopicID == newsClassID.Value).TopicName;
            if (newsClassID != null && contributeID != null)
            {
                var article = (from con in memberContext.Contribution
                               join conQuot in memberContext.ContributionQuotOrg on con.ContributeId equals conQuot.ContributeId
                               join quotTopic in memberContext.QuotTopic on conQuot.ContributeId equals quotTopic.ContributeID
                               join m in memberContext.Member on con.MemberId equals m.MemberId
                               join tm in memberContext.TopicMaster on quotTopic.TopicID equals tm.TopicID
                               where quotTopic.TopicID == newsClassID.Value && con.ContributeId == contributeID
                               select new UserArticleInfoViewModel
                               {
                                   SportID = tm.SportID,
                                   ContributeId = con.ContributeId,
                                   Title = con.Title,
                                   Body = con.Body,
                                   ImageLink = con.ContributedPicture,
                                   ContributeDate = con.ContributeDate.Value,
                                   ModifiedDate = con.ModifiedDate,
                                   MemberId = con.MemberId,
                                   TopicID = quotTopic.TopicID,
                                   NewsClassId = conQuot.NewsClassId,
                                   QuoteUniqueId1 = conQuot.QuoteUniqueId1,
                                   QuoteUniqueId2 = conQuot.QuoteUniqueId2,
                                   QuoteUniqueId3 = conQuot.QuoteUniqueId3,
                                   Status = conQuot.Status,
                                   Nickname = m.Nickname,
                                   ProfileImage = m.ProfileImg
                               }).SingleOrDefault();
                if (article != null)
                {
                    if (article.Status == 1)
                    {
                        article = AppliedDetailContent(article);
                    }
                    //(from quotTopic in memberContext.QuotTopic
                    //                   join topicMST in memberContext.TopicMaster on quotTopic.TopicID equals topicMST.TopicID
                    //                   where quotTopic.ContributeID == contributeID
                    //                   select new RelatedTopicViewModel
                    //                   {
                    //                       TopicId = quotTopic.TopicID,
                    //                       TopicName = topicMST.TopicName,
                    //                       TopicURL = topicMST.JumpUrl
                    //                   }).ToList<RelatedTopicViewModel>();
                    var articleListDefault = PostedController.GetRecentPosts(8, newsClassID.Value);
                    //articleListDefault = PostedController.GetRecentPosts(7, newsClassID.Value);
                    article.RightNewsestPosts = PostedController.GetRecentPosts(10, Convert.ToInt32(article.MemberId));
                    if (Session["CurrentUser"] != null)
                    {
                        int thisYear = DateTime.Now.Year;
                        int thisMonth = DateTime.Now.Month;

                        string query = "SELECT MemberID," +
                                        "       ISNULL(SUM(PayOffPoints), 0) AS PayOffPoints," +
                                        "       ISNULL(SUM(fundspoint), 0) AS FundsPoint," +
                                        "       ISNULL(SUM(possesionpoint), 0) AS PossesionPoint," +
                                        "       MAX(ExpectPoint_CreatedDate) AS LastExpectedPointDate " +
                                        "FROM [Splg].[Com].[Point] p " +
                                        "LEFT JOIN" +
                                        "  (SELECT PointID," +
                                        "          MAX(CreatedDate) AS ExpectPoint_CreatedDate" +
                                        "   FROM [Splg].[Com].[ExpectPoint]" +
                                        "   GROUP BY PointID) e ON p.PointID = e.PointID " +
                                        "WHERE p.PayOffFlg = '1'" +
                                        "  AND p.GiveTargetYear =  " + thisYear +
                                        "  AND p.GiveTargetMonth = " + thisMonth +
                                        " GROUP BY p.memberid";


                        var pointDatas = memberContext.Database.SqlQuery<FollowerMemberForUser>(@query).ToList<FollowerMemberForUser>();

                        article.FollowedUserInfo = (from m in memberContext.Member
                                                    from f2 in memberContext.FollowList.Where(x => x.MemberID == article.MemberId && x.FollowerMemberID == memberID).DefaultIfEmpty()
                                                    where m.MemberId == article.MemberId
                                                    select new FollowerMemberForUser
                                                    {
                                                        MemberId = m.MemberId,
                                                        Nickname = m.Nickname,
                                                        ProfileImg = m.ProfileImg,
                                                        IsFollowing = (f2.MemberID != null),
                                                    }).FirstOrDefault();
                        if (pointDatas != null)
                        {
                            var p = pointDatas.SingleOrDefault(x => x.MemberId == article.MemberId);
                            if (p != null && p.MemberId > 0)
                            {
                                article.FollowedUserInfo.PayOffPoints = p.PayOffPoints;
                                article.FollowedUserInfo.FundsPoint = p.FundsPoint;
                                article.FollowedUserInfo.PossesionPoint = p.PossesionPoint;
                                article.FollowedUserInfo.LastExpectedPointDate = p.LastExpectedPointDate;
                            }
                        }
                    }
                    article.RelatedPostList = (from ar in articleListDefault
                                               where ar.ContributeId != contributeID.Value
                                               orderby ar.ContributeDate descending
                                               select ar).Take(10);
                    ViewBag.TopicID = newsClassID.Value;
                    if (article != null && contributeID != null)
                    {
                        ViewBag.SportID = article.SportID;
                        SaveNewRecordToContributionReading(contributeID.Value);
                    }
                    return View("ViewArticleDetail", article);
                }
                return View("Error410");
            }
            else if (newsClassID != null && contributeID == null)
            {
                int topicID = newsClassID.Value;
                ViewBag.pageNO = "5-2";
                var articleListDefault = default(IEnumerable<PostedInfoViewModel>);
                if (Session["CurrentUser"] != null)
                {
                    articleListDefault = PostedController.GetRecentPosts(7, topicID);
                }
                else
                {
                    articleListDefault = PostedController.GetRecentPosts(8, topicID);
                }
                if (articleListDefault == null)
                {
                    ViewBag.SportID = 0;
                    ViewBag.TopicID = topicID;
                    return View("Index");
                }
                int pageNumber = (page ?? 1);
                int postYear = (year != null) ? year.Value : DateTime.Now.Year;
                int postMonth = DateTime.Now.Month;
                var spara = memberContext.SystemParamater.Find(1);
                int pageSize = 10;
                if (spara != null)
                    pageSize = Convert.ToInt32(spara.Spara);

                var postMothList = new UserArticleMonthListViewModel();
                postMothList.Year = postYear;
                postMothList.YearList = (from a in articleListDefault
                                         select a).GroupBy(m => m.PostYear).Select(l => l.Key).ToList();
                if (postMothList.YearList != null)
                    postMothList.YearList.Sort();
                postMothList.YearList.Sort();
                postMothList.MonthList = (from a in articleListDefault
                                          where a.PostYear == postYear
                                          select a).GroupBy(m => m.PostMonth).Select(l => l.Key).ToList();
                if (postMothList.MonthList != null && postMothList.MonthList.Any())
                {
                    postMothList.MonthList.Sort();
                    if (month != null)
                        postMonth = month.Value;
                    else
                        postMonth = postMothList.MonthList.LastOrDefault();
                }
                ViewBag.PostMonth = postMonth;

                // get main list of articles
                postMothList.PostList = (from a in articleListDefault
                                         where a.PostYear == postYear && a.PostMonth == postMonth
                                         select a).ToPagedList(pageNumber, pageSize);

                DateTime yesterday = DateTime.Now.AddDays(-1).Date;
                DateTime yesterdayEnd = DateTime.Today.Date;
                postMothList.MostViewTopicList = from p in memberContext.PopularTopicsAggregate
                                                 join t in memberContext.TopicMaster on p.TopicID equals t.TopicID
                                                 where p.TargetDate >= yesterday && p.TargetDate < yesterdayEnd
                                                 orderby p.Views descending
                                                 select t;

                ViewBag.SportID = 0;
                ViewBag.TopicID = topicID;
                return View("Index", postMothList);
            }
            return View("Error");
        }



        /// <summary>
        /// 記事の閲覧（投稿記事IDからのみ）
        /// </summary>
        /// <param name="contributeID"></param>
        /// <returns></returns>
        public ActionResult Show(long? contributeID)
        {
            ViewBag.pageNO = "5-3";
            long memberID = 0;
            string email = string.Empty;
            string username = string.Empty;

            if (Session["CurrentUser"] != null)
            {
                memberID = Convert.ToInt64(Session["CurrentUser"].ToString());
                email = memberContext.Member.SingleOrDefault(m => m.MemberId == memberID).Mail;
                username = memberContext.Member.SingleOrDefault(m => m.MemberId == memberID).Nickname;
                ViewBag.UserName = username;
            }

            //QuotTopic 
            if (contributeID != null)
            {
                var article = (from con in memberContext.Contribution
                               join conQuot in memberContext.ContributionQuotOrg on con.ContributeId equals conQuot.ContributeId
                               join quotTopic in memberContext.QuotTopic on conQuot.ContributeId equals quotTopic.ContributeID
                               join m in memberContext.Member on con.MemberId equals m.MemberId
                               join tm in memberContext.TopicMaster on quotTopic.TopicID equals tm.TopicID
                               where con.ContributeId == contributeID
                               select new UserArticleInfoViewModel
                               {
                                   SportID = tm.SportID,
                                   ContributeId = con.ContributeId,
                                   Title = con.Title,
                                   Body = con.Body,
                                   ImageLink = con.ContributedPicture,
                                   ContributeDate = con.ContributeDate.Value,
                                   ModifiedDate = con.ModifiedDate,
                                   MemberId = con.MemberId,
                                   TopicID = quotTopic.TopicID,     //最初に検索されたTOPICIDが使用される
                                   NewsClassId = conQuot.NewsClassId,
                                   QuoteUniqueId1 = conQuot.QuoteUniqueId1, 
                                   QuoteUniqueId2 = conQuot.QuoteUniqueId2,
                                   QuoteUniqueId3 = conQuot.QuoteUniqueId3,
                                   Status = conQuot.Status,
                                   Nickname = m.Nickname,
                                   ProfileImage = m.ProfileImg
                               }).FirstOrDefault();

                if (article == null) return View("Error410");

                if (article.Status == 1)
                {
                    article = AppliedDetailContent(article);
                }

                var topicMasters = (from con in memberContext.Contribution
                                join conQuot in memberContext.ContributionQuotOrg on con.ContributeId equals conQuot.ContributeId
                                join quotTopic in memberContext.QuotTopic on conQuot.ContributeId equals quotTopic.ContributeID
                                join tm in memberContext.TopicMaster on quotTopic.TopicID equals tm.TopicID
                                where con.ContributeId == contributeID
                                select new
                                {
                                    tm.TopicID,
                                    tm.ClassificationType,  //１・・スポーツ、２・・チーム、３・・選手、４・・試合、5・・リーグ 6 予想分析
                                    tm.SportID,
                                    tm.UniqueID
                                }).ToList();

                IEnumerable<PostedInfoViewModel> articleListDefault = default(IEnumerable<PostedInfoViewModel>);

                //TODO GetRecentPostsがカオスで使用方法がわからない。
                //このメソッドの元メソッドにTEAMの場合のケースがあるので、とりあえずそれのみ再現
                foreach (var tm in topicMasters)
                {
                    switch((int)tm.ClassificationType){

                        case (int)UserArticleConst.ClassificationTypes.SPORT:
                            //articleListDefault = articleListDefault.Union(PostedController.GetRecentPosts(8, (int)tm.SportID), new PostedInfoViewModelSimpleComparer());
                            break;
                        case (int)UserArticleConst.ClassificationTypes.TEAM:

                            //こんな感じかと思ったが、、、
                            //articleListDefault.Union(PostedController.GetRecentPosts(
                            //Constants.NPB_POST_TEAM_TYPE, //どのスポーツもTOPページの投稿記事表示でNPB_POST_TEAM_TYPEをセットしている
                            //tm.SportID,
                            //tm.UniqueID,
                            //(int)tm.ClassificationType));

                            //こうすれば取得できるようで、、、UNIONがうまくいかないのでひとまず１パターンのみ対応
                            //articleListDefault = articleListDefault.Union(PostedController.GetRecentPosts(8, (int)tm.TopicID), new PostedInfoViewModelSimpleComparer());
                            articleListDefault = PostedController.GetRecentPosts(8, (int)tm.TopicID);
                            break;
                        case (int)UserArticleConst.ClassificationTypes.LEAGUE:
                            //articleListDefault = articleListDefault.Union(PostedController.GetRecentPosts(3, (int)tm.SportID, tm.UniqueID, (int)tm.ClassificationType), new PostedInfoViewModelSimpleComparer());
                            break;

                    }
                }

                //articleListDefault = PostedController.GetRecentPosts(7, newsClassID.Value);
                article.RightNewsestPosts = PostedController.GetRecentPosts(10, Convert.ToInt32(article.MemberId));

                if (memberID > 0)
                //if (Session["CurrentUser"] != null)
                {
                    int thisYear = DateTime.Now.Year;
                    int thisMonth = DateTime.Now.Month;

                    string query = "SELECT MemberID," +
                                    "       ISNULL(SUM(PayOffPoints), 0) AS PayOffPoints," +
                                    "       ISNULL(SUM(fundspoint), 0) AS FundsPoint," +
                                    "       ISNULL(SUM(possesionpoint), 0) AS PossesionPoint," +
                                    "       MAX(ExpectPoint_CreatedDate) AS LastExpectedPointDate " +
                                    "FROM [Splg].[Com].[Point] p " +
                                    "LEFT JOIN" +
                                    "  (SELECT PointID," +
                                    "          MAX(CreatedDate) AS ExpectPoint_CreatedDate" +
                                    "   FROM [Splg].[Com].[ExpectPoint]" +
                                    "   GROUP BY PointID) e ON p.PointID = e.PointID " +
                                    "WHERE p.PayOffFlg = '1'" +
                                    "  AND p.GiveTargetYear =  " + thisYear +
                                    "  AND p.GiveTargetMonth = " + thisMonth +
                                    " GROUP BY p.memberid";


                    var pointDatas = memberContext.Database.SqlQuery<FollowerMemberForUser>(@query).ToList<FollowerMemberForUser>();

                    article.FollowedUserInfo = (from m in memberContext.Member
                                                from f2 in memberContext.FollowList.Where(x => x.MemberID == article.MemberId && x.FollowerMemberID == memberID).DefaultIfEmpty()
                                                where m.MemberId == article.MemberId
                                                select new FollowerMemberForUser
                                                {
                                                    MemberId = m.MemberId,
                                                    Nickname = m.Nickname,
                                                    ProfileImg = m.ProfileImg,
                                                    IsFollowing = (f2.MemberID != null),
                                                }).FirstOrDefault();

                    if (pointDatas != null)
                    {
                        var p = pointDatas.SingleOrDefault(x => x.MemberId == article.MemberId);
                        if (p != null && p.MemberId > 0)
                        {
                            article.FollowedUserInfo.PayOffPoints = p.PayOffPoints;
                            article.FollowedUserInfo.FundsPoint = p.FundsPoint;
                            article.FollowedUserInfo.PossesionPoint = p.PossesionPoint;
                            article.FollowedUserInfo.LastExpectedPointDate = p.LastExpectedPointDate;
                        }
                    }
                }

                if (articleListDefault != null)
                {
                    article.RelatedPostList = (from ar in articleListDefault
                                               where ar.ContributeId != contributeID
                                               orderby ar.ContributeDate descending
                                               select ar).Take(10);
                }

                ViewBag.TopicID = article.TopicID;

                if (article != null && contributeID != null)
                {
                    ViewBag.SportID = article.SportID;
                    SaveNewRecordToContributionReading(contributeID.Value);
                }

                return View("ViewArticleDetail", article);

            }

            return View("Error");
        }



        public ActionResult ViewArticleBySport(int? sportID, int? page, int? year, int? month)
        {
            if (sportID.HasValue)
            {
                var articleListDefault = default(IEnumerable<PostedInfoViewModel>);
                if (Session["CurrentUser"] != null)
                {
                    articleListDefault = PostedController.GetRecentPosts(6, sportID.Value);
                }
                else
                {
                    articleListDefault = PostedController.GetRecentPosts(0, sportID.Value);
                }
                if (articleListDefault == null)
                {
                    ViewBag.SportID = sportID.Value;
                    return View("Index");
                }
                int pageNumber = (page ?? 1);
                int postYear = (year != null) ? year.Value : DateTime.Now.Year;
                int postMonth = DateTime.Now.Month;
                var spara = memberContext.SystemParamater.Find(1);
                int pageSize = 10;
                if (spara != null)
                    pageSize = Convert.ToInt32(spara.Spara);

                var postMothList = new UserArticleMonthListViewModel();
                postMothList.Year = postYear;
                postMothList.YearList = (from a in articleListDefault
                                         select a).GroupBy(m => m.PostYear).Select(l => l.Key).ToList();
                if (postMothList.YearList != null)
                    postMothList.YearList.Sort();
                postMothList.YearList.Sort();
                postMothList.MonthList = (from a in articleListDefault
                                          where a.PostYear == postYear
                                          select a).GroupBy(m => m.PostMonth).Select(l => l.Key).ToList();
                if (postMothList.MonthList != null && postMothList.MonthList.Any())
                {
                    postMothList.MonthList.Sort();
                    if (month != null)
                        postMonth = month.Value;
                    else
                        postMonth = postMothList.MonthList.LastOrDefault();
                }
                ViewBag.PostMonth = postMonth;

                // get main list of articles
                postMothList.PostList = (from a in articleListDefault
                                         where a.PostYear == postYear && a.PostMonth == postMonth
                                         select a).ToPagedList(pageNumber, pageSize);

                DateTime yesterday = DateTime.Now.AddDays(-1).Date;
                DateTime yesterdayEnd = DateTime.Today.Date;
                postMothList.MostViewTopicList = from p in memberContext.PopularTopicsAggregate
                                                 join t in memberContext.TopicMaster on p.TopicID equals t.TopicID
                                                 where p.TargetDate >= yesterday && p.TargetDate < yesterdayEnd
                                                 orderby p.Views descending
                                                 select t;

                ViewBag.SportID = sportID.Value;
                return View("Index", postMothList);
            }

            return View("Error");
        }
        #endregion

        #region 5-4 記事入力
        public ActionResult AddNewArticle(long newsClassID, long quoteUniqueId1, long? quoteUniqueId2, long? quoteUniqueId3)
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var userArticleInfoViewModel = new UserArticleInfoViewModel();
            userArticleInfoViewModel.NewsClassId = newsClassID;
            userArticleInfoViewModel.QuoteUniqueId1 = quoteUniqueId1;
            userArticleInfoViewModel.QuoteUniqueId2 = quoteUniqueId2 ?? 0;
            userArticleInfoViewModel.QuoteUniqueId3 = quoteUniqueId3 ?? 0;
            userArticleInfoViewModel.Status = 1;
            userArticleInfoViewModel.Title = string.Empty;
            userArticleInfoViewModel.Body = string.Empty;
            userArticleInfoViewModel.ImageLink = UserArticleInfoViewModel.DefaultImageLink;

            userArticleInfoViewModel = AppliedDetailContent(userArticleInfoViewModel);

            return View(userArticleInfoViewModel);
        }

        public ActionResult ConfirmUserArticle()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var model = new UserArticleInfoViewModel();
            if (string.IsNullOrEmpty(model.Title))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.pageNO = "5-4-1";
                return View();
            }
        }

        [HttpPost]
        public ActionResult ConfirmUserArticle(UserArticleInfoViewModel model)
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            ViewBag.pageNO = "5-4-1";
            try
            {
                long memberID = Convert.ToInt64(Session["CurrentUser"]);
                string imageDataUrl = model.DataURL;
                string rootPath = Server.MapPath("~");
                //string imagePath = "\\Content\\img\\upload\\contribution\\";
                string tempImagePath = "\\Content\\img\\upload\\contribution\\tmp_dir\\";
                //string filePath = Path.GetFullPath(rootPath + imagePath);
                string tempFilePath = Path.GetFullPath(rootPath + tempImagePath);
                string fileName = "";
                try
                {
                    if (model.ContributeId <= 0 || string.IsNullOrEmpty(model.ImageLink) || !string.IsNullOrEmpty(model.DataURL))
                    {
                        if (!System.IO.Directory.Exists(tempFilePath))
                            System.IO.Directory.CreateDirectory(tempFilePath);
                        if (!string.IsNullOrEmpty(model.DataURL))
                        {
                            fileName = Utils.SaveImageToDirectory(imageDataUrl, tempFilePath);
                            model.ImageName = fileName;
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                model.ImageLink = @"~/Content/img/upload/contribution/tmp_dir/" + fileName;
                            }
                        }
                        else
                        {
                            model.ImageLink = UserArticleInfoViewModel.DefaultImageLink;
                        }

                    }
                }
                catch
                {
                    fileName = "";
                }
                if (model.ContributeId <= 0)
                    model.ContributeDate = System.DateTime.Now;
                model.MemberId = memberID;
                model.Nickname = memberContext.Member.SingleOrDefault(m => m.MemberId == memberID).Nickname;
                //model.RelatedTopicsList = (UserArticleRelatedTopicViewModel)Session["RelatedTopic"];
                //Session["NewPost"] = model;
                model = AppliedDetailContent(model);
                return View(model);
                //}
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult CompleteAddNewArticle()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var model = new UserArticleInfoViewModel();
            if (string.IsNullOrEmpty(model.Title))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.pageNO = "5-4-2";
                return View();
            }

        }


        [HttpPost]
        public ActionResult CompleteAddNewArticle(UserArticleInfoViewModel model)
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            ViewBag.pageNO = "5-4-2";
            bool transactionResult = true;
            List<QuotTopic> topicList = new List<QuotTopic>();
            Int64 memberId = Convert.ToInt64(Session["CurrentUser"].ToString());
            long newContributionID = 0;
            if (model != null)
            {
                if (model.ContributeId > 0)
                {
                    var article = (from a in memberContext.Contribution
                                   where a.ContributeId == model.ContributeId
                                   select a).FirstOrDefault();
                    if (article != null)
                    {
                        newContributionID = model.ContributeId;
                        if (ModelState.IsValid)
                        {
                            using (var newArticleTransaction = memberContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    article.Title = model.Title;
                                    article.Body = model.Body;
                                    //article.MemberId = model.MemberId;
                                    if (!string.IsNullOrEmpty(model.ImageName))
                                    {
                                        article.ContributedPicture = @"~/Content/img/upload/contribution/" + model.ImageName;
                                    }
                                    //article.ContributeDate = model.ContributeDate;
                                    article.ModifiedDate = DateTime.Now;
                                    memberContext.Entry(article).State = EntityState.Modified;
                                    memberContext.SaveChanges();
                                    newArticleTransaction.Commit();
                                    transactionResult = true;
                                }
                                catch
                                {
                                    transactionResult = false;
                                    newArticleTransaction.Rollback();
                                }

                            }
                        }
                        if (transactionResult && !string.IsNullOrEmpty(model.OldImageLink))
                        {
                            string tempImagePath = "\\Content\\img\\upload\\contribution\\";
                            string rootPath = Server.MapPath("~");
                            string tempFilePath = Path.GetFullPath(rootPath + tempImagePath);
                            try
                            {
                                if (System.IO.File.Exists(tempFilePath + model.OldImageLink))
                                {
                                    FileInfo f = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/img/upload/contribution/" + model.OldImageLink));
                                    f.Delete();
                                }
                            }
                            catch
                            {
                                return View("Error");
                            }
                        }
                    }
                }
                else
                {
                    using (var newArticleTransaction = memberContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var newContributeRow = new Contribution
                            {
                                Title = model.Title,
                                Body = model.Body,
                                MemberId = memberId,
                                ContributedPicture = @"~/Content/img/upload/contribution/" + model.ImageName,
                                ContributeDate = DateTime.Now,
                                ModifiedDate = DateTime.Now
                            };
                            memberContext.Contribution.Add(newContributeRow);
                            memberContext.SaveChanges();
                            newContributionID = newContributeRow.ContributeId;
                            var newContributeQuotRow = new ContributionQuotOrg
                            {
                                ContributeId = newContributeRow.ContributeId,
                                NewsClassId = model.NewsClassId,
                                QuoteUniqueId1 = model.QuoteUniqueId1,
                                QuoteUniqueId2 = model.QuoteUniqueId2,
                                QuoteUniqueId3 = model.QuoteUniqueId3,
                                Status = 1
                            };
                            memberContext.ContributionQuotOrg.Add(newContributeQuotRow);
                            topicList = GetAllTopicList(newContributeRow.ContributeId,
                                GetRelatedTopicofAPost(model.NewsClassId, model.QuoteUniqueId1, model.QuoteUniqueId2, model.QuoteUniqueId3));
                            memberContext.QuotTopic.AddRange(topicList);
                            memberContext.SaveChanges();
                            newArticleTransaction.Commit();
                            transactionResult = true;
                        }
                        catch
                        {
                            transactionResult = false;
                            newArticleTransaction.Rollback();
                        }

                    }
                }
                if (transactionResult)
                {
                    string rootPath = Server.MapPath("~");
                    string imagePath = "\\Content\\img\\upload\\contribution\\";
                    string tempImagePath = "\\Content\\img\\upload\\contribution\\tmp_dir\\";
                    string filePath = Path.GetFullPath(rootPath + imagePath);
                    string tempFilePath = Path.GetFullPath(rootPath + tempImagePath);
                    try
                    {
                        if (System.IO.File.Exists(tempFilePath + model.ImageName) && !string.IsNullOrEmpty(model.ImageName))
                        {
                            System.IO.File.Copy(tempFilePath + model.ImageName, filePath + model.ImageName);
                            DeleteTemperaryDir(@"~/Content/img/upload/contribution/tmp_dir/");
                        }
                        else
                        {
                            if (System.IO.Directory.Exists(tempFilePath))
                                DeleteTemperaryDir(@"~/Content/img/upload/contribution/tmp_dir/");
                        }
                    }
                    catch
                    {
                        return View("Error");
                    }

                    var articleListDefault = PostedController.GetRecentPosts(4);
                    var newPost = from a in articleListDefault
                                  where a.ContributeId == newContributionID
                                  select a;

                    return View("CompletePostArticle", newPost);
                }
            }
            else
            {
                return View("Error");
            }
            return View("Error");
        }

        public ActionResult EditNewUserArticle()
        {
            if (Session["CurrentUser"] == null)
            {
                return RedirectToAction("Login", "Member");
            }
            else
            {
                UserArticleInfoViewModel model = new UserArticleInfoViewModel();
                if (string.IsNullOrEmpty(model.Title))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
        }


        [HttpPost]
        public ActionResult EditNewUserArticle(UserArticleInfoViewModel model)
        {
            model = AppliedDetailContent(model);
            if (model.ContributeId > 0)
            {
                var article = from a in memberContext.Contribution
                              where a.ContributeId == model.ContributeId
                              select a;
                if (article != null)
                {
                    if (string.IsNullOrEmpty(model.OldImageLink) && !string.IsNullOrEmpty(model.ImageLink) && !model.ImageLink.Contains("dummy_2.png"))
                    {
                        string imagePath = "\\Content\\img\\upload\\contribution\\";
                        string tempImagePath = "\\Content\\img\\upload\\contribution\\tmp_dir\\";
                        string rootPath = Server.MapPath("~");
                        string tempFilePath = Path.GetFullPath(rootPath + tempImagePath);
                        string filePath = Path.GetFullPath(rootPath + imagePath);
                        string fileName = "";
                        try
                        {
                            if (!System.IO.Directory.Exists(tempFilePath))
                                System.IO.Directory.CreateDirectory(tempFilePath);

                            fileName = model.ImageLink.Split('/').Last();
                            if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(filePath + fileName))
                            {
                                System.IO.File.Copy(filePath + fileName, tempFilePath + fileName);
                                model.ImageLink = @"~/Content/img/upload/contribution/tmp_dir/" + fileName;
                                model.OldImageLink = fileName;
                                model.ImageName = fileName;
                            }

                        }
                        catch
                        {
                            fileName = "";
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult DeleteNewUserArticle(UserArticleInfoViewModel model)
        {
            if (Session["CurrentUser"] == null)
            {
                return RedirectToAction("Login", "Member");
            }
            string rootPath = Server.MapPath("~");
            string tempImagePath = "\\Content\\img\\upload\\contribution\\tmp_dir\\";
            string tempFilePath = Path.GetFullPath(rootPath + tempImagePath);
            string imagePath = "\\Content\\img\\upload\\contribution\\";
            string filePath = Path.GetFullPath(rootPath + imagePath);
            try
            {
                if (System.IO.Directory.Exists(tempFilePath))
                    DeleteTemperaryDir(@"~/Content/img/upload/contribution/tmp_dir/");
            }
            catch
            {
                return View("Error");
            }
            bool transactionResult = false;
            if (model.ContributeId > 0)
            {
                Contribution record = (from c in memberContext.Contribution
                                       where c.ContributeId == model.ContributeId
                                       select c).FirstOrDefault();
                if (record != null)
                {
                    using (var newArticleTransaction = memberContext.Database.BeginTransaction())
                    {
                        var quoteTopics = from q in memberContext.QuotTopic
                                          where q.ContributeID == model.ContributeId
                                          select q;
                        var contributedOrgs = from co in memberContext.ContributionQuotOrg
                                              where co.ContributeId == model.ContributeId
                                              select co;

                        try
                        {
                            foreach (var qt in quoteTopics)
                            {
                                memberContext.QuotTopic.Remove(qt);
                            }
                            foreach (var cqo in contributedOrgs)
                            {
                                memberContext.ContributionQuotOrg.Remove(cqo);
                            }
                            memberContext.Contribution.Remove(record);
                            memberContext.SaveChanges();

                            newArticleTransaction.Commit();
                            transactionResult = true;
                        }
                        catch
                        {
                            transactionResult = false;
                            newArticleTransaction.Rollback();
                        }
                    }
                    if (transactionResult && !string.IsNullOrEmpty(model.OldImageLink))
                    {
                        if (System.IO.File.Exists(filePath + model.OldImageLink))
                        {
                            FileInfo f = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/img/upload/contribution/" + model.OldImageLink));
                            f.Delete();
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        private List<QuotTopic> GetAllTopicList(long contributeID, List<RelatedTopicViewModel> topicList)
        {
            List<QuotTopic> result = new List<QuotTopic>();
            foreach (var item in topicList)
            {
                var newQuotTopicRow = new QuotTopic
                {
                    TopicID = item.TopicId,
                    ContributeID = contributeID
                };
                result.Add(newQuotTopicRow);
            }
            return result;
        }
        private static List<RelatedTopicViewModel> GetRelatedTopicofAPost(long newsClassID, long quoteUniqueId1, long quoteUniqueId2 = 0, long quoteUniqueId3 = 0)
        {
            IEnumerable<RelatedTopicViewModel> result = default(IEnumerable<RelatedTopicViewModel>);
            int uniqueId2 = Convert.ToInt32(quoteUniqueId2);
            int uniqueId3 = Convert.ToInt32(quoteUniqueId3);
            switch (newsClassID)
            {
                case 1:
                    result = from newsTopic in memberContext.NewsTopic
                             join topicMaster in memberContext.TopicMaster on newsTopic.TopicID equals topicMaster.TopicID
                             where newsTopic.NewsItemID == quoteUniqueId1
                             select new RelatedTopicViewModel
                             {
                                 TopicId = topicMaster.TopicID,
                                 TopicName = topicMaster.TopicName,
                                 TopicURL = topicMaster.JumpUrl
                             } into rt
                             select rt;
                    return result.ToList();
                case 2:
                    result = from t in memberContext.TopicMaster
                             where t.SportID == quoteUniqueId1 && t.ClassificationType == 2 && t.UniqueID == uniqueId3
                             select new RelatedTopicViewModel
                             {
                                 TopicId = t.TopicID,
                                 TopicName = t.TopicName,
                                 TopicURL = t.JumpUrl
                             } into rt
                             select rt;
                    break;
                case 3:
                    switch (quoteUniqueId1)
                    {
                        case 1:
                        case 2:
                            result = from t in memberContext.TopicMaster
                                     where t.SportID == quoteUniqueId1
                                     && (t.ClassificationType == 2 && t.UniqueID == uniqueId2)
                                     || (t.ClassificationType == 3 && t.UniqueID == uniqueId3)
                                     select new RelatedTopicViewModel
                                     {
                                         TopicId = t.TopicID,
                                         TopicName = t.TopicName,
                                         TopicURL = t.JumpUrl
                                     } into rt
                                     select rt;
                            break;
                        case 3:
                            result = from t in memberContext.TopicMaster
                                     where t.SportID == quoteUniqueId1
                                     && t.ClassificationType == 2
                                     && t.UniqueID == uniqueId2
                                     select new RelatedTopicViewModel
                                     {
                                         TopicId = t.TopicID,
                                         TopicName = t.TopicName,
                                         TopicURL = t.JumpUrl
                                     } into rt
                                     select rt;
                            break;
                    }
                    break;
                case 4:
                case 5:
                    switch (quoteUniqueId1)
                    {
                        case 1:
                            var npbGame = (from g in npbContext.GameInfoSS
                                           where g.ID == uniqueId2
                                           select g).FirstOrDefault();
                            uniqueId2 = npbGame.HomeTeamID.Value;
                            uniqueId3 = npbGame.VisitorTeamID.Value;
                            break;
                        case 2:
                            var jlgGame = (from g in jlgContext.ScheduleInfo
                                           where g.GameID == uniqueId2
                                           select g).FirstOrDefault();
                            uniqueId2 = jlgGame.HomeTeamID.Value;
                            uniqueId3 = jlgGame.AwayTeamID.Value;
                            break;
                        case 3:
                            var mlbGame = (from g in mlbContext.SeasonSchedule
                                           where g.GameID == uniqueId2
                                           select g).FirstOrDefault();
                            uniqueId2 = mlbGame.HomeTeamID.Value;
                            uniqueId3 = mlbGame.VisitorTeamID.Value;
                            break;
                    }
                    result = from t in memberContext.TopicMaster
                             where t.SportID == quoteUniqueId1
                             && t.ClassificationType == 2
                             && (t.UniqueID == uniqueId2 || t.UniqueID == uniqueId3)
                             select new RelatedTopicViewModel
                             {
                                 TopicId = t.TopicID,
                                 TopicName = t.TopicName,
                                 TopicURL = t.JumpUrl
                             } into rt
                             select rt;
                    break;
                case 6:
                    //short thisYear = Convert.ToInt16(quoteUniqueId2 / 100);


                    //short releVantMonth = Convert.ToInt16(quoteUniqueId2 % 100);



                    //var monthlyResult = from m in memberContext.MonthlyResults
                    //                 where m.ReleVantMonth == releVantMonth
                    //                 && m.ReleVantYear == thisYear
                    //                 && m.MemberID == quoteUniqueId1
                    //                 select m;

                    //if (monthlyResult != null)
                    //{
                    //    result = from t in memberContext.TopicMaster
                    //             join m in monthlyResult on t.SportID equals m.SportsID
                    //             where t.ClassificationType == 1
                    //             //&& t.SportID == monthlyResult.SportsID
                    //             //&& (t.UniqueID == uniqueId2)
                    //             select new RelatedTopicViewModel
                    //             {
                    //                 TopicId = t.TopicID,
                    //                 TopicName = t.TopicName,
                    //                 TopicURL = t.JumpUrl
                    //             } into rt
                    //             select rt;
                    //}






















                    result = from t in memberContext.TopicMaster
                             where t.ClassificationType == 6



                             select new RelatedTopicViewModel
                             {

                                 TopicId = t.TopicID,
                                 TopicName = t.TopicName,
                                 TopicURL = t.JumpUrl
                             } into rt
                             select rt;



                    break;
            }
            if (result == null)
                return default(List<RelatedTopicViewModel>);
            return result.ToList();
        }

        #region Save ContributeReading to Database
        /// <summary>
        /// When a article is viewed, a record is added to database for compute Top view of article
        /// </summary>
        /// <param name="newsItemId">id of post</param>
        /// <returns>void</returns>
        private void SaveNewRecordToContributionReading(long contributeId)
        {
            ContributionReading contributionReading = new ContributionReading();
            contributionReading.ContributeId = contributeId;
            contributionReading.CreatedDate = DateTime.Now;
            memberContext.ContributionReading.Add(contributionReading);
            memberContext.SaveChanges();
        }
        #endregion

        #region Delete a directory
        /// <summary>
        /// </summary>
        /// <param name="newsItemId">path</param>
        /// <returns>void</returns>
        private void DeleteTemperaryDir(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath(path));
            var files = dirInfo.GetFiles();
            foreach (var f in files)
            {
                if (f.Exists)
                    f.Delete();
            }
            // The Issue when delete an directory is the ISS will reset, then all session will be cleared
            //dirInfo.Delete(true);
        }
        #endregion

        #region
        public UserArticleInfoViewModel AppliedDetailContent(UserArticleInfoViewModel input)
        {
            UserArticleInfoViewModel model = input;
            switch (model.NewsClassId)
            {
                case 1:
                    model.NewsInfo = (from brief in memberContext.BriefNews
                                      join photo in memberContext.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
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
                                      } into news_photo
                                      where (news_photo.NewsItemID == model.QuoteUniqueId1
                                        && (news_photo.Duid == Constants.IMAGE_DUID || news_photo.Duid == null))
                                      select news_photo).FirstOrDefault();
                    if (model.SportID <= 0)
                    {
                        var sportTopic = (from bn in memberContext.BriefNews
                                          join nt in memberContext.NewsTopic on bn.NewsItemID equals nt.NewsItemID
                                          join tm in memberContext.TopicMaster on nt.TopicID equals tm.TopicID
                                          where bn.NewsItemID == model.QuoteUniqueId1 && tm.ClassificationType == 1
                                          select tm).FirstOrDefault();
                        if (sportTopic != null)
                            model.SportID = sportTopic.SportID;
                    }
                    break;
                case 2:
                    switch (model.QuoteUniqueId1)
                    {
                        case 1: // NPB
                            model.NpbTeamInfo = (new NpbTeamInfoTeamTopController()).GetTeamInfo(Convert.ToInt32(model.QuoteUniqueId3));
                            break;
                        case 2: //JLeague
                            int jType = 0;
                            var teamID = Convert.ToInt32(model.QuoteUniqueId3);
                            if (Session["JType"] != null)
                            {
                                jType = Convert.ToInt32(Session["JType"].ToString());
                            }
                            else
                            {
                                jType = GetJlgTypeByTeamId(teamID);
                            }
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
                            model.JlgTeamInfo = (new JlgTeamInfoTopController()).GetTeamInfo(teamID, gameKindID);
                            JlgEntities jlg = new JlgEntities();
                            model.RankInfoRT = (from r in jlg.RankInfoRT where r.TeamID == teamID select r).FirstOrDefault();
                            break;
                        case 3: //MLB
                            model.MlbTeamInfo = (new MlbTeamInfoTeamTopController()).GetTeamInfo(Convert.ToInt32(model.QuoteUniqueId3));
                            break;
                        case 4:
                            break;
                    }
                    break;
                case 3:
                    switch (model.QuoteUniqueId1)
                    {
                        case 1: //Npb
                            break;
                        case 2: //Jleague
                            var teamID = Convert.ToInt32(model.QuoteUniqueId2);
                            var playerID = Convert.ToInt32(model.QuoteUniqueId3);
                            model.PlayerInfoYear = (new JlgTeamInfoPlayerDetailController()).GetPlayerInfo(teamID, playerID);
                            model.PlayerSum = (new JlgTeamInfoPlayerDetailController()).GetPlayerInfo_Sum(teamID, playerID);
                            break;
                        case 3: //Mlb
                            model.MlbTeamInfo = (new MlbTeamInfoTeamTopController()).GetTeamInfo(Convert.ToInt32(model.QuoteUniqueId2));
                            break;
                    }
                    break;
                case 6:
                    ViewBag.Year = model.QuoteUniqueId2 / 100;
                    ViewBag.Month = model.QuoteUniqueId2 % 100;
                    //ViewBag.NumType = model.QuoteUniqueId3;
                    //ViewBag.OrtherMemberId = model.QuoteUniqueId1;
                    break;
            }
            model.RelatedTopicList = GetRelatedTopicofAPost(model.NewsClassId, model.QuoteUniqueId1, model.QuoteUniqueId2, model.QuoteUniqueId3);
            return model;
        }
        #endregion

        #region Get Jleague GameKindId From teamId

        public int GetJlgTypeByTeamId(int teamId)
        {
            int jType = 0;
            var jlgTeams = (from categoryGT in jlgContext.CategoryGT
                            join teamInfoGT in jlgContext.TeamInfoGT on categoryGT.CategoryGTId equals teamInfoGT.CategoryGTId
                            where teamInfoGT.TeamID == teamId
                            && (categoryGT.GameKindID == 2 || categoryGT.GameKindID == 4 || categoryGT.GameKindID == 6)
                            select new
                            {
                                TeamID = teamInfoGT.TeamID,
                                TeamName = teamInfoGT.TeamName,
                                LeagueID = categoryGT.GameKindID
                            }).FirstOrDefault();
            if (jlgTeams != null)
                jType = jlgTeams.LeagueID;
            return jType;
        }

        #endregion

        #region Change YearMonth
        /// <summary>
        /// Change YearMonth
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult GetMemberReportInfo(long memberId, string yearmonth)
        {
            UserTopViewModel userTop = new UserTopViewModel();
            int iyear = Convert.ToInt16(yearmonth.Split('-')[0]);
            int imonth = Convert.ToInt16(yearmonth.Split('-')[1]);
            userTop.ReportInfo = GetMonthlyResults(memberId, iyear, imonth);

            return Json(userTop.ReportInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region レポート情報の取得
        private IEnumerable<UserTopViewModel.ReportInfoModel> GetMonthlyResults(long memberId, int year, int month)
        {
            var report = (from s in memberContext.SportsMaster
                          join r in memberContext.MonthlyResults on s.SportsID equals r.SportsID //海外サッカーを表示する場合はleft join
                          where r.MemberID == memberId && r.ReleVantYear == year && r.ReleVantMonth == month
                          orderby r.SportsID
                          select new UserTopViewModel.ReportInfoModel
                          {
                              CorrectPercent = r.CorrectPercent,
                              CorrectPoint = r.CorrectPoint,
                              ExpectNumber = r.ExpectNumber,
                              MemberID = r.MemberID,
                              SportsID = r.SportsID,
                              SportsName = s.SportsName

                          }
            ).ToList();

            return report;
        }
        #endregion

    }
}