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
 * Class		: MyPageArticleController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using Splg.Areas.MyPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Models.News.ViewModel;
using Splg.Models.UserArticle.ViewModel;
using PagedList;
#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageArticleController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();
        #endregion

        /// <summary>
        /// GET: /mypage/article/
        /// </summary>
        [HttpGet]
        public ActionResult Index(int? page, int? year, int? month)
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            //MyPageArticleViewModel viewModel = new MyPageArticleViewModel();

            //DBから取得する処理
            //SetLines(viewModel, 0, MyPageArticleViewModel.INITIAL_SIZE);
            var articleListDefault = PostedController.GetRecentPosts(9);
            if (articleListDefault != null)
            {
                int pageNumber = (page ?? 1);
                int postYear = (year != null) ? year.Value : DateTime.Now.Year;
                int postMonth = DateTime.Now.Month;
                var spara = com.SystemParamater.Find(1);
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
                postMothList.MostViewTopicList = from p in com.PopularTopicsAggregate
                                                 join t in com.TopicMaster on p.TopicID equals t.TopicID
                                                 where p.TargetDate >= yesterday && p.TargetDate < yesterdayEnd
                                                 orderby p.Views descending
                                                 select t;
                return View(postMothList);
            }
            return View();
        }

        /// <summary>
        /// もっと見る取得
        /// </summary>
        /// <param name="currentCount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult GetMoreArticles(int currentCount)
        {
            MyPageArticleViewModel viewModel = new MyPageArticleViewModel();

            int ajaxInitialSize = 3;

            //DBから取得する処理
            SetLines(viewModel, currentCount, currentCount == 0?ajaxInitialSize:MyPageArticleViewModel.INITIAL_PAGE_SIZE);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 投稿をDBから取得する処理
        /// </summary>
        /// <param name="viewModel">ビューモデル</param>
        /// <param name="skipCount">スキップする要素数</param>
        /// <param name="takeCount">返す要素数</param>
        private void  SetLines(MyPageArticleViewModel viewModel, int skipCount, int takeCount)
        {
            try
            {
                long memberId = -1;
                object currentUser = Session["CurrentUser"];

                if (currentUser != null)
                    memberId = Convert.ToInt64(currentUser.ToString());

                if (memberId > 0)
                {
                    //投稿（Contribution）テーブルを検索する
                    //(の投稿日時（ContributeDate）から該当月のもの) <=HTMLデザインに準じ、廃止
                    //同テーブルのタイトル（Title）を表示
                    var contributions = from c in com.Contribution
                                where c.MemberId == memberId
                                orderby c.ContributeDate descending, c.ContributeId
                                    select new ContributionForMyPage
                                {
                                    ContributeId = c.ContributeId,
                                    MemberId = c.MemberId,
                                    Title = c.Title,
                                    Body = c.Body,
                                    ContributedPicture = c.ContributedPicture,
                                    ContributeDate = c.ContributeDate,
                                    ModifiedDate = c.ModifiedDate

                                };

                    viewModel.TotalCount = contributions.Count();


                    //「投稿」テーブルの投稿ID で、「引用トピック」テーブルを読んで、トピックID を取得する。
                    //この　トピックID で、「トピックマスタ」を読んで、「分類種別」が　４の時のトピック名を表示
                    var topicTitles = from c in com.Contribution
                                from q in com.QuotTopic.Where(x => x.ContributeID == c.ContributeId).DefaultIfEmpty()
                                from t in com.TopicMaster.Where(x => x.TopicID == q.TopicID).DefaultIfEmpty()
                                where c.MemberId == memberId
                                && t.ClassificationType == 4
                                      select new ContributionForMyPage
                                {
                                    ContributeId = c.ContributeId,
                                    TopicTitles = t.TopicName,

                                };

                    //収集したトピックタイトルをビューで投稿に付与
                    //表示分読み込む
                    viewModel.Contributions  = new List<ContributionForMyPage> { };

                    bool topicFg = false;
                    IEnumerable<ContributionForMyPage> list = contributions.Skip(skipCount).Take(takeCount);

                    foreach (ContributionForMyPage c in list)
                    {
                        foreach (ContributionForMyPage t in topicTitles)
                        {
                            if (t.ContributeId == c.ContributeId)
                            {
                                topicFg = true;
                                c.TopicTitles += (c.TopicTitles == null ? "" : ",") + t.TopicTitles;
                            }
                            else
                            {
                                if (topicFg)
                                {
                                    break;
                                }
                            }

                        }

                        ((List<ContributionForMyPage>)viewModel.Contributions).Add(c);


                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}