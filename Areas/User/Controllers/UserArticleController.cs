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
 * Namespace	: Splg.Areas.User.Controllers
 * Class		: UserArticleController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using Splg.Areas.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Areas.User.Models.ViewModel;
using Splg.Areas.User.Models.InfoModel;
using Splg.Models.News.ViewModel;
using Splg.Models.UserArticle.ViewModel;
using PagedList;
#endregion

namespace Splg.Areas.User.Controllers
{
    public class UserArticleController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //UserEntities mypage = new UserEntities();
        #endregion


        /// <summary>
        /// ページの取得
        /// </summary>
        /// <param name="pageSize">1ページあたりのレコード件数</param>
        /// <param name="pageNo">ページ番号</param>
        /// <returns>ActionResult</returns>
        public ActionResult Index(long memberId, int? page, int? year, int? month)
        {
            //int pageNo = 1;
            //int pageSize = 3;

            //UserArticleViewModel viewModel = new UserArticleViewModel();

            Member member = Utils.GetMember(memberId);
            ViewBag.MemberId = memberId;
            ViewBag.Nickname = member.Nickname;

            ViewBag.OtherMemberID = memberId;
            ViewBag.OtherMemberNickName = member.Nickname;

            //ViewBag.PageNo = pageNo;
            //ViewBag.PageSize = pageSize;
            //SetArticles(viewModel, memberId, 0, UserArticleViewModel.INITIAL_SIZE);

            //return View(viewModel);
            var articleListDefault = PostedController.GetRecentPosts(10,Convert.ToInt32(memberId));
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
        #region Backup source
        /*
        /// <summary>
        /// 投稿のもっと見る取得
        /// </summary>
        /// <param name="currentCount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult GetMoreArticles(long memberId, int currentCount)
        {
            UserArticleViewModel viewModel = new UserArticleViewModel();

            //運営からのお知らせ
            SetArticles(viewModel, memberId, currentCount, UserArticleViewModel.INITIAL_PAGE_SIZE);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 投稿をDBから取得する処理
        /// </summary>
        /// <param name="viewModel">ビューモデル</param>
        /// <param name="memberId"></param>
        /// <param name="skipCount">スキップする要素数</param>
        /// <param name="takeCount">返す要素数</param>
        private void SetArticles(UserArticleViewModel viewModel, long memberId, int skipCount, int takeCount)
        {
            IEnumerable<ContributionForUser> contributions
                = new List<ContributionForUser> { };

            try
            {

                if (memberId > 0)
                {
                    //投稿（Contribution）テーブルを検索する
                    //(の投稿日時（ContributeDate）から該当月のもの) <=HTMLデザインに準じ、廃止
                    //同テーブルのタイトル（Title）を表示
                    contributions = from c in com.Contribution
                                    where c.MemberId == memberId
                                    orderby c.ContributeDate descending, c.ContributeId
                                    select new ContributionForUser
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

                    //表示分読み込む
                    viewModel.Contributions = contributions.Skip(skipCount).Take(takeCount);

                    //そして、「投稿」テーブルの投稿ID で、「引用トピック」テーブルを読んで、トピックID を取得する。
                    //この　トピックID で、「トピックマスタ」を読んで、「分類種別」が　４の時のトピック名を表示
                    var topicTitles = from c in com.Contribution
                                      from q in com.QuotTopic.Where(x => x.ContributeID == c.ContributeId).DefaultIfEmpty()
                                      from t in com.TopicMaster.Where(x => x.TopicID == q.TopicID).DefaultIfEmpty()
                                      where c.MemberId == memberId
                                      && t.ClassificationType == 4
                                      select new ContributionForUser
                                      {
                                          ContributeId = c.ContributeId,
                                          TopicTitles = t.TopicName,

                                      };

                    //収集したトピックタイトルをビューで投稿に付与
                    //表示分読み込む
                    viewModel.Contributions = new List<ContributionForUser> { };

                    bool topicFg = false;
                    IEnumerable<ContributionForUser> list = contributions.Skip(skipCount).Take(takeCount);

                    foreach (ContributionForUser c in list)
                    {
                        foreach (ContributionForUser t in topicTitles)
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

                        ((List<ContributionForUser>)viewModel.Contributions).Add(c);


                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

*/
        #endregion
    }
}