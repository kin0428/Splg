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
 * Class		: UserSearchController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System;
using System.Web.Mvc;
using Splg.Models;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Service;
using Splg.Services.System;

#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class UserSearchController : Controller
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
 
        // todo: interfaceへ変更
        private UserSearchService workerService;

        private SystemDatetimeService systemDatetimeService;

        #endregion

        public UserSearchController()
        {
            // todo インスタンス管理
            this.workerService = new UserSearchService(this.com);
            this.systemDatetimeService = new SystemDatetimeService();
        }

        /// <summary>
        /// GET: /user_search/
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定(フォーム認証部分も本来なら見るべき。)
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            // ViewModelを取得
            var viewModel = this.workerService.GetViewModel();

            return View(viewModel);
        }

        /// <summary>
        /// 検索アクション
        /// </summary>
        /// <param name="keyword">アカウント名の検索文字列</param>
        /// <param name="usersearchcount">スキップする要素数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult Search(string keyword, int usersearchcount = 0)
        {
            long memberId = this.GetLoginMemberId();

            // ViewModelを取得
            var viewModel = this.workerService.GetViewModel(
                                                             memberId,
                                                             keyword,
                                                             usersearchcount,
                                                             UserSearchViewModel.INITIAL_PAGE_SIZE,
                                                             this.systemDatetimeService.TargetYear,
                                                             this.systemDatetimeService.TargetMonth);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 対象メンバーをフォローする
        /// </summary>
        /// <param name="followingMemberId"></param>
        [HttpPost]
        public ActionResult Follow(long followingMemberId)
        {
            string result = null;

            try
            {
                long memberId = this.GetLoginMemberId();

                Utils.follow(memberId, followingMemberId);

                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// フォロー対象からフォローを外す
        /// </summary>
        /// <param name="followingMemberId"></param>
        [HttpPost]
        public ActionResult UnFollow(long followingMemberId)
        {
            string result = null;

            try
            {
                long memberId = this.GetLoginMemberId();

                Utils.unfollow(memberId, followingMemberId);

                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ログインユーザのMemberIDを取得
        /// </summary>
        /// <returns></returns>
        private long GetLoginMemberId()
        {
            // HACK: 共通化されるまでの仮メソッドです

            long memberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
            {
                memberId = Convert.ToInt64(currentUser.ToString());
            }

            return memberId;
        }
    }
}